using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Categories;

public class CategoryPageEntryViewModel : BaseEntryViewModel<DtoCategory>
{
    private readonly bool _isSilent;
    protected override string Endpoint => "Category";

    public SuperComboVm<DtoCategory> Categories { get; } = new();
    
    public CategoryPageEntryViewModel(
        DtoCategory? model,
        EntryType entryType,
        Type pageType,
        bool isSilent = false)
    {
        _isSilent = isSilent;
        SetDetails(model, entryType, pageType);

        //AddValidation(() => Model.CategoryName, optional: false);
        
        Refresh();
    }

    private async void Refresh()
    {
        if (_isSilent)
            return;
        
        await Categories.LoadAsync(this, "categuries", Model.CateguryId, s => Model.CateguryId = s.Id);
    }

    /*protected override bool PreSaveValidations()
    {
        if ((Categories.Items.Count > 1 && Model.CateguryId.IsNullOrEmpty()).Stop("ValidationFailed", "SelectACategoryAtLeastOneExistsMessage"))
        {
            return false;
        }

        return base.PreSaveValidations();
    }*/
}