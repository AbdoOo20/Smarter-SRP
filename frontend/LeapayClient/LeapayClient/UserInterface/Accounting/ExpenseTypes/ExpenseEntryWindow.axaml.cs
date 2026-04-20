using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.ExpenseTypes;

public partial class ExpenseEntryWindow: UserControlBase
{
    public ExpenseEntryViewModel ViewModel { get; }
    
    public ExpenseEntryWindow(WindowEntryModel<DtoExpenseTypes> window)
    {
        ViewModel = new ExpenseEntryViewModel(window.Model, window.EntryType, window.PageType);
        
        InitializeComponent();
        DataContext = this;
    }
}