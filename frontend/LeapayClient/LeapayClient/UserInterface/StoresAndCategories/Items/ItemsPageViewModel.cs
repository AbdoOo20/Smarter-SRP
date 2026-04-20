using System.Collections.ObjectModel;
using Avalonia.Threading;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using Fly.Shared.Modules.Store.Requests;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.BarcodePrintDialogue;
using Leapay.Client.UserInterface.StoresAndCategories.Categories;
using Leapay.Client.UserInterface.StoresAndCategories.Units;
using Mapster;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Items;

public partial class ItemsPageViewModel : BasePageViewModel<DtoItem>
{
    protected override string Endpoint => "Item";
    
    protected override List<Tuple<object, EventHandler<DataGridButtonEventArgs<DtoItem>>>> ExtraButtons =>
    [
        new (MaterialIconKind.BarcodeScan, OnPrintBarcode),
    ];
    
    public ItemsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);

        ExtraButtonVisibility = true;
        ExtraIcon = MaterialIconKind.BarcodeScanner;
        ExtraText = this["PrintBarcode"];
    }

    private TableImportModel<DtoItem> GetImportModel()
    {
        var categories = App.GetService<CategoriesPageViewModel>();
        var units = App.GetService<UnitsPageViewModel>();
        
        return new TableImportModel<DtoItem>
        {
            SynchronisationInstances = [ this, categories, units ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var local = model.Adapt<CreateUpdateItemRequest>();
                    var temp = local.ItemUnits.ToList();
                    local.ItemUnits = [];
                    local.ItemUnits = temp;

                    var vm = new ItemEntryViewModel(local, EntryType.Add, GetType(), true);
                    vm.SetData(local.ItemUnits);

                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new ItemEntryViewModel(model.Adapt<CreateUpdateItemRequest>(), EntryType.Add, GetType(), true);
                var enforce = vm.PubEnforce();

                if (!enforce.IsSuccess()) return (enforce.GetDisplayMessage(), false);

                return (this["Success"], true);
            },
            ItemDuplicateValidationMechanism = model =>
            {
                var str = "";
                var result = true;
                
                var duplicateName = Models.FirstOrDefault(c => c.Name == model?.Name);

                if (duplicateName is not null)
                {
                    result = false;
                    str += this["DuplicateName"] + "\n";
                }
                
                var duplicateBarcode = Models.FirstOrDefault(c => c.Barcode == model?.Barcode);

                if (duplicateBarcode is not null)
                {
                    result = false;
                    str += "\n" + this["BarcodeDuplicatedWith"] + $": {duplicateBarcode.Name}";
                }
                
                return (str, result);
            },
            Columns = new ObservableCollection<ColumnImportModel>(new List<ColumnImportModel>
            {
                new()
                {
                    HasForeignKey = false,
                    DbName = "Name",
                    Name = this["Name"],
                    IsRequired = true
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "Barcode",
                    Name = this["Barcode"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoItem)?.CategoryId =
                            categories.Models.FirstOrDefault(m => m.Name == tuple.Item2)?.Id ?? new Identifier();
                    },
                    ForeignKeySatisfactionMechanism = tuple =>
                    {
                        return categories.Models.Any(c => c.Name == tuple);
                    },
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new CategoryEntryWindow(new WindowEntryModel<DtoCategory>(new DtoCategory
                        {
                            Name = name,
                        }, EntryType.Add, typeof(CategoriesPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    DbName = "CategoryId",
                    Name = this["CategoryName"],
                    IsRequired = true
                },
                new()
                {
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            var item = tuple.Item1 as DtoItem;
                            item?.ItemUnits =
                            [
                                new DtoItemUnit
                                {
                                    IsPrimary = true,
                                    UnitId = units.Models.FirstOrDefault(u => u.Name == tuple.Item2)?.Id ?? new Identifier(),
                                    UnitName = units.Models.FirstOrDefault(u => u.Name == tuple.Item2)?.Name
                                }
                            ];
                        });
                    },
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new UnitEntryWindow(new WindowEntryModel<DtoUnit>(new DtoUnit
                        {
                            Name = name,
                        }, EntryType.Add, typeof(UnitsPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    ForeignKeySatisfactionMechanism = tuple => { return units.Models.Any(c => c.Name == tuple); },
                    DbName = "ItemUnits",
                    Name = this["Unit"],
                    IsRequired = true
                },
                new()
                {
                    HasForeignKey = true,
                    ProcessOrder = 1,
                    ShowInUi = false,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            var item = tuple.Item1 as DtoItem;

                            if (item.ItemUnits.Count() == 0)
                                return;

                            var primary = item.ItemUnits.FirstOrDefault(u => u.IsPrimary);

                            if (primary is null)
                                return;

                            if (!decimal.TryParse(tuple.Item2, out var transfer) && !string.IsNullOrEmpty(tuple.Item2))
                            {
                                Warning("User entered a transfer rate that is incorrect while importing items.");
                                return;
                            }

                            primary.TransferRate = transfer;
                        });
                    },
                    DbName = "TransferRate",
                    Name = this["TransferRate"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "Price",
                    Name = this["Price"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "MinPrice",
                    Name = this["MinPrice"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "RegionalPrice",
                    Name = this["RegionalPrice"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "BulkPrice",
                    Name = this["BulkPrice"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "Expirable",
                    Name = this["IsItemExpirable"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "IsActive",
                    Name = this["IsItemActive"],
                    IsRequired = false
                },
            })
        };
    }
    
    private async void OnPrintBarcode(object? sender, DataGridButtonEventArgs<DtoItem> args)
    {
        if (string.IsNullOrEmpty(args.Model.Barcode))
        {
            _ = DialogueServices.ShowErrorAsync(this["ItemHasNoBarcode"]);
            return;
        }
        
        var vm = new BarcodePrintDialogueViewModel(null, EntryType.Add)
        {
            ItemName = args.Model.Name,
            ItemPrice = args.Model.Price.ToString("N"),
            ItemBarcode = args.Model.Barcode,
            ItemCount = 10
        };
        
        var entry = new BarcodePrintDialogueWindow(vm);

        await ShowContentAsync(entry, options: new(this["PrintBarcode"], false, false, false, false));
    }
    
    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoItem> args)
    {
        var entry = new ItemEntryWindow(new(args.Model.Adapt<CreateUpdateItemRequest>(),
            EntryType.Edit, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Edit), true, true, true, true));
    }

    protected override async void OnExtraButton()
    {
        var vm = new BarcodePrintDialogueViewModel(null, EntryType.Add)
        {
            ItemCount = 10
        };
        
        var entry = new BarcodePrintDialogueWindow(vm);

        await ShowContentAsync(entry, options: new(this["PrintBarcode"], false, false, false, false));
    }

    protected override async Task OnAddAsync()
    {
        var entry = new ItemEntryWindow(new(new(),
            EntryType.Add, GetType()));

        await ShowContentAsync(entry, options: new(GetPageName(GetType(), EntryType.Add), true, true, true, true));
    }
}