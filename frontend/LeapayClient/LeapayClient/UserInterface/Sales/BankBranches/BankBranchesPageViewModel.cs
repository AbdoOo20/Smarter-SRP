using System.Collections.ObjectModel;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Sales.Banks;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.BankBranches;

public partial class BankBranchesPageViewModel : BasePageViewModel<DtoBankBranch>
{
    protected override string Endpoint => "BankBranch";
    
    public BankBranchesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        
        RegisterForImport(GetImportModel);
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoBankBranch> args)
    {
        var entry = new BankBranchEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new BankBranchEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
    
    private TableImportModel<DtoBankBranch> GetImportModel()
    {
        var banks = App.GetService<BanksPageViewModel>();

        return new TableImportModel<DtoBankBranch>
        {
            SynchronisationInstances = [ this, banks ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CommonPageEntryViewModel<DtoBankBranch>(model, "BankBranch", EntryType.Add, GetType());
                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new CommonPageEntryViewModel<DtoBankBranch>(model, "BankBranch", EntryType.Add,  GetType());
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
                    str += this["DuplicateName"];
                }
                
                return (str, result);
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
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoBankBranch)?.BankId =
                            (banks.Models.FirstOrDefault(m => m.Name == tuple.Item2)?.Id).GetValueOrDefault();
                    },
                    ForeignKeySatisfactionMechanism = tuple => { return Models.Any(c => c.Name == tuple); },
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new BankEntryWindow(new WindowEntryModel<DtoBank>(new DtoBank
                        {
                            Name = name,
                        }, EntryType.Add, typeof(BanksPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    DbName = "BankId",
                    Name = this["ParentBank"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "Note",
                    Name = this["Note"],
                    IsRequired = false,
                    DefaultValue = ""
                },
                new()
                {
                    DbName = "AccountOpeningDebtor",
                    Name = this["AccountOpeningDebtor"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "AccountOpeningCreditor",
                    Name = this["AccountOpeningCreditor"],
                    IsRequired = false
                },
            })
        };
    }
}