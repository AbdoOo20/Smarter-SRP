using System.Collections.ObjectModel;
using Fly.Shared.Modules.Marketers.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Marketer.MarketerEntry;

public partial class MarketerPageViewModel : BasePageViewModel<DtoMarketer>
{
    protected override string Endpoint => "Marketer";
    
    public MarketerPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
        RegisterForImport(GetImportModel);
    }

    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoMarketer> args)
    {
        var entry = new MarketerEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new MarketerEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }
    
    private TableImportModel<DtoMarketer> GetImportModel()
    {
        return new TableImportModel<DtoMarketer>
        {
            RefreshPage = GetType(),
            SynchronisationInstances = [ this ],
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new MarketerEntryViewModel(model, EntryType.Add, GetType(), true);
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
                var vm = new MarketerEntryViewModel(model, EntryType.Add, GetType(), true);
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
                    DbName = "Salary",
                    Name = this["Salary"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "Percentage",
                    Name = this["Percentage"],
                    IsRequired = false
                },
                new()
                {
                    DbName = "IsRepresentative",
                    Name = this["IsRepresentative"],
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
}