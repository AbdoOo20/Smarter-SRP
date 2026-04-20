using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations;

public partial class FinancingOperationsPage : INavigableView<FinancingOperationsPageViewModel>
{
    public FinancingOperationsPageViewModel ViewModel { get; }

    public FinancingOperationsPage(FinancingOperationsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
        
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {

    }
}