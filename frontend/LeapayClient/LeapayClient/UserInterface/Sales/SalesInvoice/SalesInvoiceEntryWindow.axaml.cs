using Avalonia.Input;
using Avalonia.Threading;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;

namespace Leapay.Client.UserInterface.Sales.SalesInvoice;

public partial class SalesInvoiceEntryWindow: UserControlBase
{
    public SalesInvoiceEntryViewModel ViewModel { get; }
    
    public SalesInvoiceEntryWindow(WindowEntryModel<DtoSalesInvoice> window)
    {
        ViewModel = new SalesInvoiceEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
        Loaded += (_, _) =>
        {
            ViewModel.ItemQuantityInfoBar = ItemQuantityInfoBar;
            ViewModel.ItemInfoBar = ItemInfoBar;
        };
        
        ViewModel.ShowData(DataGrid);
    }

    public SalesInvoiceEntryWindow(SalesInvoiceEntryViewModel vm)
    {
        ViewModel = vm;
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
    }
    
    private void OnDiscountTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!decimal.TryParse((sender as TextBox)?.Text, out var val))
            return;
        
        ViewModel.CalculatePrice(val);
    }
    
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (ViewModel.IsSelectionWindowOpen)
            return;

        if (e.Source is not null && e.Source == LastFocus && e.Key is Key.Enter)
            ViewModel.SaveCommand.Execute(null);
        
        HelperServices.HandlePreviewKey<SalesInvoiceEntryWindow>(e);
    }
    
    public void FocusFirst()
    {
        ContentHelper.ExecuteGlobal<SalesInvoiceEntryWindow>(x => x.Activate());

        Dispatcher.UIThread.Post(() =>
        {
            Focus();
            FocusTarget.Focus();
        }, DispatcherPriority.Background);
    }
}