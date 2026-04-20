using Fly.Shared.Modules.Store.Dtos;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.UserInterface.Stores.CostCentres;

public partial class CostCentreEntryWindow
{
    public CommonPageEntryViewModel<DtoCostCenter> ViewModel { get; }
    
    public CostCentreEntryWindow(LocalisationService localisationService,
        DtoCostCenter model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CommonPageEntryViewModel<DtoCostCenter>(this,
            localisationService, model, type, "CostCenter", client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}