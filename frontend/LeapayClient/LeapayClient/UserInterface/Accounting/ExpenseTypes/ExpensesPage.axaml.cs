using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.ExpenseTypes;

public partial class ExpensesPage : UserControlBase
{
    public ExpensesPageViewModel ViewModel { get; set; }
    
    public ExpensesPage(ExpensesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}