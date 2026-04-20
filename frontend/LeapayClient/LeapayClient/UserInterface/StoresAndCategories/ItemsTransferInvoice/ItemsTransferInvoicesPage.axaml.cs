using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemsTransferInvoice;

public partial class ItemsTransferInvoicesPage : UserControlBase
{
    public ItemsTransferInvoicesPageViewModel ViewModel { get; set; }
    
    public ItemsTransferInvoicesPage(ItemsTransferInvoicesPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}