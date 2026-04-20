using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.Assignments;

public class AssignmentPageEntryViewModel : BaseEntryViewModel<DtoEmployeeAssignment>
{
    protected override string Endpoint => "EmployeeAssignment";
    
    public SuperComboVm<DtoEmployeeAssignment> Employees { get; } = new();
    
    public StaticComboVm<AssignmentsEnum> AssignmentType { get; } = new();

    public AssignmentPageEntryViewModel(
        DtoEmployeeAssignment? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        AddValidation(() => Model.EmployeeId, optional: false);
        
        Refresh();
    }

    private async void Refresh()
    {
        await Employees.LoadAsync(this, "employees", Model.EmployeeId, s => Model.EmployeeId = s.Id);
        AssignmentType.Set(Model.AssignmentsType, s => Model.AssignmentsType = s);
    }
}