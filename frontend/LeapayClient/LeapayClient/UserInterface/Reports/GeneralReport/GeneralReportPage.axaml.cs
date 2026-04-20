using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Reports.GeneralReport;

public partial class GeneralReportPage : UserControlBase
{
    public GeneralReportPageViewModel ViewModel { get; set; }
    
    public GeneralReportPage(GeneralReportPageViewModel pageViewModel)
    {
        ViewModel = pageViewModel;
        SynchronisationModel = pageViewModel;
        
        DataContext = this;

        InitializeComponent();
        
        ViewModel.SetXamlObjects(DataGrid, UpperPanel, LowerPanel, CombosWrap, MainComboPanel);
    }

    private void OnDataSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DataScroller.MaxHeight = e.NewSize.Height * DataScrollerHeightMultiplier;
        DataScroller.MaxWidth = e.NewSize.Width * DataScrollerWidthMultiplier;
        //ViewModel.DataGridHeight = e.NewSize.Height * DataGridHeightMultiplier;
    }
}