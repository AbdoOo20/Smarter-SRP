# SmartiERP

### Changes Tracking
Current changes finished:
1) Scroll to bottom on item addition
2) Sales invoice update reflects its date
3) Use enter as a way to traverse login page and perform actions

Work in progress:
1) Check reports
2) AutoSuggestBox should be made obsolete. It is heavily buggy. A new alternative will be considered.
### الأصول


### Reports
- [ ] LateDeptions | need to remove dates.
- [ ] Item discounting count | need to remove dates
- [ ] Lost Serial numbers | remove dates and dropdown
- [ ] Item movement | recheck
- [ ] collection percent client | remove dates
- [ ] Items counting | active the dropdown
- [ ] Suppliers Deption | remove dates
- [ ] Client Deption | remove dats
- [ ] Account Movment | remove report
### Other
- [ ] What abount payment-servies.
- [ ] Marketer Disbirment
- [ ] Marketer clients
- [ ] Money functions | What's abount receipting and paying bank ?
	- [ ] Show receipt report comes from back-end
	- [ ] Redesign
	- [ ] DisountSesposit financia | rehcek
	- [ ] Active NextSerialNumber, ManualSerialnumber
	- [ ] 
- [ ] Cahier page
- [ ] Disburment / supply -- update, delete
- [ ] Employee salary counting
- [ ] MarketerId in sale-invoice

 
### الأصناف البديلة
### رقم القطعة - string
### المطاعم
- الطاولات
- طالعات الريسيت
- 



## المنظومات
### المبيعات
- قطاعي
- جملة
- مطاعم
- مختبرات
- ملابس


```
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
TRUNCATE `tinventory`;
TRUNCATE `tinventorycost`;
TRUNCATE `tmarketerdisbursementpermit`;
TRUNCATE `tmarketerdisbursementpermitdetail`;
TRUNCATE `tmarketerinventory`;
TRUNCATE `tmarketersalesinvoice`;
TRUNCATE `tmarketersalesinvoicedetail`;
TRUNCATE `tpurchaseinvoice`;
TRUNCATE `tpurchaseinvoicecost`;
TRUNCATE `tpurchaseinvoicedetail`;
TRUNCATE `tsalesinvoice`;
TRUNCATE `tsalesinvoicedetail`;
TRUNCATE `tsupplypermit`;
TRUNCATE `tsupplypermitdetail`;
```



```
🔁 [Bug]  Account -> AccountType issue
🔁 [Bug] (API) Category/categories issue



🔁 [Bug] WebDAV HttpDelete: Method Not Allowed
🔁 Dto Details via GetAllAsync()
```


```
            CreateNav<DashboardPage>("HomeTab", "Home", SymbolRegular.Home12),
            CreateNav("AccountingTab", SymbolRegular.DataHistogram24,
                CreateNav<AccountingTreePage>("AccountingTreeTab", "AccountingTree", SymbolRegular.TreeDeciduous20),
                CreateNav<ExpenseTypesPage>("ExpensesTab", "ExpenseTypes", SymbolRegular.Money16),
                CreateNav<EntryPage>("EntryTab", "Entry", SymbolRegular.Record12),
                CreateNav<AssetsPage>("AssetsTab", "Assets", SymbolRegular.People12)
            ),
            CreateNav("PersonnelTab", SymbolRegular.People48,
                CreateNav<EmployeesPage>("EmployeesTab", "Employees", SymbolRegular.Person32),
                CreateNav<AssignmentsPage>("AssignmentsTab", "EmployeeAssignments", SymbolRegular.TasksApp20),
                CreateNav<DepartmentsPage>("DepartmentsTab", "Departments", SymbolRegular.NotebookSection20),
                CreateNav<DiscountIncreasePage>("DiscountIncreaseTab", "DiscountIncrease", SymbolRegular.Money16)
            ),
            CreateNav("StoresCategoriesTab", SymbolRegular.People48,
                CreateNav<CategoriesPage>("CategoriesTab", "Categories", SymbolRegular.Diversity24),
                CreateNav<StoresPage>("StoresTab", "Stores", SymbolRegular.StoreMicrosoft16),
                CreateNav<CostCentresPage>("CostCentresTab", "CostCentres", SymbolRegular.CenterHorizontal20),
                CreateNav<UnitsPage>("UnitsTab", "Units", SymbolRegular.CurrencyDollarEuro16),
                CreateNav<SuppliersPage>("SuppliersTab", "Suppliers", SymbolRegular.People24),
                CreateNav<ItemsPage>("ItemsTab", "Items", SymbolRegular.People24),
                CreateNav<SupplyPermitsPage>("SupplyPermitsTab", "SupplyPermits", SymbolRegular.PersonSupport16),
                CreateNav<DisbursementsPermitsPage>("DisbursementsPermitsTab", "DisbursementPermits",
                    SymbolRegular.Receipt28)
            ),
            CreateNav("SalesTab", SymbolRegular.People48,
                CreateNav<SalesInvoicePage>("SalesInvoiceTab", "SalesInvoices", SymbolRegular.Storage16),
                CreateNav<VaultsPage>("VaultsTab", "Safes", SymbolRegular.Storage16),
                CreateNav<BanksPage>("BankTab", "Banks", SymbolRegular.BuildingBank16),
                CreateNav<BankBranchesPage>("BankBranchTab", "BankBranches", SymbolRegular.BuildingBankLink16),
                CreateNav<ClientsPage>("ClientsTab", "Clients", SymbolRegular.BuildingBankLink16),
                CreateNav<SalesCheckoutEntryWindow>("SalesCheckoutTab", "SalesCheckout",
                    SymbolRegular.ShoppingBagArrowLeft20)
            ),
            CreateNav("PurchasesTab", SymbolRegular.People48,
                CreateNav<PurchasesInvoicePage>("PurchasesInvoicesTab", "PurchaseInvoices", SymbolRegular.Storage16)
            ),
            CreateNav("FinancingManagementTab", SymbolRegular.People48,
                CreateNav<FinancingOperationsPage>("FinancingOperationsTab", "FinancingOperations",
                    SymbolRegular.MoneyHand24)
            ),
            CreateNav("MarketingTab", SymbolRegular.People48,
                CreateNav<MarketerDisbursementPermitPage>("MarketerDisbursementPermitTab", "MarketerDisbursementPermit",
                    SymbolRegular.Record48),
                CreateNav<MarketerInfoPage>("MarketerInfoTab", "Marketer", SymbolRegular.Record48)
            ),
            CreateNav<StatementsNavigatorPage>("StatementsNavigator", "StatementsNavigator", SymbolRegular.People48),
            CreateNav<StatementsLookupPage>("StatementsLookupTab", "StatementsLookup", SymbolRegular.MoneyHand24),
            CreateNav("MiscTab", SymbolRegular.People48,
                CreateNav<AccountSettingsPage>("AccountsConfigTab", "AccountSettings", SymbolRegular.Record48)
            ),
```


This code provides a basic framework.  Error handling (e.g., network errors, file I/O errors, checksum mismatches) and more sophisticated update logic (e.g., rollback mechanisms, progress reporting) would need to be added for a production-ready solution.  Consider using a dedicated library for robust zip handling and potentially for the entire update process.


```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

public class Updater
{
    private const string UpdateApiUrl = "YOUR_UPDATE_API_URL"; // Replace with your API endpoint
    private const string AppVersionFile = "appversion.txt"; // File containing current version
    private const string BackupDir = "backup";

    public async Task CheckForUpdatesAsync()
    {
        string currentVersion = GetCurrentVersion();

        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{UpdateApiUrl}?version={currentVersion}");
                response.EnsureSuccessStatusCode();

                var updateInfo = await response.Content.ReadAsAsync<UpdateInfo>();

                if (updateInfo != null && updateInfo.Version > currentVersion)
                {
                    if (await DownloadAndInstallUpdateAsync(updateInfo))
                    {
                        MessageBox.Show("Update successful. Please restart the application.");
                    }
                    else
                    {
                        MessageBox.Show("Update failed.");
                    }
                }
                else
                {
                    MessageBox.Show("Application is up to date.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error checking for updates: {ex.Message}");
        }
    }


    private async Task<bool> DownloadAndInstallUpdateAsync(UpdateInfo updateInfo)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var updateFile = Path.GetTempFileName();
                await client.DownloadFileAsync(updateInfo.DownloadUrl, updateFile);

                // Verify Checksum (Important!)
                if (!VerifyChecksum(updateFile, updateInfo.Checksum))
                {
                    File.Delete(updateFile);
                    return false; //Checksum failed
                }


                //Backup Current Version
                BackupApplication();

                //Extract Update
                ZipFile.ExtractToDirectory(updateFile, "."); // Extract to application directory

                File.Delete(updateFile);

                //User Confirmation (Important!)
                if (MessageBox.Show("Update complete.  Restart application now?", "Update Complete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    //Rollback
                    RestoreApplication();
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            //Rollback on error
            RestoreApplication();
            MessageBox.Show($"Update installation failed: {ex.Message}");
            return false;
        }
    }

    private void BackupApplication()
    {
        if (!Directory.Exists(BackupDir))
        {
            Directory.CreateDirectory(BackupDir);
        }
        string backupPath = Path.Combine(BackupDir, DateTime.Now.ToString("yyyyMMddHHmmss"));
        ZipFile.CreateFromDirectory(".", backupPath);
    }

    private void RestoreApplication()
    {
        if (Directory.Exists(BackupDir))
        {
            string latestBackup = Directory.GetDirectories(BackupDir)[0];
            ZipFile.ExtractToDirectory(latestBackup, ".");
            Directory.Delete(BackupDir, true);
        }
    }


    private string GetCurrentVersion()
    {
        if (File.Exists(AppVersionFile))
        {
            return File.ReadAllText(AppVersionFile);
        }
        return "1.0.0"; // Default version if file not found
    }

    private bool VerifyChecksum(string filePath, string expectedChecksum)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                var hash = md5.ComputeHash(stream);
                var calculatedChecksum = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                return calculatedChecksum == expectedChecksum;
            }
        }
    }
}

public class UpdateInfo
{
    public string Version { get; set; }
    public string DownloadUrl { get; set; }
    public string Checksum { get; set; }
}
```

Remember to replace `"YOUR_UPDATE_API_URL"` with the actual URL of your update API endpoint.  This API should return a JSON object matching the `UpdateInfo` class structure.  This is a simplified example and requires significant enhancements for production use.  Always thoroughly test your update mechanism before deploying it to users.
