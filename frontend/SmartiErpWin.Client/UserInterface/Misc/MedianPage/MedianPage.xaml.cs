using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Misc.MedianPage;

public partial class MedianPage : INavigableView<MedianPageViewModel>
{
    public MedianPageViewModel ViewModel { get; }

    public MedianPage(MedianPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
    }
}