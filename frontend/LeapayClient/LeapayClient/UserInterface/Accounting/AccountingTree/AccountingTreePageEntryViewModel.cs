using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.AccountingTree;

public sealed class AccountingTreePageEntryViewModel : BaseEntryViewModel<DtoAccount>
{
    protected override string Endpoint => "Account";

    public SearchableComboVm<DtoAccount> Accounts { get; set; } = new("ParentAccount");
    
    public StaticComboVm<AccountType> AccountTypes { get; } = new();

    public AccountingTreePageEntryViewModel(DtoAccount? model, EntryType entryType, Type pageType)
    {
        SetDetails(model, entryType, pageType);
        Refresh();
    }

    private async void Refresh()
    {
        await Accounts.LoadAsync(this, "accounts", Model.ParentId, s => Model.ParentId = s.Id);
        AccountTypes.Set(Model.Natural, s => Model.Natural = s);
    }

    protected override void PreSaveActions()
    {
        Model.Natural = ComboModelBase.ConvertToEnum<AccountType>(ComboModelBase.SelectedStaticComboItem);
        Model.ParentId = ComboModelBase.GetSelectedItemId();
    }
}