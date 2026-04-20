using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.DiscountIncrease;

public partial class DiscountIncreaseEntryWindow: UserControlBase
{
    public DiscountIncreaseEntryViewModel ViewModel { get; }
    
    public DiscountIncreaseEntryWindow(WindowEntryModel<DtoDiscountIncrease> window)
    {
        ViewModel = new DiscountIncreaseEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}