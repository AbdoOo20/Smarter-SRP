using Fly.Shared.Modules.Personnel.Dtos;

namespace SmartiErpWin.Client.UserInterface.Personnel.DiscountsAndIncreases;

public partial class DiscountIncreaseEntryWindow
{
    public DiscountIncreasePageEntryViewModel ViewModel { get; }
    
    public DiscountIncreaseEntryWindow(LocalisationService localisationService,
        DtoDiscountIncrease model, EntryType type, HttpClient client, Type pageType)
    {
        ViewModel = new DiscountIncreasePageEntryViewModel(this,
            localisationService, model, type, client, pageType);
        
        InitializeComponent();
        DataContext = this;
    }
}