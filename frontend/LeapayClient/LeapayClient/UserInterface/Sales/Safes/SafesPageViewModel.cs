using System.Collections.ObjectModel;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.Safes;

public partial class SafesPageViewModel : BasePageViewModel<DtoSafe>
{
    protected override string Endpoint => "Safe";
    
    public SafesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoSafe> args)
    {
        var entry = new SafeEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new SafeEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
    
    private TableImportModel<DtoSafe> GetImportModel()
    {
        return new TableImportModel<DtoSafe>
        {
            SynchronisationInstances = [ this ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (category, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new SafeEntryViewModel(category, EntryType.Add, GetType());
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
                    str += this["DuplicateName"];
                }
                
                return (str, result);
            },
            ItemValidationMechanism = safe =>
            {
                var vm = new SafeEntryViewModel(safe, EntryType.Add, GetType());
                var enforce = vm.PubEnforce();

                if (!enforce.IsSuccess()) return (enforce.GetDisplayMessage(), false);

                return (this["Success"], true);
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
                    DbName = "Note",
                    Name = this["Note"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = false,
                    DbName = "PeymentMode",
                    Name = this["PeymentMode"],
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
                new()
                {
                    DbName = "LossPercent",
                    Name = this["LossPercent"],
                    IsRequired = false
                },
            })
        };
    }
}