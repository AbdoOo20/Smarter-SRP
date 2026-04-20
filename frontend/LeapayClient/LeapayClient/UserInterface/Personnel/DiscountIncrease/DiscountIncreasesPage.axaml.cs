using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.DiscountIncrease;

public partial class DiscountIncreasePage : UserControlBase
{
    public DiscountIncreasePageViewModel ViewModel { get; set; }
    
    public DiscountIncreasePage(DiscountIncreasePageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.ShowData(DataGrid);
    }
}