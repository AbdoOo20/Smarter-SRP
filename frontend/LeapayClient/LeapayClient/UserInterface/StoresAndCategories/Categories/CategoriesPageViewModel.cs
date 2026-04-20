using System.Collections.ObjectModel;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.StoresAndCategories.Categories;

public partial class CategoriesPageViewModel : BasePageViewModel<DtoCategory>
{
    protected override string Endpoint => "Category";

    public CategoriesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    private TableImportModel<DtoCategory> GetImportModel()
    {
        return new TableImportModel<DtoCategory>
        {
            RefreshPage = GetType(),
            SynchronisationInstances = [ this ],
            ItemProcessMechanism = async (category, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new CategoryPageEntryViewModel(category, EntryType.Add, GetType(), true);
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
            ItemValidationMechanism = category =>
            {
                var vm = new CategoryPageEntryViewModel(category, EntryType.Add, GetType(), true);
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
                    DbName = "Description",
                    Name = this["Description"],
                    IsRequired = false
                },
                new()
                {
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoCategory)?.CateguryId =
                            Models.FirstOrDefault(m => m.Name == tuple.Item2)?.Id;
                    },
                    ForeignKeySatisfactionMechanism = tuple => { return Models.Any(c => c.Name == tuple); },
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new CategoryEntryWindow(new WindowEntryModel<DtoCategory>(new DtoCategory
                        {
                            Name = name,
                        }, EntryType.Add, typeof(CategoriesPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    DbName = "CateguryId",
                    Name = this["ParentCategory"],
                    IsRequired = false
                }
            })
        };
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoCategory> args)
    {
        var entry = new CategoryEntryWindow(new WindowEntryModel<DtoCategory>(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new CategoryEntryWindow(
            new WindowEntryModel<DtoCategory>(new DtoCategory(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
}