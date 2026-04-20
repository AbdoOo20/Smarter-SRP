using Avalonia.Interactivity;
using Fly.Shared.Modules.Factoring.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Factoring.FactoringInvoice;

public partial class FactoringInvoiceEntryWindow: UserControlBase
{
    public FactoringInvoiceEntryViewModel ViewModel { get; }
    
    public FactoringInvoiceEntryWindow(WindowEntryModel<DtoFactoringInvoice> window)
    {
        ViewModel = new FactoringInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        Init();
    }

    public FactoringInvoiceEntryWindow(FactoringInvoiceEntryViewModel vm)
    {
        ViewModel = vm;
        Init();
    }
    
    private void Init()
    {
        InitializeComponent();
        DataContext = this;
        SynchronisationModel = ViewModel;

        ViewModel.ShowData(DataGrid);
        ViewModel.Costs.ShowData(SecondaryDataGrid);
        ViewModel.Compositions.ShowData(CompositionsDataGrid);
        
        ViewModel.ItemInfoBar = ItemInfoBar;
        ViewModel.ItemCompositionInfoBar = ItemCompositionInfoBar;
    }

    private void InternalFactoringCheckedChanged(object? sender, RoutedEventArgs e)
    {
        ViewModel.OnInternalFactoringCheckChanged((sender as CheckBox)?.IsChecked ?? false);
    }
}