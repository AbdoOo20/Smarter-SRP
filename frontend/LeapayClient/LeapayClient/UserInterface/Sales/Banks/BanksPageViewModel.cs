using System.Collections.ObjectModel;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.Banks;

public partial class BanksPageViewModel : BasePageViewModel<DtoBank>
{
    protected override string Endpoint => "Bank";
    
    public BanksPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        
        RegisterForImport(GetImportModel);
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoBank> args)
    {
        var entry = new BankEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new BankEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
    
    private TableImportModel<DtoBank> GetImportModel()
    {
        return new TableImportModel<DtoBank>
        {
            SynchronisationInstances = [ this ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CommonPageEntryViewModel<DtoBank>(model, "Bank", EntryType.Add, GetType());
                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
            },
            
            ItemValidationMechanism = model =>
            {
                var vm = new CommonPageEntryViewModel<DtoBank>(model, "Bank", EntryType.Add,  GetType());
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
                    DbName = "Note",
                    Name = this["Note"],
                    IsRequired = false,
                    DefaultValue = ""
                },
            })
        };
    }
}