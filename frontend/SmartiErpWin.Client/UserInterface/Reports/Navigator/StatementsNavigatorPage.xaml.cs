using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Reports.Navigator;

public partial class StatementsNavigatorPage : INavigableView<StatementsNavigatorPageViewModel>
{
    public StatementsNavigatorPageViewModel ViewModel { get; }

    public StatementsNavigatorPage(StatementsNavigatorPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
        
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {

    }
}