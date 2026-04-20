using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.AccountingTree;

public partial class AccountingTreeEntryWindow: UserControlBase
{
    public AccountingTreePageEntryViewModel ViewModel { get; }
    
    public AccountingTreeEntryWindow(DtoAccount? model, EntryType type, Type pageType)
    {
        ViewModel = new AccountingTreePageEntryViewModel(model, type,  pageType);
        
        DataContext = this;
        InitializeComponent();
    }
}