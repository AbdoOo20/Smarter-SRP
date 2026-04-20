using Fly.Shared.Modules.Personnel.Dtos;

namespace SmartiErpWin.Client.UserInterface.Personnel.Employees;

public class EmployeesPageEntryViewModel : BaseEntryViewModel<DtoEmployee>
{
    protected override string Endpoint => "Employee";
    
    public EmployeesPageEntryViewModel(Window window,
        LocalisationService locale,
        DtoEmployee? model,
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
        await RequestApiComboItemsFromInnerEndpointAsync("departments");

        ComboModelBase.SetComboModelOrDefault(Model.DepartmentId);
    }

    protected override void PreSaveActions()
    {
        Model.DepartmentId = ComboModelBase.GetSelectedItemId();
    }
}