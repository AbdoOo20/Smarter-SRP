using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.UserInterface.Accounting.ExpenseTypes;

public class ExpenseTypesPageEntryViewModel : BaseEntryViewModel<DtoExpenseTypes>
{
    public ExpenseTypesPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoExpenseTypes? model,
        EntryType entryType, HttpClient client): base(client)
    {        
        EntryType = entryType;
        EntryWindow = window;
        Model = model ?? new();
        
        InitBaseModel(locale, null, null);
    }
}