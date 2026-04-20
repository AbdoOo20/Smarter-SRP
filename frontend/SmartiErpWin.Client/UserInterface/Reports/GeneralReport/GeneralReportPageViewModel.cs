using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Financial.Enums;
using Fly.Shared.Modules.Financial.Requests;
using Fly.Shared.Modules.Global.Requests;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Helpers;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.ViewModels.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;
using DynamicText = Fly.Shared.Modules.Global.Requests.DynamicText;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace SmartiErpWin.Client.UserInterface.Reports.GeneralReport;

public sealed partial class GeneralReportPageViewModel : BasePageViewModel<StatementQueryRequest>
{
    protected override string Endpoint => "Reports";

    private string? _innerEndpoint;

    [ObservableProperty]
    private StatementQueryRequest? _queryRequest;

    [ObservableProperty]
    private Visibility _accountTypeVisibility;

    [ObservableProperty]
    private Visibility _comboVisibility;

    [ObservableProperty]
    private Visibility _additionalQueriesExpanderVisibility;

    [ObservableProperty]
    private Visibility _datesVisibility;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private double _width;

    [ObservableProperty]
    private string? _customEndpoint;

    [ObservableProperty]
    private string? _comboName;

    [ObservableProperty]
    private ObservableCollection<DynamicField> _dynamicFields = [];

    [ObservableProperty]
    private ObservableCollection<DynamicCombo> _dynamicCombos = [];

    private StatementAccountType? _accountType;

    public DynamicReports? Data { get; set; }

    private readonly MainWindowViewModel _mainViewModel;

    private WrapPanel? _upperStack;

    private WrapPanel? _lowerStack;

    private WrapPanel? _combosWrap;
    
    private StackPanel? _mainComboPanel;

    public GeneralReportPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        MainWindowViewModel mainViewModel) : base(client)
    {
        _mainViewModel = mainViewModel;
        InitBaseModel(localisationService, sessionService, contentDialogService);
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
            ? ComboModelBase.GetStaticPosition()
            : (int)_accountType;

        if (CustomEndpoint is not null)
        {
            var req = await RequestComboModelsAsync<LookupOf<string, string>>(CustomEndpoint);
            var suggestBox = UiHelpers.CreateSuggestionBox(
                _mainComboPanel,
                ComboName, req.Data.ToList(),
                onClear: () => { QueryRequest.AccountId = null; });
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
        }

        await RefreshAsync(true);
    }

    public async void SetNewAsync<TRequest>(TRequest request,
        string endpoint,
        string? customEndpoint = null,
        StatementAccountType? accountType = null,
        bool areDatesVisible = true,
        double width = 300)
        where TRequest : StatementQueryRequest
    {
        ArgumentNullException.ThrowIfNull(request);

        Width = width;
        QueryRequest = null;
        ComboModelBase = new();

        _innerEndpoint = endpoint;
        _accountType = accountType;
        CustomEndpoint = customEndpoint;
        ComboName = LocaleService[CustomEndpoint];
        ComboVisibility = (CustomEndpoint is not null).ApplyVisibility();
        DatesVisibility = areDatesVisible.ApplyVisibility();

        Title = LocaleService[endpoint];
        QueryRequest = request;

        if (QueryRequest.StartDate == default && QueryRequest.EndDate == default)
        {
            TimeHelper.SetForStatement(EntryType.Add, out var start, out var end);

            QueryRequest.StartDate = start;
            QueryRequest.EndDate = end;

            OnPropertyChanged(nameof(QueryRequest));
        }

        SetDynamics(QueryRequest);
        AccountTypeVisibility = _accountType is null ? Visibility.Visible : Visibility.Collapsed;

        ComboModelBase.SetStaticItemsInternal<StatementAccountType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemIndexInternal(0);

        _upperStack?.Children.Clear();
        _lowerStack?.Children.Clear();
        DataGridPanel?.Children.Clear();

        /*var auto = new DynamicAutoSuggestPrefab();
        auto.ItemsSource?.Cast<LookupOf<string, string>>().ToList().Clear();*/

        await OnQueryAsync();
    }

    [RelayCommand]
    private void OnPrint()
    {
        var dataTable = CreateDataTable(Data);
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

        var dataTable = CreateDataTable(Data);

        if (dataTable is null)
            return;

        var dataGrid = await DataGridServices.CreateDataGridForAnunymousModelAsync(
                LocaleService,
                //dataTable
                Data.Data.Select(s => s.Row.ToArray()).ToList()
            );

        DataGridPanel?.Children.Add(new Card { Content = dataGrid });

        SetMargins(Data?.Uppers, Data?.Lowers);
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

    private Card CreateBar(string? title, string? text)
    {
        return new Card
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
                            new SymbolIcon
                            {
                                Symbol = SymbolRegular.Info28,
                                Margin = new Thickness(0, 3, 0, 0)
                            },
                            new TextBlock
                            {
                                FontWeight = FontWeights.Bold,
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
            var suggestBox = UiHelpers.CreateSuggestionBox(
                _combosWrap,
                combo.Title,
                req.Data.ToList(),
                onClear: () => { combo.SelectedCombo = null; });
            
            suggestBox.OnSuggestionChosen += (_, of) =>
            {
                newCombo.SelectedCombo = of;
                /*var dynamicCombo = DynamicCombos.FirstOrDefault(c => c.Title == combo.Title);

                if (dynamicCombo is null)
                {
                    Warning("Dynamic combo update failed. No combo found in the list.");
                    return;
                }

                dynamicCombo.SelectedCombo = of;*/
            };
            
            DynamicCombos.Add(newCombo);
        }
        // Below is a data binding approach, which is buggy. Above is a much simpler C# approach
        // that is more optimized.
        /*DynamicCombos = [];


        for (var i = 0; i < DynamicCombos.Count; i++)
        {
            var item = DynamicCombos[i];
            var req = await RequestComboModelsAsync<LookupOf<string, string>>(item.DataEndpoint);

            item.Combos = new ObservableCollection<LookupOf<string, string>>(req.Data);
            ComboModelBase.AddDefaultTo(item.Combos);
            item.SelectedCombo = item.Combos.FirstOrDefault();
        }*/

        //query.DynamicCombos.Observe(DynamicCombos);
        //foreach (var item in DynamicCombos)
        //{
        //    var req = await RequestComboModelsAsync<LookupOf<string, string>>(item.DataEndpoint);

        //    req.Data.Observe(item.Combos);
        //    ComboModelBase.AddDefaultTo(item.Combos);
        //    item.SelectedCombo = item.Combos.FirstOrDefault();
        //}
    }
}