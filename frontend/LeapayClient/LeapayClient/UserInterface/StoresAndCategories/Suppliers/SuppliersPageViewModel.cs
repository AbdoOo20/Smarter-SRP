using System.Collections.ObjectModel;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Suppliers;

public partial class SuppliersPageViewModel : BasePageViewModel<DtoSupplier>
{
    protected override string Endpoint => "Supplier";
    
    public SuppliersPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    private TableImportModel<DtoSupplier> GetImportModel()
    {
        return new TableImportModel<DtoSupplier>
        {
            SynchronisationInstances = [ this ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CommonPageEntryViewModel<DtoSupplier>(model, "Supplier", EntryType.Add, GetType());
                    return await vm.SaveWithResultAsync();
                }

                return (this["OperationCancelled"], false);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new CommonPageEntryViewModel<DtoSupplier>(model, "Supplier", EntryType.Add,  GetType());
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
                
                var duplicatePhone = Models.FirstOrDefault(c => c.Phone == model?.Phone);

                if (duplicatePhone is not null)
                {
                    result = false;
                    str += this["DuplicatePhoneWith"] + $": {duplicatePhone.Name} ({duplicatePhone.Phone})";
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
    
    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoSupplier> args)
    {
        var entry = new SupplierEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()    {
        var entry = new SupplierEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}