using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.ItemVariance;

public partial class ItemVariancesPageViewModel : BasePageViewModel<DtoItemVariance>
{
    protected override string Endpoint => "ItemVariance";
    
    public ItemVariancesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoItemVariance> args)
    {
        var entry = new ItemVarianceEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()    {
        var entry = new ItemVarianceEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}