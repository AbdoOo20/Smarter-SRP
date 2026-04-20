using SmartiErpWin.Client.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.Views.Pages;

public partial class DashboardPage : INavigableView<DashboardPageViewModel>
{
    public DashboardPageViewModel ViewModel { get; }

    public DashboardPage(DashboardPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;

        InitializeComponent();
    }
}