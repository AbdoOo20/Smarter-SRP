using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.ExpenseTypes;

public class ExpenseEntryViewModel : BaseEntryViewModel<DtoExpenseTypes>
{
    protected override string Endpoint => "ExpenseTypes";
    
    public ExpenseEntryViewModel(
        DtoExpenseTypes? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
    }
}