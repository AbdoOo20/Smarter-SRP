using System.Collections.ObjectModel;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Units;

public partial class UnitsPageViewModel : BasePageViewModel<DtoUnit>
{
    protected override string Endpoint => "Unit";
    
    public UnitsPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    private TableImportModel<DtoUnit> GetImportModel()
    {
        return new TableImportModel<DtoUnit>
        {
            RefreshPage = GetType(),
            SynchronisationInstances = [ this ],
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CommonPageEntryViewModel<DtoUnit>(model, "Unit", EntryType.Add, GetType());
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
            ItemValidationMechanism = model =>
            {
                var vm = new CommonPageEntryViewModel<DtoUnit>(model, "Unit", EntryType.Add,  GetType());
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
                    DbName = "Description",
                    Name = this["Description"],
                    IsRequired = false
                },
            })
        };
    }
        
    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoUnit> args)
    {
        var entry = new UnitEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new UnitEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}