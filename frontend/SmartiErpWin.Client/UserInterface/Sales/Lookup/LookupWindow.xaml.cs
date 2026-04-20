namespace SmartiErpWin.Client.UserInterface.Sales.Lookup;

public partial class LookupWindow
{
    public LookupWindowViewModel ViewModel { get; }

    public LookupWindow(LookupWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        ViewModel.LoginWindow = this;
        
        DataContext = this;

        InitializeComponent();
    }
}