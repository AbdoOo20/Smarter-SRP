using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.Assets;

public partial class AssetEntryWindow: UserControlBase
{
    public AssetPageEntryViewModel ViewModel { get; }
    
    public AssetEntryWindow(WindowEntryModel<DtoAssets> window)
    {
        ViewModel = new AssetPageEntryViewModel(window.Model, window.EntryType, window.PageType);
        
        InitializeComponent();
        DataContext = this;
    }
}