using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Sales.Clients;

public partial class ClientEntryWindow: UserControlBase
{
    public CommonPageEntryViewModel<DtoClient> ViewModel { get; }
    
    public ClientEntryWindow(WindowEntryModel<DtoClient> window)
    {
        ViewModel = new CommonPageEntryViewModel<DtoClient>(window.Model,
            "Client",
            window.EntryType,
            window.PageType,
            App.CloseDialogWindowOfContentType<ClientEntryWindow>);
        
        ViewModel.AddValidation(() => ViewModel.Model.Name, optional: false);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}