using System.Collections.ObjectModel;
using System.Windows.Controls;
using AutoMapper;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.PurchasesModels;
using SmartiErpWin.Client.Helpers;

namespace SmartiErpWin.Client.UserInterface.Purchases.PurchaseInvoices;

public partial class PurchasesInvoiceEntryViewModel : BasePageEntryViewModel<DtoPurchasesInvoiceModel, DtoPurchasesInvoiceDetailsModel>
{
    protected override string Endpoint => "Purchase";
    
    private readonly IMapper _mapper;

    [ObservableProperty]
    private Visibility _paymentMethodVisibility = Visibility.Collapsed;
    
    [ObservableProperty]
    private Visibility _supplierVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private long _invoiceNumber;

    [ObservableProperty]
    private StackPanel? _secondaryDataPanel;

    [ObservableProperty]
    private ObservableCollection<DtoPurchasesInvoiceCostsModel> _costsModels = [];
    
    public PurchasesInvoiceDetailsEntryWindow? EntryState { get; set; }

    public PurchasesInvoiceCostsEntryWindow? SecondaryEntryState { get; set; }
    
    [ObservableProperty]
    private decimal _totalPrice;
    
    public event EventHandler<DtoPurchasesInvoiceCostsModel>? OnSecondaryDataGridSelectionChanged;

    public PurchasesInvoiceEntryViewModel(Window window,
        LocalisationService localisationService,
        DtoPurchaseInvoice? model,
        EntryType entryType,
        HttpClient client,
        IMapper mapper,
        Type pageType): base(client)
    {
        InitBaseModel(localisationService, null, null);
        Model = new DtoPurchasesInvoiceModel
        {
            DtoPurchasesInvoice = model ?? new DtoPurchaseInvoice
            {
                Date = DateTime.Now
            }
        };
        
        _mapper = mapper;
        EntryType = entryType;
        EntryWindow = window;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);
        
        if (EntryType == EntryType.Add)
        {
            Model.DtoPurchasesInvoice.Date = DateTime.Now;
        }
        
        Model.DtoPurchasesInvoice.Details ??= [];
        Model.DtoPurchasesInvoice.Costs ??= [];
        
        OnItemDelete += OnDelete;

        OnDataGridSelectionChanged += OnDataSelectionChanged;
        OnSecondaryDataGridSelectionChanged += OnSecondaryDataSelectionChanged;
        
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        ComboModelBase.SetStaticItemsInternal<PeymentType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItem(ComboModelBase.ConvertToDisplay((int)Model.DtoPurchasesInvoice.PeymentType));

        if (EntryType == EntryType.Add)
        {
            var request = await RequestSpecificAsync<long>("Number");
            InvoiceNumber = request.Data;
        }
        else
        {
            InvoiceNumber = Model.DtoPurchasesInvoice.Number;
        }

        CheckVisibilities();

        ComboModelBase.OnStaticComboItemChanged += () =>
        {
            CheckVisibilities();
            return default;
        };

        ComboModelBase.OnApiComboItemChanged += () =>
        {
            CheckVisibilities();
            return default;
        };

        var categories = (await RequestComboModelsAsync<ComboModel>("Items")).Data;

        var data = new List<DtoPurchasesInvoiceDetailsModel>();

        foreach (var dto in Model.DtoPurchasesInvoice.Details)
        {
            var model = _mapper.Map<DtoPurchasesInvoiceDetailsModel>(dto);

            model.ItemName = categories.FirstOrDefault(u => u.Value == dto.ItemId.Value.ToString())?
                .Display;
            model.TemporaryId = dto.Id;

            var units = (await RequestComboModelsAsync<ComboModel>("Units", dto.ItemId.ToString())).Data;

            model.UnitName = units.FirstOrDefault(u => u.Value == dto.UnitId.Value.ToString())?
                .Display;

            data.Add(model);
        }

        var secondaryData = Model.DtoPurchasesInvoice.Costs.Select(dto =>
        {
            var model = _mapper.Map<DtoPurchasesInvoiceCostsModel>(dto);

            model.TemporaryId = dto.Id;
            model.Ammount = dto.Ammount;

            return model;
        }).ToList();

        Model.PurchasesInvoiceDetailModels.AddRange(data);
        Model.PurchasesInvoiceDetailCosts.AddRange(secondaryData);

        CalculatePrice();

        if (refresh)
        {
            await RefreshAsync();
        }
    }

    public void CheckVisibilities()
    {
        PaymentMethodVisibility = ComboModelBase.AssertThatStaticIsAt().ApplyVisibility();
        SupplierVisibility = ComboModelBase.AssertThatApiIsSelected(2).ApplyVisibility();
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model.PurchasesInvoiceDetailModels);
        RefreshDataGrid();
        
        if (refreshCombos)
        {
            await RequestApiComboItemsFromInnerEndpointAsync("Stores");
            ComboModelBase.SetComboModelOrDefault(Model.DtoPurchasesInvoice.StoreId, 0, true);
            
            await RequestApiComboItemsFromInnerEndpointAsync("Supplier", 1);
            ComboModelBase.SetComboModelOrDefault(Model.DtoPurchasesInvoice.SupplierId, 1);
            
            await RequestApiComboItemsFromInnerEndpointAsync("Safes", 2);
            ComboModelBase.SetComboModelOrDefault(Model.DtoPurchasesInvoice.SafeId, 2, true);
        }
        
        SynchroniseTypeBase<PurchasesInvoiceDetailsEntryViewModel>(new DtoPurchasesInvoiceDetailsModel());
        CalculatePrice();
        //InformComboChange();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoPurchasesInvoiceDetailsModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid()),
        };
        
        Model.PurchasesInvoiceDetailModels.Add(@new);
        SetData(Model.PurchasesInvoiceDetailModels);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }
    
    [RelayCommand]
    private void OnSecondaryAdd()
    {
        var @new = new DtoPurchasesInvoiceCostsModel
        {
            TemporaryId = Identifier.Create(Guid.NewGuid())
        };
        
        Model.PurchasesInvoiceDetailCosts.Add(@new);
        RefreshSecondaryDataGrid();
        var dataGrid = DataGridServices.FindDataGrid(SecondaryDataPanel);

        SelectInSpecificDataGrid(dataGrid, @new);
    }
    
    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoPurchasesInvoiceDetailsModel>(sender);
        Edit(model);
    }

    private void OnDataSelectionChanged(object? sender, DtoPurchasesInvoiceDetailsModel e)
    {
        Edit(e);
        EntryState?.FocusFirst();
    }
    
    private void OnSecondaryDataSelectionChanged(object? sender, DtoPurchasesInvoiceCostsModel e)
    {
        SecondaryEdit(e);
        SecondaryEntryState?.FocusFirst();
    }
    
    private void Edit(DtoPurchasesInvoiceDetailsModel model)
    {
        SynchroniseTypeBase<PurchasesInvoiceDetailsEntryViewModel>(model);
    }
   
    private void SecondaryEdit(DtoPurchasesInvoiceCostsModel model)
    {
        SynchroniseTypeBase<PurchasesInvoiceCostsEntryViewModel>(model);
    }
    
    protected override async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;
        
        var model = DataGridServices.FindModel<DtoPurchasesInvoiceDetailsModel>(sender);

        if (model is null)
            return;

        Model.PurchasesInvoiceDetailModels = HelperServices
            .RemoveItemFromModelList(Model.PurchasesInvoiceDetailModels.AsEnumerable(), model)
            .OfType<DtoPurchasesInvoiceDetailsModel>()
            .ToList();

        await RefreshAsync(false);
    }

    private void UpdateListing(DtoPurchasesInvoiceDetailsModel model)
    {
        var list = Model.PurchasesInvoiceDetailModels;
        
        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            list.Add(model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        Model.PurchasesInvoiceDetailModels = list;
        SetData(Model.PurchasesInvoiceDetailModels);
        RefreshDataGrid();
    }

    private void UpdateSecondaryListing(DtoPurchasesInvoiceCostsModel model)
    {
        var list = Model.PurchasesInvoiceDetailCosts;
        
        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            //list.Add(model);
            list.Insert(0, model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        Model.PurchasesInvoiceDetailCosts = list;
        //SetData(Model.SalesInvoiceDetailModels);
        RefreshDataGrid();
        //ScrollTo(index);
    }

    public void ShowSecondaryDataPanel(StackPanel panel)
    {
        SecondaryDataPanel = panel;
        RefreshSecondaryDataGrid();
    }
    
    protected override void RefreshDataGrid()
    {
        base.RefreshDataGrid();
        RefreshSecondaryDataGrid();
    }

    private async void RefreshSecondaryDataGrid()
    {
        if (SecondaryDataPanel is null)
            return;
        
        SecondaryDataPanel.Children.Clear();
        
        if (Model.PurchasesInvoiceDetailCosts.Count < 1)
        {
            var block = DataGridServices.GetEmptyItemsControl(MinDataGridHeight);
            SecondaryDataPanel.Children.Add(block);
            
            return;
        }
        
        var dataGrid = await DataGridServices
            .CreateDataGridForDtoModelAsync(LocaleService,
                Model.PurchasesInvoiceDetailCosts.ToList(),
                onDelete: OnSecondaryDelete);
        
        dataGrid.SelectionMode = DataGridSelectionMode.Single;
        dataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            
        var style = new Style(typeof(DataGridRow));
        var trigger = new Trigger
        {
            Property = DataGridRow.IsSelectedProperty,
            Value = true
        };
        
        trigger.Setters.Add(new Setter(Control.BackgroundProperty,
            System.Windows.Media.Brushes.SlateBlue));
        style.Triggers.Add(trigger);
        dataGrid.RowStyle = style;

        dataGrid.SelectionChanged += (sender, _) =>
        {
            if (dataGrid is { SelectedItem: DtoPurchasesInvoiceCostsModel item })
            {
                OnSecondaryDataSelectionChanged(this, item);
            }
        };
        
        SecondaryDataPanel.Children.Add(dataGrid);
    }
    
    private async void OnSecondaryDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;
        
        var model = DataGridServices.FindModel<DtoPurchasesInvoiceCostsModel>(sender);

        if (model is null)
            return;

        Model.PurchasesInvoiceDetailCosts = HelperServices
            .RemoveItemFromModelList(Model.PurchasesInvoiceDetailCosts.AsEnumerable(), model)
            .OfType<DtoPurchasesInvoiceCostsModel>()
            .ToList();

        _ = RefreshAsync(false);
    }
    
    protected override void PreSaveActions()
    {
        Model.DtoPurchasesInvoice.StoreId = ComboModelBase.GetSelectedItemId();
        Model.DtoPurchasesInvoice.SupplierId = ComboModelBase.GetSelectedItemId(1);
        Model.DtoPurchasesInvoice.PeymentType = ComboModelBase.ConvertCurrentToEnum<PeymentType>();
        Model.DtoPurchasesInvoice.SafeId = ComboModelBase.GetSelectedItemId(2);
        
        Model.DtoPurchasesInvoice.Details = [];

        foreach (var item in Model.PurchasesInvoiceDetailModels)
        {
            Model.DtoPurchasesInvoice.Details.Add(_mapper.Map<DtoPurchasesInvoiceDetailsModel>(item));
        }
        
        Model.DtoPurchasesInvoice.Costs = [];

        foreach (var item in Model.PurchasesInvoiceDetailCosts)
        {
            Model.DtoPurchasesInvoice.Costs.Add(_mapper.Map<DtoPurchasesInvoiceCostsModel>(item));
        }
    }

    private void CalculatePrice()
    {
        TotalPrice = Model.PurchasesInvoiceDetailModels
            .Sum(d => d.Price * d.Quantity) + Model.PurchasesInvoiceDetailCosts
            .Sum(d => d.Ammount);
    }

    protected override async Task SaveAsync()
    {
        if (Model is null)
            return;
        
        PreSaveActions();
        
        var result = await PostSaveSpecificAsync(Model.DtoPurchasesInvoice);
        EventHelper.InvokePurchases(new(result.Data));
        EntryWindow?.Close();
        
        SynchroniseTypeBase(PageType);
    }
    
    [RelayCommand]
    private async Task OnSavePrintAsync()
    {
        await SaveAsync();
        //ReportRunner.OnPurchesComplate(this);
        //EventHelper.InvokePurchases(result.Data);
        //EventHelper.InvokePurchases(new(Model.DtoPurchasesInvoice));
    }
    
    public sealed override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.FirstOrDefault())
        {
            case CodeBook.RefData:
                RefreshDataGrid();
                break;
            case DtoPurchasesInvoiceDetailsModel model:
                UpdateListing(model);
                break;
            case DtoPurchasesInvoiceCostsModel costModel:
                UpdateSecondaryListing(costModel);
                break;
            default:
                await RefreshAsync(false);
                break;
        }
        
        CalculatePrice();
    }
}