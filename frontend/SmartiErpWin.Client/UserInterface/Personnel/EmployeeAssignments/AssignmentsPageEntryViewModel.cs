using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;

namespace SmartiErpWin.Client.UserInterface.Personnel.EmployeeAssignments;

public class AssignmentsPageEntryViewModel : BaseEntryViewModel<DtoEmployeeAssignment>
{
    protected override string Endpoint => "EmployeeAssignment";
    
    public AssignmentsPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoEmployeeAssignment? model,
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
        ComboModelBase.AddStaticComboItems(LocaleService["FullTime"], LocaleService["SpecificWork"]);
        
        ComboModelBase.SetStaticComboBoxItem(ComboModelBase.ConvertToDisplay((int)Model.AssignmentsType));
        ComboModelBase.SetComboModelOrDefault(Model.EmployeeId);
    }

    protected override void PreSaveActions()
    {
        Model!.AssignmentsType = ComboModelBase.ConvertToEnum<AssignmentsEnum>(
            ComboModelBase.SelectedStaticComboItem);
        Model.EmployeeId = ComboModelBase.GetSelectedItemId();
    }

    private Dictionary<int, string> ConversionKit => new()
    {
        { 0, LocaleService["FullTime"] },
        { 1, LocaleService["SpecificWork"] }
    };
}