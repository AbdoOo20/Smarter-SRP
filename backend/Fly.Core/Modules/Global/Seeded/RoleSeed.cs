using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;

namespace Fly.Core.Modules.Global.Seeded;


/*
 * 
 * 
 * 
    var el = document.querySelectorAll('h3');
    let controllers = [];
    el.forEach( (e) => {
        var txt = e.querySelector('a>span').innerHTML;
        controllers.push(txt);
    });
    JSON.stringify(controllers);
 * 
 * 
'["Account","Assets","Bank","BankBranch","Category","Client","Config","CostCenter","Currency","Department","DisbursementPermit","DiscountIncrease","Employee","EmployeeAssignment","Entry","ExpenseTypes","Financial","Group","Item","ItemQuantity","Marketer","MarketerDisbursementPermit","MarketerMobile","MarketerSalesInvoice","Purchase","Reports","Safe","Sales","Signing","Statement","Store","Supplier","SupplyPermit","Unit","User"]'
 */

public static class RoleSeed
{
    public static List<DtoRole> Roles = [];

    private static void SetDefault()
    {
        Roles.AddRange([

        new DtoRole()
        {
            Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB001"),
            Key = "Gl.Ghost",
            Name = "Ghost",
            WindowName = string.Empty,
            Description = "إدارة الدعم الفني",
        },
        new DtoRole()
        {
            Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB002"),
            Key = "Gl.Admin",
            Name = "Admin",
            WindowName = string.Empty,
            Description = "الإدارة",
        },
        ]);
    }

    private static void SeedMarketerRoles()
    {
        Roles.AddRange([
            new DtoRole()
            {
                Id = Identifier.Parse("08B4DAE1-2EEF-4509-B0E0-1FFFFFFFF101"),
                Key = "Gl.Marketer",
                Name = "Marketer",
                WindowName = string.Empty,
                Description = "صلاحية مسوق خارجي",
            }]);
    }

    private static void SeedReports()
    {
        int roleNumber = 0; // Starting role number
        string staticIdPart = "08B4DAE1-3EEF-4509-B1E6-2233212";
        string[] reports =
        [
            "AccountMovements", "TheClientDebtion", "TheClientDebtionDetails", "ClientsOverdueDebts",
            "TheCSupplierDebtion", "TheRepresentativeDebtionDetails", "MarketerCredtions", "ItemsStoragesQuantities",
            "ItemsMarkersQuantities", "ItemsMovements", "RepresentativeItemsMovements", "MaintenanceInvoicesByStatus",
            "FactoringInvoicesByStatus", "MissedSerial", "SalesItemsProfit2", "SalesCollectionInsights",
            "MaxDiscountItemsBased", "DiscountItemsBased", "RepresentativeAmounts", "FinancialHestory",
            "ExpencesHestory", "SalesItemsProfit3", "DamageItems"
        ];
        string[] actions = { "Show" };
        foreach (string report in reports)
        {
            foreach (string action in actions)
            {
                roleNumber++;
                var role = new DtoRole
                {
                    Id = Identifier.Parse($"{staticIdPart}{roleNumber:00000}"),
                    Key = $"StatementsNavigator.{report}-{action}",
                    Name = $"{report}",
                    WindowName = string.Empty,
                    Description = $"Performs the {action} action on the {report} Report"
                };
                Roles.Add(role);
            }
        }
    }

    private static void SeedFinancial()
    {
        int roleNumber = 0; // Starting role number
        string staticIdPart = "08B4DAE1-3EEF-4509-B1E7-2253252";
        string[] financialActions = [ "AccountPaymentReceipt", "DiscountItemPaymentReceipt", "ExpensePaymentReceipt",
            "ClientPaymentReceipt", "EmployeePaymentReceipt", "SupplierPaymentReceipt", "ClientRepresentativeReceipt"
        ];
        string[] actions = { "Create" };
        foreach (string financialAction in financialActions)
        {
            foreach (string action in actions)
            {
                roleNumber++;
                var role = new DtoRole
                {
                    Id = Identifier.Parse($"{staticIdPart}{roleNumber:00000}"),
                    Key = $"FinancialOperation.{financialAction}-{action}",
                    Name = $"{financialAction}",
                    WindowName = string.Empty,
                    Description = $"Performs the {action} action on the {financialAction} Report"
                };
                Roles.Add(role);
                //Console.WriteLine("current id is {0}", role.Id);
                //Console.WriteLine("current number is {0}", roleNumber);
            }
        }
    }
    
    private static void AutoGenerate()
    {
        string[] controllerNames = {
            "Account", "ExpenseTypes", "Entry", "Assets",
            "Employee", "EmployeeAssignment", "Department", "DiscountIncrease",
            "Category", "Store", "CostCenter", "Unit", "Supplier", "Item", "ItemVariance", "ItemComposition", "SupplyPermit", "DisbursementPermit",
            "Sales", "Safe", "Bank", "BankBranch", "Client", "SellReturnInvoice", "QuantityCrediting", "FastSales",
            "Purchase", "ManufacturingFacility",  "FactoringInvoice", "MaintenanceSector", "MaintenanceInvoice", 
            "Marketer", "MarketerDisbursementPermit",
            "Financial",
            "Config",
            "Group", "User", "ItemsTransferInvoice"
        };
        string[] actions = { "Show", "Create", "Update", "Delete", "Export", "Import", "Print" };
        string staticIdPart = "08B4DAE1-2EEF-4509-B1E6-1FA9B4A";
        int roleNumber = 1; // Starting role number

        foreach (string controllerName in controllerNames)
        {
            foreach (string action in actions)
            {
                Roles.Add(new DtoRole
                {
                    Id = Identifier.Parse($"{staticIdPart}{roleNumber:00000}"),
                    Key = $"{controllerName}.{action}",
                    Name = $"{action} {controllerName}",
                    WindowName = string.Empty,
                    Description = $"Performs the {action} action on the {controllerName} entity"
                });

                roleNumber++; // Increment the role number for the next role
            }
        }
    }



    public static void SetAll()
    {
        //SetDefault();
        SeedMarketerRoles();
    }

    public static void Generate()
    {
        Roles = [];
        //  SetDefault();
        SeedFinancial();
        SeedReports();
        AutoGenerate();
        // SeedMarketerRoles();
        //Roles.ForEach(f => Console.WriteLine("permession id is {0}, for {1}", f.Id, f.Name));
    }

    //roleNumber++;
    //roles.Add(new DtoRole
    //{
    //    Id = Identifier.Parse($"D9C5401A-CC1F-4A27-9B2B-7EB5B80F698C"),
    //    Key = $"GHOST",
    //    Name = $"Ghost",
    //    WindowName = string.Empty,
    //    Description = $"Ghost Permission",
    //    IsSecret = true
    //});
    //roleNumber++;
    //roles.Add(new DtoRole
    //{
    //    Id = Identifier.Parse($"{staticIdPart}{roleNumber:00000}"),
    //    Key = $"General.Marketer",
    //    Name = $"Marketer",
    //    WindowName = string.Empty,
    //    Description = $"Marketer Permission"
    //});
    //public static void Add()
    //{
    //    roles = new List<DtoRole>()
    //    {
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB001"),
    //        Name = "Ghost"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB002"),
    //        Name = "Admin"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB003"),
    //        Name = "Add Account"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB004"),
    //        Name = "Edit Account"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB005"),
    //        Name = "Delete Account"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB006"),
    //        Name = "View Account"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB007"),
    //        Name = "Add Assets"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB008"),
    //        Name = "Edit Assets"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB009"),
    //        Name = "Delete Assets"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB010"),
    //        Name = "View Assets"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB011"),
    //        Name = "Add Category"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB012"),
    //        Name = "Edit Category"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB013"),
    //        Name = "Delete Category"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB014"),
    //        Name = "View Category"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB015"),
    //        Name = "Add CostCenter"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB016"),
    //        Name = "Edit CostCenter"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB017"),
    //        Name = "Delete CostCenter"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB018"),
    //        Name = "View CostCenter"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB019"),
    //        Name = "Add Currency"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB020"),
    //        Name = "Edit Currency"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB021"),
    //        Name = "Delete Currency"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB022"),
    //        Name = "View Currency"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB023"),
    //        Name = "Add Department"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB024"),
    //        Name = "Edit Department"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB025"),
    //        Name = "Delete Department"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB026"),
    //        Name = "View Department"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB027"),
    //        Name = "Add DisbursementPermit"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB028"),
    //        Name = "Edit DisbursementPermit"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB029"),
    //        Name = "Delete DisbursementPermit"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB030"),
    //        Name = "View DisbursementPermit"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB031"),
    //        Name = "Add DiscountIncrease"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB032"),
    //        Name = "Edit DiscountIncrease"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB033"),
    //        Name = "Delete DiscountIncrease"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB034"),
    //        Name = "View DiscountIncrease"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB035"),
    //        Name = "Add Employee"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB036"),
    //        Name = "Edit Employee"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB037"),
    //        Name = "Delete Employee"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB038"),
    //        Name = "View Employee"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB039"),
    //        Name = "Add EmployeeAssignment"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB040"),
    //        Name = "Edit EmployeeAssignment"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB041"),
    //        Name = "Delete EmployeeAssignment"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB042"),
    //        Name = "View EmployeeAssignment"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB043"),
    //        Name = "Add Entry"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB044"),
    //        Name = "Edit Entry"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB045"),
    //        Name = "Delete Entry"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB046"),
    //        Name = "View Entry"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB047"),
    //        Name = "Add ExpenseTypes"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB048"),
    //        Name = "Edit ExpenseTypes"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB049"),
    //        Name = "Delete ExpenseTypes"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB050"),
    //        Name = "View ExpenseTypes"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB051"),
    //        Name = "Add Item"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB052"),
    //        Name = "Edit Item"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB053"),
    //        Name = "Delete Item"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB054"),
    //        Name = "View Item"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB055"),
    //        Name = "Add Signing"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB056"),
    //        Name = "Edit Signing"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB057"),
    //        Name = "Delete Signing"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB058"),
    //        Name = "View Signing"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB059"),
    //        Name = "Add Store"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB060"),
    //        Name = "Edit Store"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB061"),
    //        Name = "Delete Store"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB062"),
    //        Name = "View Store"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB063"),
    //        Name = "Add Supplier"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB064"),
    //        Name = "Edit Supplier"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB065"),
    //        Name = "Delete Supplier"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB066"),
    //        Name = "View Supplier"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB066"),
    //        Name = "View Supplier"
    //    },
    //    //  ***************************************************************
    //    //  ***************************************************************
    //    //  ***************************************************************
    //    //  ***************************************************************
    //    //  ***************************************************************
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB067"),
    //        Name = "Add Tax"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB068"),
    //        Name = "Edit Tax"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB069"),
    //        Name = "Delete Tax"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB070"),
    //        Name = "View Tax"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB071"),
    //        Name = "Add User"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB072"),
    //        Name = "Edit User"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB073"),
    //        Name = "Delete User"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB074"),
    //        Name = "View User"
    //    },


    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB075"),
    //        Name = "Add UserGroup"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB076"),
    //        Name = "Edit UserGroup"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB077"),
    //        Name = "Delete UserGroup"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB078"),
    //        Name = "View UserGroup"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB079"),
    //        Name = "Add Vendor"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB080"),
    //        Name = "Edit Vendor"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB081"),
    //        Name = "Delete Vendor"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB082"),
    //        Name = "View Vendor"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB083"),
    //        Name = "Add Warehouse"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB084"),
    //        Name = "Edit Warehouse"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB085"),
    //        Name = "Delete Warehouse"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB086"),
    //        Name = "View Warehouse"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB087"),
    //        Name = "Add WorkOrder"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB088"),
    //        Name = "Edit WorkOrder"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB089"),
    //        Name = "Delete WorkOrder"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB090"),
    //        Name = "View WorkOrder"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB091"),
    //        Name = "Add WorkOrderStatus"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB092"),
    //        Name = "Edit WorkOrderStatus"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB093"),
    //        Name = "Delete WorkOrderStatus"
    //    },
    //    new DtoRole()
    //    {
    //        Id = Identifier.Parse("08B4DAE1-2EEF-4509-B1E6-1FA9B4ACB094"),
    //        Name = "View WorkOrderStatus"
    //    },
    //};
    //}

}
