using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Requests;
using SmartiErpWin.Client.DtoModels;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public abstract partial class BasePageEntryViewModel<T, TD> : BaseRefreshablePageViewModel<TD>
    where TD: class, new()
    where T: class, new()
{
    [ObservableProperty]
    private EntryType _entryType;

    [ObservableProperty]
    private T? _model;
    
    [ObservableProperty]
    private Type? _pageType;

    protected override bool CanSelectInDataGrid => true;

    protected override double MaxDataGridHeight => 100;
    
    protected Window? EntryWindow { get; set; }

    [ObservableProperty]
    private ComboModelBase _comboModelBase = new();

    protected BasePageEntryViewModel(HttpClient client) : base(client)
    {
        MinDataGridHeight = 0;
        Limit = 100;
        CurrentPage = 1;
        SearchText = "";
        MinDataGridHeight = EntryDataGridDefaultHeight;
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        EntryWindow?.Close();
        
        return Task.CompletedTask;
    }
    
    [RelayCommand]
    protected virtual async Task SaveAsync()
    {
        if (Model is null)
            return;
        
        PreSaveActions();
        
        await PostSaveSpecificAsync(Model);
        
        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }
    
    protected virtual void PreSaveActions() { }
    
    protected async Task RequestApiComboItemsFromInnerEndpointAsync(string innerEndpoint, int index = 0)
    {
        var request = await RequestComboModelsAsync<ComboModel>(innerEndpoint);
        
        ComboModelBase.SetApiComboItems(request.Data, index, Model as DtoBase);
    }

    protected new async Task RequestCombosUsingQueryRequestAsync(
        string innerEndpoint,
        int index = 0,
        QueryRequestBase? reqBase = null)
    {
        var request = await RequestCombosUsingQueryRequestAsync<ComboModel>(
            innerEndpoint,
            reqBase ?? new());
        
        ComboModelBase.SetApiComboItems(request.Data, index, Model as DtoBase);
    }
    
    protected async Task GetNextNumber()
    {
        var url = "";
    }
}