using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.Store.Dtos;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.Personnel.Departments;
using Leapay.Client.UserInterface.Personnel.Employees.Windows;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.Employees;

public partial class EmployeesPageViewModel : BasePageViewModel<DtoEmployee>
{
    protected override string Endpoint => "Employee";
    
    public EmployeesPageViewModel()
    {
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;

        ExtraText = this["CountingSalary"];
        ExtraButtonVisibility = true;
        
    }

    private TableImportModel<DtoEmployee> GetImportModel()
    {
        var departments = App.GetService<DepartmentsPageViewModel>();

        return new TableImportModel<DtoEmployee>
        {
            SynchronisationInstances = [ this, departments ],
            RefreshPage = GetType(),
            ItemProcessMechanism = async (model, token) =>
            {
                if (!token.IsCancellationRequested)
                {
                    var vm = new EmployeePageEntryViewModel(model, EntryType.Add, GetType(), true);
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
                
                return (str, result);
            },
            ItemValidationMechanism = model =>
            {
                var vm = new EmployeePageEntryViewModel(model, EntryType.Add,  GetType(), true);
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
                    HasForeignKey = true,
                    ForeignKeyMatchingMechanism = tuple =>
                    {
                        (tuple.Item1 as DtoEmployee)?.DepartmentId =
                            departments.Models.FirstOrDefault(m => m.Name == tuple.Item2)?.Id ?? new Identifier();
                    },
                    ForeignKeySatisfactionMechanism = tuple =>
                    {
                        return departments.Models.Any(c => c.Name == tuple);
                    },
                    ItemAdditionMechanism = async name =>
                    {
                        var vm = new DepartmentEntryWindow(new WindowEntryModel<DtoDepartment>(new DtoDepartment
                        {
                            Name = name,
                        }, EntryType.Add, typeof(DepartmentsPageViewModel)));

                        await ShowContentAsync(vm);
                    },
                    DbName = "DepartmentId",
                    Name = this["DepartmentName"],
                    IsRequired = true
                },
                new()
                {
                    DbName = "Department",
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
        
    protected override async void OnEdit(object? sender, DataGridButtonEventArgs<DtoEmployee> args)
    {
        var entry = new EmployeeEntryWindow(new(args.Model, EntryType.Edit, GetType()));
        await ShowContentAsync(entry, EntryType.Edit);
    }

    protected override async Task OnAddAsync()
    {
        var entry = new EmployeeEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry);
    }

    protected override async void OnExtraButton()
    {
        var entry = new SalaryCountingEntryWindow(new(new(), EntryType.Add, GetType()));
        await ShowContentAsync(entry, name: "CountingSalary");
    }
}