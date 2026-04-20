using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;

namespace SmartiErpWin.Client.UserInterface.Personnel.DiscountsAndIncreases;

public class DiscountIncreasePageEntryViewModel : BaseEntryViewModel<DtoDiscountIncrease>
{
    protected override string Endpoint => "DiscountIncrease";
    
    public DiscountIncreasePageEntryViewModel(Window window,
        LocalisationService locale,
        DtoDiscountIncrease? model,
        EntryType entryType,
        HttpClient client,
        Type pageType): base(client)
    {
        Model = model ?? new();
        EntryWindow = window;
        EntryType = entryType;
        PageType = pageType;
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        Refresh();
    }

    private async void Refresh()
    {
        ComboModelBase.ConversionKit = ConversionKit;
        await RequestApiComboItemsFromInnerEndpointAsync("employees");
        ComboModelBase.AddStaticComboItems(LocaleService["Discount"], LocaleService["Increase"]);
        
        ComboModelBase.SetStaticComboBoxItem(ComboModelBase.ConvertToDisplay((int)Model.Mode));
        ComboModelBase.SetComboModelOrDefault(Model.EmployeeId);
    }

    protected override void PreSaveActions()
    {
        Model!.Mode = ComboModelBase.ConvertToEnum<DiscountIncreaseMode>(
            ComboModelBase.SelectedStaticComboItem);
        Model.EmployeeId = ComboModelBase.GetSelectedItemId();
    }

    private Dictionary<int, string> ConversionKit => new()
    {
        { 0, LocaleService["Discount"] },
        { 1, LocaleService["Increase"] }
    };
}