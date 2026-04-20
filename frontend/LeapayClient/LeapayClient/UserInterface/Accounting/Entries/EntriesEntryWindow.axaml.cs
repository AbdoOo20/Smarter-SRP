using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.Entries;

public partial class EntriesEntryWindow: UserControlBase
{
    public EntriesEntryViewModel ViewModel { get; }
    
    public EntriesEntryWindow(WindowEntryModel<DtoEntry> window)
    {
        ViewModel = new EntriesEntryViewModel(window.Model, window.EntryType, window.PageType);
        
        InitializeComponent();
        DataContext = this;
        
        ViewModel.ShowData(DataGrid);
        ViewModel.SetDate(CalenderBox);
    }
}