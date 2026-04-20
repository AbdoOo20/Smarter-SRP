using Fly.Shared.Modules.Marketers.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Marketer.MarketerDisbursementPermits;

public partial class MarketerDisbursementPermitEntryWindow: UserControlBase
{
    public MarketerDisbursementPermitEntryViewModel ViewModel { get; }
    
    public MarketerDisbursementPermitEntryWindow(WindowEntryModel<DtoMarketerDisbursementPermit> window)
    {
        ViewModel = new MarketerDisbursementPermitEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        
        ViewModel.ItemInfoBar = ItemInfoBar;
    }
}