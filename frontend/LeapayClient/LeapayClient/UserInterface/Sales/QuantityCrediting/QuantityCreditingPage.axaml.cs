using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingsPage : UserControlBase
{
    public QuantityCreditingsPageViewModel ViewModel { get; set; }
    
    public QuantityCreditingsPage(QuantityCreditingsPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}