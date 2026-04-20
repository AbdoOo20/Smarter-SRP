using System.Collections.ObjectModel;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Global.Requests;
using Leapay.Client.ClientModels.Ui;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Misc.Configuration;
using Leapay.Client.Wrappers;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SmartErpWin.Core;
using SmartErpWin.Core.Extensions;
using SukiUI.Controls;

namespace Leapay.Client.UserInterface.Common.HomePage;

public partial class HomePageViewModel : BaseRefreshableViewModel<DtoBase>
{
    private readonly PageNavigationService _pageNavigationService;
    protected override string Endpoint => "";
    
    [ObservableProperty]
    public partial ObservableCollection<DashboardCardModel> Cards { get; set; } = []; 
    
    [ObservableProperty]
    public partial ObservableCollection<ItemProfitCardModel> ItemSales { get; set; } = []; 
    
    [ObservableProperty]
    public partial ObservableCollection<ItemProfitCardModel> StorageSales { get; set; } = []; 
    
    [ObservableProperty]
    public partial ObservableCollection<NavigationViewItemWrapper> FavouritedViews { get; set; } = []; 
    
    [ObservableProperty]
    public partial string? WelcomeText { get; set; }
    
    [ObservableProperty]
    public partial bool FinancialStatsCardsNoticeVisible { get; set; }
    
    [ObservableProperty]
    public partial DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
    
    [ObservableProperty]
    public partial DateTimeOffset EndDate { get; set; } = DateTimeOffset.Now.AddDays(1);
    
    [ObservableProperty]
    public partial bool EnterCustom { get; set; }
    
    [ObservableProperty]
    public partial bool ProfitsExist { get; set; }
    
    [ObservableProperty]
    public partial bool HasPermissions { get; set; }
    
    [ObservableProperty]
    public partial string GreenIndicator { get; set; }
    
    [ObservableProperty]
    public partial string YellowIndicator { get; set; }
    
    [ObservableProperty]
    public partial string RedIndicator { get; set; }
    
    [ObservableProperty]
    public partial StaticComboVm<TimeFilter> TimeFilterCombo { get; set; } = new();
    
    [ObservableProperty]
    public partial StaticComboVm<ChartFilter> ChartsFilterCombo { get; set; } = new();
    
    public HomePageViewModel(PageNavigationService pageNavigationService)
    {
        _pageNavigationService = pageNavigationService;
        SetUi();
    }

    public event EventHandler? FavouritedItemsRefreshed;

    private bool _hasInitialized = false;
    
    private void SetUi()
    {
        SetWelcomeText();
        SetEvents();
        
        ContentHelper.MenuItemsLoaded += (_, _) =>
        {
            RefreshFavouritedViews();
        };
        
        TimeFilterCombo.Set(TimeFilter.LastDay, OnFilterChanged);
        ChartsFilterCombo.Set(ChartFilter.StaticNumbers, OnChartFilterChanged);
        
        _hasInitialized = true;
    }
    
    private void OnChartFilterChanged(ChartFilter obj)
    {
        GreenIndicator = obj is ChartFilter.SalesPercentages ? this["GrossMargin"] : this["TotalProfit"];
        YellowIndicator = obj is ChartFilter.SalesPercentages ? this["RoiStat"] : this["TotalSales"];
        RedIndicator = obj is ChartFilter.SalesPercentages ? this["CostRatio"] : this["TotalPurchases"];
        
        if(_hasInitialized)
            LoadCards();
    }

    private void OnFilterChanged(TimeFilter filter)
    {
        var decrement = filter switch
        {
            TimeFilter.LastDay => 1,
            TimeFilter.LastThreeDays => 3,
            TimeFilter.LastWeek => 7,
            TimeFilter.LastMonth => 30,
            TimeFilter.LastYear => 365,
            TimeFilter.LastThreeMonths => 90,
            TimeFilter.LastSixMonths => 180,
            TimeFilter.Custom => 0,
            TimeFilter.LastTwoWeeks => 14,
            TimeFilter.AllTime => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(filter), filter, null)
        };

        Dispatcher.UIThread.Post(() =>
        {
            switch (filter)
            {
                case TimeFilter.Custom:
                    EnterCustom = true;
                    break;
                case TimeFilter.AllTime:
                    StartDate = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
                    EnterCustom = false;
                    break;
                default:
                    StartDate = DateTimeOffset.Now.Date.AddDays(-1 * (decrement - 1));
                    EnterCustom = false;
                    break;
            }

            if (filter is not TimeFilter.Custom)
            {
                EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if(_hasInitialized) LoadCards();
            }
        }, DispatcherPriority.Loaded);
    }

    partial void OnStartDateChanged(DateTimeOffset value)
    {
        LoadCards();
    }

    partial void OnEndDateChanged(DateTimeOffset value)
    {
        LoadCards();
    }

    private void SetEvents()
    {
        if (Session != null)
            Session.UserInfoChanged += (_, _) => SetWelcomeText();
    }

    private void SetWelcomeText()
    {
        if (Session?.UserInfo is null)
            return;

        Dispatcher.UIThread.Post(async void () =>
        {
            var company = App.GetService<CompanyInfoPageViewModel>();
            await company.RefreshCompanyDataAsync();
            
            WelcomeText = company.CompanyInfo.Name;
        });
    }

    [RelayCommand]
    public void LoadCards()
    {
        HasPermissions = Session.JudgeSpecificAccess("StatementsNavigator.SalesItemsProfit2-Show");

        if (!HasPermissions)
            return;
        
        var items = new List<DashboardCardModel>();
        var overall = new DashboardCardModel();
        var grossMarginCard = new DashboardCardModel();
        var costRatioCard = new DashboardCardModel();
        var roiCard = new DashboardCardModel();
        var oldCount = Cards.Count;
        
        items.AddRange(overall, grossMarginCard, costRatioCard, roiCard);
        items.Observe(Cards);
        ItemSales.Clear();
        StorageSales.Clear();
        Series.Clear();
        
        if (oldCount > 0) // update is requested, reanimate UI in code-behind
        {
            OnSynchronisationRequestedAsync(CodeBook.RefCom);
        }
        
        decimal totalCostDuringPeriod = 0;
        decimal totalSalesDuringPeriod = 0;
        decimal profitDuringPeriod = 0;
        decimal grossMargin = 0;   // 25%
        decimal costRatio = 0;  // 75%
        decimal roi = 0;            // 33.3%
        
        DynamicReports? dataDuringPeriod = new();

        SetRefreshState(true);
        var loading = Task.Run(async () =>
        {
            dataDuringPeriod = (await SendByQueryAsync(new ItemInfoQueryRequest
            {
                StartDate = StartDate.Date,
                EndDate = EndDate.Date
            }, "Reports/SalesItemsProfit2")).Data;

            totalCostDuringPeriod = ParseDecimal(dataDuringPeriod, "TotalPurchase");
            totalSalesDuringPeriod = ParseDecimal(dataDuringPeriod, "TotalSales");
            profitDuringPeriod = ParseDecimal(dataDuringPeriod, "TotalProfit");
            
            roi = CalculatePercentagesForPeriod(profitDuringPeriod, totalCostDuringPeriod, totalSalesDuringPeriod, out costRatio, out grossMargin);
            profitDuringPeriod = totalSalesDuringPeriod - totalCostDuringPeriod;
            
            UpdateItemProfits(dataDuringPeriod, profitDuringPeriod);
        });

        _ = loading.ContinueWith(_ =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                overall.Visible = totalSalesDuringPeriod > 0 || totalCostDuringPeriod > 0;
                FinancialStatsCardsNoticeVisible = !overall.Visible;
                grossMarginCard.Visible = overall.Visible;
                costRatioCard.Visible = overall.Visible;
                roiCard.Visible = overall.Visible;
                ProfitsExist  = overall.Visible;
                FillChart(dataDuringPeriod.CustomData);
                SetRefreshState(false);
            });
        });
        
        _ = overall.LoadDataAsync(loading, () => new StackPanel
        {
            Spacing = 5,
            Children =
            {
                GetTextCard(totalCostDuringPeriod, "TotalPurchases"),
                GetTextCard(totalSalesDuringPeriod, "TotalSales"),
                GetTextCard(profitDuringPeriod, "TotalProfit", ContentHelper.GetLightGreenGradient())
            }
        });
        
        _ = grossMarginCard.LoadDataAsync(loading, () => GetWaveCard(grossMargin, "GrossMargin", ContentHelper.GetLightGreenGradient()));
        _ = costRatioCard.LoadDataAsync(loading, () => GetWaveCard(costRatio, "CostRatio"));
        _ = roiCard.LoadDataAsync(loading, () => GetWaveCard(roi, "RoiStat", ContentHelper.GetLightGreenGradient()));
        return;

        decimal ParseDecimal(DynamicReports? dynamicReports, string val)
        {
            try
            {

                return decimal.Parse(dynamicReports.CustomData
                    .GetValueOrDefault(val, ""));
            }
            catch (Exception x)
            {
                Error(x.ToString());
            }

            return 0;
        }
    }

    private static decimal CalculatePercentagesForPeriod(decimal profitDuringPeriod, decimal totalCostDuringPeriod,
        decimal totalSalesDuringPeriod, out decimal costRatio, out decimal grossMargin)
    {
        decimal roi;
        roi = profitDuringPeriod / totalCostDuringPeriod * 100;
        costRatio = totalCostDuringPeriod / totalSalesDuringPeriod * 100;
        grossMargin = profitDuringPeriod / totalSalesDuringPeriod * 100;
        return roi;
    }

    private void UpdateItemProfits(DynamicReports? salesReq, decimal profit)
    {
        var profitPerItems = new List<ItemProfitCardModel>();
        var profitPerStorage = new List<ItemProfitCardModel>();

        foreach (var data in salesReq?.Data?.ToList()[1..^1].Take(30) ?? [])
        {
            _ = decimal.TryParse(data.Row?.LastOrDefault(), out var sales);
            var itemName = data.Row?.ElementAtOrDefault(1) + $"\n{data.Row?.ElementAtOrDefault(3)} • ";
            var storageName = data.Row?.ElementAtOrDefault(2);

            ProcessItemModelList(profitPerItems, profit, itemName, sales);
            ProcessItemModelList(profitPerStorage, profit, storageName, sales);
        }

        Dispatcher.UIThread.Post(() =>
        {
            profitPerItems.OrderByDescending(s => s.ItemSalesPercentage)
                .Observe(ItemSales);
            
            profitPerStorage.OrderByDescending(s => s.ItemSalesPercentage)
                .Observe(StorageSales);
        });
    }

    private static void ProcessItemModelList(List<ItemProfitCardModel> list, decimal profit, string? name, decimal sales)
    {
        var exists = list.FirstOrDefault(p => p.Name == name);

        if (exists != null)
        {
            exists.ItemSalesNum += sales;
            exists.ItemSalesPercentage = (double)(exists.ItemSalesNum / profit * 100);
            exists.ItemSales = $"({exists.ItemSalesNum.ToDottedCommaedNumber()})";
                
            list = list.Where(x => x.Name != name).ToList();
            list.Add(exists);
        }
        else
        {
            list.Add(new()
            {
                Name = name,
                ItemSalesNum = sales,
                ItemSales = $"({sales.ToDottedCommaedNumber()})",
                ItemSalesPercentage = (double)(sales / profit * 100)
            });
        }
    }

    private GlassCard GetTextCard(decimal profit, string title, IBrush? brush = null)
    {
        var card = new GlassCard
        {
            IsOpaque = true,
            Padding = new(8),
            Content = new Viewbox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                StretchDirection = StretchDirection.DownOnly,
                Child = new TextBlock
                {
                    FontSize = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = this[title] + ": " + profit.ToDottedCommaedNumber()
                }
            }
        };

        if (brush is not null)
        {
            card.IsOpaque = false;
            card.Background = brush;
        }

        return card;
    }

    private StackPanel GetWaveCard(decimal grossMargin, string title, IBrush? brush = null)
    {
        var card = new StackPanel
        {
            Spacing = 5
        };
        var item = new GlassCard
        {
            IsOpaque = brush is null,
            Padding = new(8),
            Content = new Viewbox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                StretchDirection = StretchDirection.DownOnly,
                Child = new TextBlock
                {
                    Padding = new(5),
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = this[title]
                }
            }
        };
        card.Children.Add(item);
        
        card.Children.Add(new Viewbox
        {
            Child = new WaveProgress
            {
                Value = (double)grossMargin
            }
        });

        if (brush is not null)
            item.Background = brush;
        
        return card;
    }

    private void RefreshFavouritedViews()
    {
        ContentHelper.GetFavouritedViews().Observe(FavouritedViews);
        FavouritedItemsRefreshed?.Invoke(this, EventArgs.Empty);
    }
    
    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (args.FirstOrDefault() is CodeBook.RefData)
        {
            RefreshFavouritedViews();
        }

        if (args.FirstOrDefault() is not CodeBook.RefCom)
        {
            LoadCards();
        }
        
        return base.OnSynchronisationRequestedAsync(args);
    }

    [ObservableProperty]
    public partial ObservableCollection<ISeries> Series { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Axis> XAxes { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<Axis> YAxes { get; set; } = [];

    private void FillChart(Dictionary<string, string> data)
    {
        var periods = new List<PeriodCalculationResult>();

        foreach (var keyVal in data.Skip(3))
        {
            periods.Add(PeriodCalculationResult.ParseString(keyVal.Value, periods.LastOrDefault() ?? new()));
        }

        Series =
        [
            new LineSeries<DateTimePoint>
            {
                Values = GetValues(periods, p => ChartsFilterCombo.Is(ChartFilter.SalesPercentages)
                    ? p.CostTimesQuantity / p.SalesTimesQuantity * 100 : p.CostTimesQuantity),
                Fill = null,
                GeometryStroke = new SolidColorPaint(SKColors.Red, 1),
                Stroke = new SolidColorPaint(SKColors.Red, 2)
            },
            new LineSeries<DateTimePoint>
            {
                Values = GetValues(periods, p => ChartsFilterCombo.Is(ChartFilter.SalesPercentages)
                    ? p.TotalProfit / p.CostTimesQuantity * 100 : p.SalesTimesQuantity),
                Fill = null,
                GeometryStroke = new SolidColorPaint(SKColors.Yellow, 1),
                Stroke = new SolidColorPaint(SKColors.Yellow, 2)
            },
            new LineSeries<DateTimePoint>
            {
                Values = GetValues(periods, p => ChartsFilterCombo.Is(ChartFilter.SalesPercentages)
                    ? p.TotalProfit / p.SalesTimesQuantity * 100 : p.TotalProfit),
                Fill = null,
                GeometryStroke = new SolidColorPaint(SKColors.Green, 1),
                Stroke = new SolidColorPaint(SKColors.Green, 2)
            }
        ];

        var scopeLowerThanDay = EndDate.DateTime - StartDate.DateTime <= TimeSpan.FromDays(1);
        var format = scopeLowerThanDay ? "h:mm:ss tt" : "MM/dd/yyyy";
        
        XAxes =
        [
            new Axis
            {
                Labeler = value =>
                {
                    if(value < 0)
                        return "\n";

                    return new DateTime((long)value).ToString(format);
                }, 
                
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                LabelsRotation = 0 
            }
        ];
        YAxes =
        [
            new Axis
            {
                Labeler = value => value.ToString("F")
            }
        ];
        return;

        List<DateTimePoint> GetValues(List<PeriodCalculationResult> periodsVal,
            Func<PeriodCalculationResult, double> selector)
            => periodsVal.Select(period => new DateTimePoint(period.EndDate, selector(period))).ToList();
    }
}