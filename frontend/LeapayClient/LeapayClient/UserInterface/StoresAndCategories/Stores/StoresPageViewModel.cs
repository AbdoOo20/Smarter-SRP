using System.Collections.ObjectModel;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.StoresAndCategories.Stores.DamagedPartial;
using Leapay.Client.UserInterface.StoresAndCategories.Stores.Partials;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Stores;

public partial class StoresPageViewModel : BasePageViewModel<DtoStore>
{
    protected override string Endpoint => "Store";
    
    public StoresPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;

        ExtraButtonVisibility = true;
        PrintVisibility = true;

        PrintText = this["MoveItemsBetweenStorages"];
        ExtraText = this["SetItemsAsDamaged"];
        ExtraIcon = MaterialIconKind.PackageDown;
        
        RegisterForImport(GetImportModel);
    }

    private TableImportModel<DtoStore> GetImportModel()
    {
        return new TableImportModel<DtoStore>
        {
            SynchronisationInstances = [ this ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CommonPageEntryViewModel<DtoStore>(model, "Store", EntryType.Add, GetType());
                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new CommonPageEntryViewModel<DtoStore>(model, "Store", EntryType.Add,  GetType());
                var enforce = vm.PubEnforce();

                if (!enforce.IsSuccess()) return (enforce.GetDisplayMessage(), false);

                return (this["Success"], true);
            },
            Columns = new ObservableCollection<ColumnImportModel>(new List<ColumnImportModel>
            {
                new()
                {
                    DbName = "Name",
                    Name = this["Name"],
                    IsRequired = true
                },
                new()
                {
                    DbName = "Address",
                    Name = this["Address"],
                    IsRequired = false,
                    DefaultValue = ""
                },
            })
        };
    }
    
    protected override async void OnExtraButton()
    {        
        var entry = new DamageItemsEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry, options: new(this["SetItemsAsDamaged"], true, true, true, true));
    }

    protected override async Task PrintAsync()
    {
        
        var entry = new MoveItemsEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry, options: new(this["MoveItemsBetweenStorages"], true, true, true, true));
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoStore> args)
    {
        var entry = new StoreEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()    {
        var entry = new StoreEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}