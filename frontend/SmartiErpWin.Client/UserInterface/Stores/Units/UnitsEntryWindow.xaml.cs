using Fly.Shared.Modules.Store.Dtos;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.UserInterface.Stores.Units;

public partial class UnitEntryWindow
{
    public CommonPageEntryViewModel<DtoUnit> ViewModel { get; }
    
    public UnitEntryWindow(LocalisationService localisationService,
        DtoUnit model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CommonPageEntryViewModel<DtoUnit>(this,
            localisationService, model, type, "Unit", client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}