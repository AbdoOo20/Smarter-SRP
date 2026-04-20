using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.BankBranches;

public class BankBranchEntryViewModel : BaseEntryViewModel<DtoBankBranch>
{
    protected override string Endpoint => "BankBranch";
    
    public SuperComboVm<DtoBankBranch> Banks { get; } = new();

    public BankBranchEntryViewModel(
        DtoBankBranch? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        AddValidation(() => Model.Name, optional: false);
        AddValidation(() => Model.BankId, optional: false);

        Refresh();
    }

    private async void Refresh()
    {
        await Banks.LoadAsync(this, "banks", Model.BankId, s => Model.BankId = s.Id);
    }
}