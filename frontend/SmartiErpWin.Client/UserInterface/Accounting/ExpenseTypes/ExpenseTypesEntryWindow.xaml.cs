using Fly.Shared.Modules.Accounting.Dtos;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.UserInterface.Accounting.ExpenseTypes;

public partial class ExpenseTypesEntryWindow
{
    public CommonPageEntryViewModel<DtoExpenseTypes> ViewModel { get; }
    
    public ExpenseTypesEntryWindow(LocalisationService localisationService,
        DtoExpenseTypes model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new CommonPageEntryViewModel<DtoExpenseTypes>(this,
            localisationService, model, type, "ExpenseTypes", client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}