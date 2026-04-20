using SmartiErpWin.Client.UserInterface.Financial.StatementsLookup.OperationViewModels;

namespace SmartiErpWin.Client.UserInterface.Financial.StatementsLookup.OperationWindows;

public partial class ItemUnitStatementLookupWindow
{
    public ItemUnitStatementLookupViewModel ViewModel { get; }
    
    public ItemUnitStatementLookupWindow(LocalisationService localisationService,
        EntryType type,
        HttpClient client,
        Type pageType)
    {
        ViewModel = new ItemUnitStatementLookupViewModel(this,
            localisationService, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
    
    private void ComboPrefabOnOnSelectionChanged(object? sender, EventArgs e)
    {
        ViewModel.SetUnits();
    }
}