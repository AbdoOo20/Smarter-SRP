using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.Assets;

public class AssetPageEntryViewModel : BaseEntryViewModel<DtoAssets>
{
    protected override string Endpoint => "Assets";

    public SuperComboVm<DtoAssets> Accounts { get; } = new();
    
    public AssetPageEntryViewModel(
        DtoAssets? model,
        EntryType entryType,
        Type pageType)
    {
        Model = model ?? new();
        EntryType = entryType;
        PageType = pageType;

        AddValidation(() => Model.AccountId, optional: false);
        
        Refresh();
    }

    private async void Refresh()
    {
        await Accounts.LoadAsync(this, "accounts", Model.AccountId, s => Model.AccountId = s.Id);
    }
}