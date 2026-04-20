using AutoMapper;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using SmartiErpWin.Client.UserInterface.Stores.CollectionPrice._00;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Stores.CollectionPrice;

public partial class CollectionPricesPageViewModel : BaseViewModel<DtoItem>, INavigationAware


//  BaseViewModel<DtoItem>
//  BasePageViewModel<DtoItem> : BaseViewModel

//  BaseDetailsEntryViewModel<DtoItem> : BaseEntryViewModel<DtoItem> : BaseViewModel
//  BaseEntryViewModel<DtoItem> : BaseViewModel
//  BasePageEntryViewModel<T, D> : BaseRefreshablePageViewModel :   BasePageViewModel : BaseViewModel
{

    public override bool IsEnabled => true;

    protected override string Endpoint => "Item";

    private IMapper _mapper;

    [ObservableProperty]
    private bool _isOpened = false;

    [ObservableProperty]
    public List<DtoItem> _items = [];

    [ObservableProperty]
    public bool _isRefreshing = false;

    [ObservableProperty]
    public bool _actionsEnabled = false;

    [ObservableProperty]
    private Visibility _ringVisibility = Visibility.Collapsed;

    [ObservableProperty]
    public string _searchText;

    private Wpf.Ui.Controls.DataGrid DataGrid;

    public CollectionPricesPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        IMapper mapper,
        LocalisationService localisationService) : base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);

        _mapper = mapper;
        //OnItemEdit += OnEdit;
        _isOpened = true;
    }

    public void SetDataGrid(Wpf.Ui.Controls.DataGrid dataGrid)
    {
        this.DataGrid = dataGrid;
        if (DataGrid is not null)
        {
            //DataGrid.ItemsSource = null;
            DataGrid.CanUserSortColumns = false;
        }
        //Task.Run(Refresh);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        try
        {
            Items = [];
            IsRefreshing = true;
            ActionsEnabled = false;
            RingVisibility = Visibility.Visible;
            var list = await this.GetAllAsync(text: SearchText);
            Items = list.Data.OrderBy(s => s.Name).ToList();
        }
        catch { }
        finally
        {
            IsRefreshing = false;
            ActionsEnabled = true;
            RingVisibility = Visibility.Collapsed;
        }
    }

    [RelayCommand]
    private async void OnAdd()
    {
        var selectedPrices = Items.Select(S => new ChangePriceRequest { ItemId = S.Id, Price = S.Price }).ToList();
        var response = await PostSaveSpecificAsync(selectedPrices, "ChangePrice");
        if (!response.Status)
        {
            // Error
        }
        else
        {
            await Refresh();
        }
    }

    [RelayCommand]
    private void OnPrint()
    {
        if (DataGrid is null) return;

        CollectionPriceSettings collectionPriceSettings = new();

        if (collectionPriceSettings.ShowDialog() == true)
        {
            // DataGrid.ItemsSource = null;
            // DataGrid.CanUserSortColumns = false;
            var amount = collectionPriceSettings.Amount ?? 0;
            var isPercent = collectionPriceSettings.UsePercent;
            var usePercent = collectionPriceSettings.UsePercent;
            var itms = Items.ToList();
            Items = [];
            for (int i = 0; i < itms.Count; i++)
            {
                var _itm = itms[i];
                if (isPercent)
                {
                    amount = Math.Round(_itm.Price * amount / 100, 3);
                }
                if (_itm.Price + amount < 0)
                    _itm.Price = 0;
                else
                    _itm.Price += amount;
                //DataGrid.Items.Add(Items[i]);
            }
            Items = itms;
            // DataGrid.ItemsSource = Items;
        }
    }

    public Task OnNavigatedToAsync()
    {
        if (IsOpened)
            _ = Task.Run(Refresh);

        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}