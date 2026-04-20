using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Sales.SalesCheckout;

public partial class SalesCheckoutEntryWindow: INavigableView<SalesCheckoutEntryViewModel>
{
    public SalesCheckoutEntryViewModel ViewModel { get; }
    
    public SalesCheckoutEntryWindow(LocalisationService localisationService, HttpClient client)
    {
        ViewModel = new SalesCheckoutEntryViewModel(localisationService, client, GetType());
        
        InitializeComponent();
        
        DataContext = this;
        
        OnLoaded(this, null);
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.InitAsync(CategoriesTabControl, ActiveDataStackPanel, BarcodeBox);
        OnSizeChanged(sender, null);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs? e)
    {
        var width = e is null ? ActualWidth : e.NewSize.Width;
        var height = e is null ? ActualHeight : e.NewSize.Height;
        
        ParentActiveDataStackPanel.MinWidth = width * 0.35;
        ParentActiveDataStackPanel.MaxWidth = width * 0.35;
        ActiveDataStackPanel.MinWidth = width * 0.35;
        CategoriesTabControl.MinWidth = width * 0.74;
        CategoriesTabControl.MinHeight = height * 0.54;
        //CategoriesScroller.MaxWidth = width * 0.74;
        //CentrePanel.MinHeight = height * 0.9;

        _ = ViewModel.SoftRefreshAsync();
    }
}