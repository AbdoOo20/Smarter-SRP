using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Common.SysConfigPage;

public partial class SysConfigPage: UserControlBase
{
    public SysConfigPageViewModel ViewModel { get; }
    
    public SysConfigPage(SysConfigPageViewModel viewModel)
    {
        ViewModel = viewModel;
        SynchronisationModel = viewModel;

        InitializeComponent();
        DataContext = this;
    }
}