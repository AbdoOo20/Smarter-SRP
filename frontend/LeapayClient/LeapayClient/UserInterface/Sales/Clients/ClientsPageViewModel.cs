using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.StoresAndCategories.Suppliers;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.Clients;

public partial class ClientsPageViewModel : BasePageViewModel<DtoClient>
{
    protected override string Endpoint => "Client";
    
    public ClientsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    private TableImportModel<DtoClient> GetImportModel()
    {
        return new TableImportModel<DtoClient>
        {
            SynchronisationInstances = [ this ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CommonPageEntryViewModel<DtoClient>(model, "Client", EntryType.Add, GetType());
                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
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
                
                var duplicatePhone = Models.FirstOrDefault(c => c.Phone == model?.Phone);

                if (duplicatePhone is not null)
                {
                    result = false;
                    str += this["DuplicatePhoneWith"] + $": {duplicatePhone.Name} ({duplicatePhone.Phone})";
                }
                
                return (str, result);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new CommonPageEntryViewModel<DtoClient>(model, "Client", EntryType.Add,  GetType());
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
                    DbName = "Email",
                    Name = this["Email"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "Phone",
                    Name = this["Phone"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "MaxDebt",
                    Name = this["MaxDebt"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "Address",
                    Name = this["Address"],
                    IsRequired = false
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
        
    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoClient> args)
    {
        var entry = new ClientEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new ClientEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
    
    [RelayCommand]
    public async Task<Identifier> OnAddReturnAsync()
    {
        var entry = new ClientEntryWindow(new(new(), EntryType.Add, GetType()));
        _ = ShowContentAsync(entry);
        return (await entry.ViewModel.WaitUntilUserSavesAsync()).Id;
    }
    
    [RelayCommand]
    public Task<DtoClient> OnAddReturnFullAsync()
    {
        var entry = new ClientEntryWindow(new(new(), EntryType.Add, GetType()));
        _ = ShowContentAsync(entry);
        return entry.ViewModel.WaitUntilUserSavesAsync();
    }
}