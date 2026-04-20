using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.Employees;

public class EmployeePageEntryViewModel : BaseEntryViewModel<DtoEmployee>
{
    private readonly bool _isSilent;
    protected override string Endpoint => "Employee";
    
    public SuperComboVm<DtoEmployee> Departments { get; } = new();

    public EmployeePageEntryViewModel(
        DtoEmployee? model,
        EntryType entryType,
        Type pageType, 
        bool isSilent = false)
    {
        _isSilent = isSilent;
        SetDetails(model, entryType, pageType);

        Refresh();
    }

    private async void Refresh()
    {
        if (_isSilent)
            return;
        
        await Departments.LoadAsync(this, "departments", Model.DepartmentId, s => Model.DepartmentId = s.Id);
    }
}