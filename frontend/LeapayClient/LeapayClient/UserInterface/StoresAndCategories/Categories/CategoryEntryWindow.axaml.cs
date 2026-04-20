using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.StoresAndCategories.Categories;

public partial class CategoryEntryWindow: UserControlBase
{
    public CategoryPageEntryViewModel ViewModel { get; }
    
    public CategoryEntryWindow(WindowEntryModel<DtoCategory> window)
    {
        ViewModel = new CategoryPageEntryViewModel(window.Model, window.EntryType, window.PageType);
        SynchronisationModel = ViewModel;

        InitializeComponent();
        DataContext = this;
    }
}