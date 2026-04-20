using Avalonia.Interactivity;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.SalesPoint;

public partial class SalesPointPage : UserControlBase
{
    public SalesPointPageViewModel ViewModel { get; set; }
    
    public SalesPointPage(SalesPointPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        Loaded += OnLoaded;
    }
    
    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        await ViewModel.InitAsync(ItemsSearchPanel);
    }
}