using System.Collections.ObjectModel;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.PurchaseAndSales.Dtos;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.StoresAndCategories.Items;
using Leapay.Client.UserInterface.StoresAndCategories.Stores;
using Leapay.Client.UserInterface.StoresAndCategories.Units;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.QuantityCrediting;

public partial class QuantityCreditingsPageViewModel : BasePageViewModel<DtoQuantityCreditingInvoice>
{
    protected override string Endpoint => "QuantityCrediting";
    
    public QuantityCreditingsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoQuantityCreditingInvoice> args)
    {
        var entry = new QuantityCreditingEntryWindow(new(args.Model,
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }
    
    protected override async Task OnAddAsync()    {
        var entry = new QuantityCreditingEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
    
    private TableImportModel<DtoQuantityCreditingInvoice> GetImportModel()
    {
        var stores = App.GetService<StoresPageViewModel>();
        var items = App.GetService<ItemsPageViewModel>();
        var units = App.GetService<ItemsPageViewModel>();
        
        return new TableImportModel<DtoQuantityCreditingInvoice>
        {
            SynchronisationInstances = [ this, stores, items, units ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (model.ImportUnitId.IsNullOrEmpty())
                {
                    return (this["SelectedUnitNotPartOfItem"], false);
                }
                
                model.Details = model.Details.Append(new()
                {
                    ItemId = model.ImportItemId.GetValueOrDefault(),
                    UnitId = model.ImportUnitId.GetValueOrDefault(),
                    Price = model.ImportPrice,
                    Quantity = model.ImportQuantity
                }).ToArray();
                
                if (!token.IsCancellationRequested)
                {
                    var vm = new QuantityCreditingEntryViewModel(model, EntryType.Add, GetType(), true);
                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new QuantityCreditingEntryViewModel(model, EntryType.Add, GetType(), true);
                var enforce = vm.PubEnforce();

                if (!enforce.IsSuccess()) return (enforce.GetDisplayMessage(), false);

                return (this["Success"], true);
            },
            Columns = new ObservableCollection<ColumnImportModel>(new List<ColumnImportModel>
            {
                new()
                {
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoQuantityCreditingInvoice)?.StoreId =
                            stores.Models.FirstOrDefault(m => m.Name == tuple.Item2)?.Id;
                    },
                    ForeignKeySatisfactionMechanism = tuple => { return stores.Models.Any(c => c.Name == tuple); },
                    DbName = "StoreId",
                    Name = this["Store"],
                    IsRequired = true,
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new StoreEntryWindow(new WindowEntryModel<DtoStore>(new DtoStore
                        {
                            Name = name,
                        }, EntryType.Add, typeof(StoresPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                },
                new()
                {
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoQuantityCreditingInvoice)?.ImportItemId =
                            items.Models.FirstOrDefault(m => m.Name == tuple.Item2)?.Id;
                    },
                    ForeignKeySatisfactionMechanism = tuple => { return items.Models.Any(c => c.Name == tuple); },
                    DbName = "ImportItemId",
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new ItemEntryWindow(new WindowEntryModel<CreateUpdateItemRequest>(new CreateUpdateItemRequest
                        {
                            Name = name,
                        }, EntryType.Add, typeof(ItemsPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    Name = this["Item"],
                    IsRequired = true
                },
                new()
                {
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoQuantityCreditingInvoice)?.ImportUnitId =
                            items.Models.FirstOrDefault(m => m.Name == tuple.Item2)?.ItemUnits.FirstOrDefault(i => i.ItemName == tuple.Item2)?.UnitId;
                    },
                    ForeignKeySatisfactionMechanism = tuple => { return units.Models.Any(c => c.Name == tuple); },
                    DbName = "ImportUnitId",
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new UnitEntryWindow(new WindowEntryModel<DtoUnit>(new DtoUnit
                        {
                            Name = name,
                        }, EntryType.Add, typeof(UnitsPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    Name = this["Unit"],
                    IsRequired = true
                },
                new()
                {
                    DbName = "ImportQuantity",
                    Name = this["Quantity"],
                    IsRequired = true
                },
            })
        };
    }
}