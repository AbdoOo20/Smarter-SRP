using Fly.Shared.Modules.Personnel.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.DiscountIncrease;

public partial class DiscountIncreasePageViewModel : BasePageViewModel<DtoDiscountIncrease>
{
    protected override string Endpoint => "DiscountIncrease";
    
    public DiscountIncreasePageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoDiscountIncrease> args)
    {
        var entry = new DiscountIncreaseEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new DiscountIncreaseEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}