using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Requests;
using SmartErpWin.Core.Helpers;

namespace SmartiErpWin.Client.UserInterface.Financial.StatementsLookup.OperationViewModels;

public partial class ItemUnitStatementLookupViewModel : BaseEntryViewModel<ItemQuntityQueryRequest>
{
    protected override string Endpoint => "ItemQuantity";

    [ObservableProperty]
    private string? _resultText;
    
    public ItemUnitStatementLookupViewModel(Window window,
        LocalisationService locale,
        EntryType entryType,
        HttpClient client,
        Type pageType): base(client)
    {
        EntryWindow = window;
        PageType = pageType;
        EntryType = entryType;

        TimeHelper.SetForStatement(EntryType, out var start, out var end);
        
        Model = new()
        {
            FromDate = start,
            ToDate = end
        };
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(GetType(), EntryType);
        
        Refresh();
    }
    
    private async void Refresh()
    {
        await PrepareComboBoxAsync("Items", Model.ItemId);
        await PrepareComboBoxAsync("Stores", Model.StoreId, 2);
    }

    public async void SetUnits()
    {
        var selectedUnit = ComboModelBase.GetSelectedItemId();

        if (selectedUnit == Identifier.Empty)
            return;
        
        await RequestApiComboItemsFromInnerEndpointAsync("Units", 1, selectedUnit.ToString());
        ComboModelBase.SetComboModelOrDefault(Model.UnitVemf, 1);
    }

    protected override void PreSaveActions()
    {
        Model.ItemId = ComboModelBase.GetSelectedItemId();
        Model.UnitVemf = ComboModelBase.GetSelectedItemId(1);
        Model.StoreId = ComboModelBase.GetSelectedItemId(2);
    }

    protected override async Task SaveAsync()
    {
        PreSaveActions();
        
        ResultText = (await PostSaveStatementLookupAsync(Model)).Data;
    }
}