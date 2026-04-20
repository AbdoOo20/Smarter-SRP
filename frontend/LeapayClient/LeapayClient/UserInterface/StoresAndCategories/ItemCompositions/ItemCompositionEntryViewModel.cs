using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemCompositions;

public partial class ItemCompositionEntryViewModel : BaseItemPageEntryViewModel<DtoItemComposition, DtoItemCompositionDetail>
{
    protected override string Endpoint => "ItemComposition";

    [ObservableProperty]
    public partial SuperComboVm<DtoItemCompositionDetail> Categories { get; set; } = new();
    
    private IValidationEnforcer DetailsEnforcer { get; set; } = new ValidationEnforceImpl();

    [ObservableProperty]
    public partial decimal TotalPrice { get; set; }
    
    public ItemCompositionEntryViewModel(
        DtoItemComposition? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType, true);
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;

        AddValidation(() => Model.Name, optional: false);
        RefreshState(async () => await StartupAsync());
    }

    private async Task StartupAsync(bool refresh = true)
    {
        SetDetailsModel(null);
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemId, optional: false));
        DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.UnitId, optional: false));
        //DetailsEnforcer.AddValidation(CreateValidation(() => ModelDetails.Model.ItemVarianceId, optional: false));
        
        await OnAsync(() => refresh, async () => await RefreshAsync());
    }

    protected override async Task RefreshAsync(bool refreshCombos = true)
    {
        SetDataRespectively();
        
        if (refreshCombos)
        {
            await Categories.LoadAsync(this, "Categories", Model.CategoryId,
                s => Model.CategoryId = s.Id);
        }
        
        CalculatePrice();
    }
    
    protected override void WindowClosure()
    {
        App.CloseDialogWindowOfContentType<ItemCompositionEntryWindow>();
    }
    
    private void CalculatePrice()
    {
        TotalPrice = Model.Details
            .Sum(d => d.Price * d.Quantity);
    }

    protected override bool PreSaveChecks()
    {
        if (string.IsNullOrEmpty(Model.Name).Stop("ValidationFailed", "NameCannotBeEmpty"))
            return false;
        
        if (Model.CategoryId.IsNullOrEmpty().Stop("ValidationFailed", "CategoryIdSelectOption"))
            return false;
        
        if ((!Model.Details.Any()).Stop("ValidationFailed", "SelectItemDetailsBeforeProceeding"))
            return false;

        return base.PreSaveChecks();
    }
}