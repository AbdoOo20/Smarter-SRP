using Fly.Shared.Modules.Marketers.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Marketer.MarketerEntry;

public partial class MarketerEntryWindow: UserControlBase
{
    public MarketerEntryViewModel ViewModel { get; }
    
    public MarketerEntryWindow(WindowEntryModel<DtoMarketer> window)
    {
        ViewModel = new MarketerEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}