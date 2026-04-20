using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Personnel.Employees.Windows;

public partial class SalaryCountingEntryWindow: UserControlBase
{
    public SalaryCountingEntryViewModel ViewModel { get; }
    
    public SalaryCountingEntryWindow(WindowEntryModel<DtoBase> window)
    {
        ViewModel = new SalaryCountingEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;
        
        InitializeComponent();
        DataContext = this;
    }
}