using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.UserInterface.Accounting.Assets;

public class AssetPageEntryViewModel : BaseEntryViewModel<DtoAssets>
{
    protected override string Endpoint => "Assets";
    
    public AssetPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoAssets? model,
        EntryType entryType,
        HttpClient client,
        Type pageType): base(client)
    {
        Model = model ?? new();
        EntryWindow = window;
        EntryType = entryType;
        PageType = pageType;
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        Refresh();
    }

    private async void Refresh()
    {
        await PrepareComboBoxAsync("accounts", Model.AccountId);
    }

    protected override void PreSaveActions()
    {
        Model.AccountId = ComboModelBase.GetSelectedItemId();
    }
}