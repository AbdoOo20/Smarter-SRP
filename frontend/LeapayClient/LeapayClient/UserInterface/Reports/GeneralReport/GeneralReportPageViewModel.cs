using System.Collections.ObjectModel;
using System.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Common;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Global.Requests;
using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Mapster;
using Material.Icons.Avalonia;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Helpers;
using SukiUI.Controls;

namespace Leapay.Client.UserInterface.Reports.GeneralReport;

public record ButtonColumnSingular(string Id, string ColumnName, Action<object, object?> Action, Func<Control> Interface, int IncrementForName);

public partial class GeneralReportPageViewModel : BasePageViewModel<DtoEmployeeAssignment>
{
    protected override string Endpoint => "Reports";

    private string? _innerEndpoint;

    [ObservableProperty]
    public partial StatementQueryRequest? QueryRequest { get; set; }
    
    [ObservableProperty]
    public partial bool AccountTypeVisibility { get; set; }
    
    [ObservableProperty]
    public partial DateTimeOffset StartDate { get; set; }
    
    [ObservableProperty]
    public partial DateTimeOffset EndDate { get; set; }
    
    [ObservableProperty]
    public partial bool ComboVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool AdditionalQueriesExpanderVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool DatesVisibility { get; set; }
    
    [ObservableProperty]
    public partial string? Title { get; set; }

    [ObservableProperty]
    public partial bool IsBackVisible { get; set; } = true;
    
    [ObservableProperty]
    public partial double Width { get; set; }
    
    [ObservableProperty]
    public partial string? CustomEndpoint { get; set; }

    [ObservableProperty]
    public partial string? ComboName { get; set; }

    public List<ButtonColumnSingular> Actions { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<DynamicField> DynamicFields { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<DynamicCombo> DynamicCombos { get; set; } = [];

    private StatementAccountType? _accountType;

    [ObservableProperty]
    public partial StaticComboVm<StatementAccountType> AccountType { get; set; } = new();
    
    public DynamicReports? Data { get; set; }

    private readonly MainWindowViewModel _mainViewModel;

    private WrapPanel? _upperStack;

    private WrapPanel? _lowerStack;

    private WrapPanel? _combosWrap;
    
    private StackPanel? _mainComboPanel;

    public GeneralReportPageViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public void SetXamlObjects(StackPanel grid,
        WrapPanel upper,
        WrapPanel lower,
        WrapPanel combosWrap,
        StackPanel? mainComboPanel)
    {
        DataGridPanel = grid;
        _upperStack = upper;
        _lowerStack = lower;
        _combosWrap = combosWrap;
        _mainComboPanel = mainComboPanel;
    }

    [RelayCommand]
    private void OnBack()
    {
        _mainViewModel.GoBack();
    }

    public async void SetAccounts()
    {
        if (QueryRequest is null)
            return;
        
        _mainComboPanel?.Children.Clear();
        
        var pos = _accountType is null
            ? (int)AccountType.GetSelectedItem()
            : (int)_accountType;

        if (CustomEndpoint is not null)
        {
            var req = await RequestCombosUsingQueryRequestAsync<LookupOf<string, string>>(CustomEndpoint, new QueryRequestBase
            {
                SearchText = pos.ToString()
            });
            var comboModels = req.Data.Select(x => x.Adapt<ComboModel>()).ToList();

            var suggestBox = UiHelpers.CreateSuggestionBox(
                _mainComboPanel,
                ComboName, comboModels,
                onClear: () => { QueryRequest.AccountId = null; },
                actionAdorner: a => a.Background = BackgroundTheme);
            
            suggestBox.OnSuggestionChosen += (sender, of) =>
            {
                QueryRequest.AccountId = Guid.Parse(of.Value);
            };
        }

        QueryRequest.ItemId = pos;
    }

    [RelayCommand]
    private async Task OnQueryAsync()
    {
        if (QueryRequest != null)
        {
            QueryRequest.DynamicFields = DynamicFields.ToList();
            QueryRequest.DynamicCombos = DynamicCombos.ToList();
            QueryRequest.StartDate = StartDate.DateTime;
            QueryRequest.EndDate = EndDate.DateTime;
            QueryRequest.AccountType = AccountType.GetSelectedItem();
        }

        await RefreshAsync(true);
    }

    public async void SetNewAsync<TRequest>(TRequest request,
        string endpoint,
        string? customEndpoint = null,
        StatementAccountType? accountType = null,
        bool areDatesVisible = true,
        double width = 300,
        List<ButtonColumnSingular>? buttons = null)
        where TRequest : StatementQueryRequest
    {
        SetState(true);
        ArgumentNullException.ThrowIfNull(request);

        Width = width;
        QueryRequest = null;
        ComboModelBase = new();

        _innerEndpoint = endpoint;
        _accountType = accountType;
        CustomEndpoint = customEndpoint;
        ComboName = LocaleService?[CustomEndpoint];
        ComboVisibility = (CustomEndpoint is not null).ApplyVisibility();
        DatesVisibility = areDatesVisible.ApplyVisibility();

        Actions = buttons ?? [];
        
        Title = LocaleService?[endpoint];
        QueryRequest = request;
        AccountType.Set(StatementAccountType.All, _ =>
        {
            SetAccounts();
        });
        
        if (QueryRequest.StartDate == default && QueryRequest.EndDate == default)
        {
            TimeHelper.SetForStatement(EntryType.Add, out var start, out var end);

            StartDate = start;
            EndDate = end;

            OnPropertyChanged(nameof(QueryRequest));
        }
        else
        {
            StartDate = request.StartDate;
            EndDate = request.EndDate;
        }
        
        SetDynamics(QueryRequest);
        AccountTypeVisibility = _accountType is null;

        ComboModelBase.SetStaticItemsInternal<StatementAccountType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemIndexInternal(0);

        _upperStack?.Children.Clear();
        _lowerStack?.Children.Clear();
        DataGridPanel?.Children.Clear();
        
        await OnQueryAsync();
    }

    [RelayCommand]
    private void OnRequestPrint()
    {
        var dataTable = CreateDataTable(Data);

        for (var index = 0; index < dataTable?.Columns.Count; index++)
        {
            var column = dataTable.Columns[index];
            if (column.ColumnName.Contains(SharedConstants.InternalTableColumn))
            {
                dataTable.Columns.RemoveAt(index);
            }
        }

        QueryRequest.Title = Title;
        QueryRequest.ReportName = _innerEndpoint;
        EventHelper.InvokeReport(new ReportPrintEventArgs(dataTable, QueryRequest, this));
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetState(true);
        SetMargins(null, null);

        try
        {
            var request = await SendByQueryAsync(QueryRequest, _innerEndpoint);

            Data = request.Data;

            if (!string.IsNullOrEmpty(request.ErrorMessage))
                throw new(request.ErrorMessage);
            
            if (Data?.Data is null || Data.Data.Count == 0)
                throw new("ChangeFilter");

            RefreshDataGrid();
        }
        catch (Exception exception)
        {
            if (ShowError)
            {
                ShowResult(LocaleService[exception.Message], string.Empty);
            }
        }
        finally
        {
            SetState(false);
        }
    }

    protected override async void RefreshDataGrid()
    {
        DataGridPanel?.Children.Clear();
        var data = Data.Data.Select(s =>
            s.Row.ToArray()).ToList();

        var displayedData = data.ToList();
        
        var buttonsMatrix = new List<ButtonColumn[]>();
        var headers = data.ElementAtOrDefault(0);
        
        var internalDataEnum = headers?.Where(d => d?
            .StartsWith(SharedConstants.InternalTableColumn) ?? false) ?? [];

        var internalColumns = internalDataEnum as string[] ?? internalDataEnum.ToArray();
        
        if (Actions.Count != 0 && internalColumns.Length != 0)
        {
            var j = 0;
            for (; j < internalColumns.Length; ++j)
            {
                var internalColumn = internalColumns[j];
                var match = Actions.FirstOrDefault(a => internalColumn.Contains(a.Id));

                if (match is null)
                    continue;
                
                var i = headers.IndexOf(internalColumn);
                
                var dataWithoutHeaders = data.Skip(1);
                var dataWithoutHeadersEnum = dataWithoutHeaders as string[][] ?? dataWithoutHeaders.ToArray();

                var setButtons = new List<ButtonColumn>();
                
                foreach (var entry in dataWithoutHeadersEnum)
                {
                    var id = entry.ElementAtOrDefault(i);
                    var increment = match.IncrementForName;
                    var name = entry.ElementAtOrDefault(i + increment) ?? this["ItemWithNoName"];
                    
                    setButtons.Add(new ButtonColumn(match.ColumnName, match.Action, match.Interface, new DtoNamesBase()
                    {
                        Name = name,
                        Description = id
                    }));
                }
                
                buttonsMatrix.Add(setButtons.ToArray());
                /*buttonsMatrix.Add(dataWithoutHeadersEnum
                    .Select(entry => entry.ElementAtOrDefault(i) ?? " ")
                    .Select(internalData => new ButtonColumn(match.ColumnName,
                        match.Action, match.Interface, new DtoNamesBase()
                        {
                            Name = internalData
                        })).ToArray());*/
            }
        }
        
        var dataGrid = await DataGridServices.CreateDataGridForStringModelAsync(LocaleService,
            displayedData, buttonsMatrix, BackgroundTheme, this);

        DataGridPanel?.Children.Add(new GlassCard { Content = dataGrid });

        SetMargins(Data?.Uppers, Data?.Lowers);
    }
    
    partial void OnStartDateChanged(DateTimeOffset value)
    {
        if (IsRefreshing)
            return;

        _ = OnQueryAsync();
    }

    partial void OnEndDateChanged(DateTimeOffset value)
    {
        if (IsRefreshing)
            return;

        _ = OnQueryAsync();
    }
    
    private static DataTable? CreateDataTable(DynamicReports? report)
    {
        if (report is null)
            return null;

        var dataTable = new DataTable();
        var headers = report.Data?.ElementAtOrDefault(0).Row;
        foreach (var header in headers ?? [])
        {
            dataTable.Columns.Add(header.Replace(".", "-"));
        }

        for (var i = 1; i < report.Data?.Count; i++)
        {
            var data = report.Data.ElementAtOrDefault(i).Row;
            var row = dataTable.NewRow();

            for (var j = 0; j < data?.Count; j++)
            {
                var dataAtRow = data.ElementAtOrDefault(j);
                row[j] = dataAtRow;
            }

            dataTable.Rows.Add(row);
        }

        return dataTable;
    }

    private void SetMargins(ICollection<DynamicText>? upper, ICollection<DynamicText>? lower)
    {
        SetMargin(_upperStack, upper);
        SetMargin(_lowerStack, lower);
    }

    private void SetMargin(WrapPanel? stack, ICollection<DynamicText>? margin)
    {
        if (stack is null)
            return;

        stack.Children.Clear();

        foreach (var text in margin ?? [])
        {
            stack.Children.Add(CreateBar(text.Title, text.Value));
        }
    }

    private GlassCard CreateBar(string? title, string? text)
    {
        return new GlassCard
        {
            FlowDirection = FlowDirection,
            MaxWidth = 500,
            Content = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new MaterialIcon
                            {
                                Kind = MaterialIconKind.Info,
                                Margin = new Thickness(0, 3, 0, 0)
                            },
                            new TextBlock
                            {
                                FontWeight = FontWeight.Bold,
                                FontSize = 16,
                                Margin = new Thickness(10, 0, 0, 0),
                                Text = title,
                                TextWrapping = TextWrapping.Wrap
                            }
                        }
                    },
                    new TextBlock
                    {
                        Margin = new Thickness(0, 5, 0, 0),
                        Text = text,
                        TextWrapping = TextWrapping.Wrap
                    }
                }
            }
        };
    }

    private async void SetDynamics(StatementQueryRequest query)
    {
        SetDynamicFields(query);
        await SetDynamicCombosAsync(query);

        AdditionalQueriesExpanderVisibility = (DynamicFields.Any() || DynamicCombos.Any())
            .ApplyVisibility();
    }

    private void SetDynamicFields(StatementQueryRequest query)
    {
        query.DynamicFields.Observe(DynamicFields);
    }

    private async Task SetDynamicCombosAsync(StatementQueryRequest query)
    {
        _combosWrap?.Children.Clear();

        DynamicCombos = [];
        
        foreach (var combo in query.DynamicCombos)
        {
            var newCombo = new DynamicCombo(combo.Title, combo.DataEndpoint, combo.QueryName, combo.IsSearchable, combo.ComboWidth);
            
            var req = await RequestComboModelsAsync<LookupOf<string, string>>(combo.DataEndpoint);
            var comboModels = req.Data.Select(x => x.Adapt<ComboModel>()).ToList();
            
            var suggestBox = UiHelpers.CreateSuggestionBox(
                _combosWrap,
                combo.Title,
                comboModels,
                onClear: () => { newCombo.SelectedCombo = null; },
                actionAdorner: a => a.Background = BackgroundTheme);
            
            suggestBox.OnSuggestionChosen += (_, of) =>
            {
                newCombo.SelectedCombo = of.Adapt<LookupOf<string, string>>();
            };
            
            DynamicCombos.Add(newCombo);
        }
    }
}