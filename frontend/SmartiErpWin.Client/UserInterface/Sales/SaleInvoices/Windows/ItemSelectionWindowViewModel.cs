using System.Collections.ObjectModel;
using AutoMapper;
using Fly.Framework.Shared.Common.Dtos;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Helpers;
using SmartiErpWin.Client.DtoModels;

namespace SmartiErpWin.Client.UserInterface.Sales.Lookup;

public partial class ItemSelectionWindowViewModel : BaseRefreshableViewModel<DtoBase>
{
    public ItemSelectionWindow? Window { get; set; }

    [ObservableProperty]
    private string? _actionText;

    [ObservableProperty]
    private string? _searchText;
    
    private string? _endpoint;
    
    private string? _previousId;

    [ObservableProperty]
    private ComboModel? _selectedItem;

    public event EventHandler? ItemsFetched;
    
    public ItemSelectionWindowViewModel(
        SessionService session,
        LocalisationService localeService,
        HttpClient client,
        IMapper mapper,
        string endpoint,
        string? previousId = null) : base(client)
    {
        InitBaseModel(localeService, session, null);
        ApplicationTitle = LocaleService["SelectItem"];
        ActionText = LocaleService?["Search"];
        _mapper = mapper;
        _endpoint = endpoint;
        _previousId = previousId;
        
        _ = PrepareAsync();
    }

    [ObservableProperty]
    private string? _infoText;

    [ObservableProperty]
    private ObservableCollection<ComboModel> _modelsInView = [];
    
    [ObservableProperty]
    private ObservableCollection<ComboModel> _models = [];
    
    private readonly IMapper _mapper;

    private async Task PrepareAsync()
    {
        SetRefreshState(true);
        
        Endpoint = _endpoint;

        await RequestApiComboItemsFromInnerEndpointAsync("Categories", defaultText: this["All"]);
        ComboModelBase.SetComboModelOrDefault(null);

        await SetAllItemsAsync();

        SetRefreshState(false);
    }
    
    private async Task SetAllItemsAsync()
    {
        var items = await RequestComboModelsAsync<ComboModel>("Items");
        
        items.Data.Observe(Models);
        OnSearch(string.Empty);

        if (_previousId != null)
        {
            SelectedItem = Models.FirstOrDefault(m => m.Value == _previousId);
        }
        
        ItemsFetched?.Invoke(this, EventArgs.Empty);
    }

    public async Task SetItemsByCategoryAsync()
    {
        if (string.IsNullOrEmpty(ComboModelBase.Api1.Value))
        {
            await SetAllItemsAsync();
            return;
        }
        
        var selectedCat = ComboModelBase.Api1.Value; 
        var items = await RequestComboModelsAsync<ComboModel>("Items", selectedCat);
        
        items.Data.Observe(Models);
        OnSearch(string.Empty);
        
        ItemsFetched?.Invoke(this, EventArgs.Empty);
    }

    public void OnSearch(string text)
    {
        var search = SearchHelper.Search(text, Models.ToList());

        search.Observe(ModelsInView);
    }
    
    [RelayCommand]
    private void OnClearSelection()
    {
        SelectedItem = null;
        Window.DialogResult = true;
        Window?.Close();
    }
    
    [RelayCommand]
    private void OnSelect()
    {
        if (SelectedItem is null || string.IsNullOrEmpty(SelectedItem.Value)
                                 || string.IsNullOrEmpty(SelectedItem.Display))
            return;
        
        Window.DialogResult = true;
        Window?.Close();
    }
    
    [RelayCommand]
    private void OnCancel()
    {
        Window.DialogResult = false;
        Window?.Close();
    }
}