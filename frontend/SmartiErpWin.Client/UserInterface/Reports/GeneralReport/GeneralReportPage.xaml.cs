using SmartiErpWin.Client.Prefabs;
using System.Windows.Controls;
using Wpf.Ui.Abstractions.Controls;

namespace SmartiErpWin.Client.UserInterface.Reports.GeneralReport;

public partial class GeneralReportPage : INavigableView<GeneralReportPageViewModel>
{
    public GeneralReportPageViewModel ViewModel { get; }

    public GeneralReportPage(GeneralReportPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        DataContext = this;
        InitializeComponent();
        
        ViewModel.SetXamlObjects(DataGrid, UpperPanel, LowerPanel, CombosWrap, MainComboPanel);
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DataScroller.MaxHeight = e.NewSize.Height * DataScrollerHeightMultiplier;
        DataScroller.MaxWidth = e.NewSize.Width * DataScrollerWidthMultiplier;
        ViewModel.MinDataGridHeight = e.NewSize.Height * DataGridHeightMultiplier;
    }

    private void OnAccountTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SetAccounts();
    }

    private void DynamicAutoSuggestPrefab_GotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is DynamicAutoSuggestPrefab prefab)
        {
            prefab.Focus();
            Console.WriteLine($"Selection changed for {prefab.TitleText}: {prefab.SelectedItem}");
        }
    }
}