using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.Assignments;

public partial class AssignmentsPageViewModel : BasePageViewModel<DtoEmployeeAssignment>
{
    protected override string Endpoint => "EmployeeAssignment";
    
    public AssignmentsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoEmployeeAssignment> args)
    {
        var entry = new AssignmentEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new AssignmentEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}