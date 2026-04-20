using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Misc.Configuration;

public partial class CompanyInfoPage : UserControlBase
{
    public CompanyInfoPageViewModel ViewModel { get; set; }
    
    public CompanyInfoPage(CompanyInfoPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
    }
}