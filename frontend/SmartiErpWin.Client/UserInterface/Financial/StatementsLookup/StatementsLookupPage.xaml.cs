using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Financial.StatementsLookup;

public partial class StatementsLookupPage : INavigableView<StatementsLookupPageViewModel>
{
    public StatementsLookupPageViewModel ViewModel { get; }

    public StatementsLookupPage(StatementsLookupPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
        
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {

    }
}