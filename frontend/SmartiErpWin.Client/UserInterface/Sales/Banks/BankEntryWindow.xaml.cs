using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.UserInterface.Sales.Banks;

public partial class BankEntryWindow
{
    public CommonPageEntryViewModel<DtoBank> ViewModel { get; }
    
    public BankEntryWindow(LocalisationService localisationService,
        DtoBank model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CommonPageEntryViewModel<DtoBank>(this,
            localisationService, model, type, "Bank", client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}