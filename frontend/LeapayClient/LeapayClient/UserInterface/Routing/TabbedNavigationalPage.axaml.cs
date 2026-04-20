using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Routing;

public partial class TabbedNavigationalPage : UserControlBase
{
    public TabbedNavigationalPageViewModel ViewModel { get; }

    public TabbedNavigationalPage(TabbedNavigationalPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
    }
}