using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.ExpenseTypes;

public partial class ExpensesPageViewModel : BasePageViewModel<DtoExpenseTypes>
{
    protected override string Endpoint => "ExpenseTypes";
    
    public ExpensesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoExpenseTypes> args)
    {
        var entry = new ExpenseEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()    {
        var entry = new ExpenseEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}