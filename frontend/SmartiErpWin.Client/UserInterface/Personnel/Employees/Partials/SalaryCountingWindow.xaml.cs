namespace SmartiErpWin.Client.UserInterface.Personnel.Employees.Partials;

/// <summary>
/// Interaction logic for SalaryCountingWindow.xaml
/// </summary>
public partial class SalaryCountingWindow 
{
    public SalaryCountingWindowViewModel ViewModel { get; }

    public SalaryCountingWindow(SalaryCountingWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        ViewModel.Window = this;

        DataContext = this;

        InitializeComponent();
    }
}