using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Common.SysConfigPage.Internals;

public partial class SysConfigInitialEntryWindow: UserControlBase
{
    public SysConfigInitialEntryViewModel ViewModel { get; }
    
    public SysConfigInitialEntryWindow(WindowEntryModel<DtoBase> window)
    {
        ViewModel = new SysConfigInitialEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}