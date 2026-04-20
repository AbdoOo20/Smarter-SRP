using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Routing;

public partial class MedianPage : UserControlBase
{
    public MedianPageViewModel ViewModel { get; }

    public MedianPage(MedianPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
    }
}