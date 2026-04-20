using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;

namespace Fly.Shared.Modules.Global.SeededData;

public class ConfigSeed
{
 private const string c = "";
 private static readonly DateTime d = new(2024, 7, 2);

 public static IEnumerable<DtoConfig>? Configs = new List<DtoConfig>
 {
  Create(
   id: "adecdcbe-5c66-4069-9f41-ee29aa4a6da1"
   , safeAccountId: "adecdcbe-5c66-4069-9f41-ee29aa4a6da1"
   , bankAccountId: "adecdcbe-5c66-4069-9f41-ee29aa4a6da2"
   , clientsAccountId: "adecdcbe-5c66-4069-9f41-ee29aa4a6dc2"
   , expenseAccountId: "0E54960C-8A5B-473D-8022-C4A47C651001"
   , employeesAccountId: "13440fba-02cd-4f4b-843b-6159611c3fac"
   , supplyersAccountId: "2b5ee402-49f1-444c-9a86-bf76e0ecbf90"
   , purchaseInvoicesAccountId: "0E54960C-8A5B-473D-8022-C4A47C651002"
   , generalClientAccountAccountId: "adecdcbe-5c66-4069-9f41-ee29aa4a6dc3"
   , generalSupplierAccountAccountId: "2b5ee402-49f1-444c-9a86-bf76e0ecbf91"
   , salesInvoicesAccountId: "0E54960C-8A5B-473D-8022-C4A47C651102"
   , marketerInvoicesAccountId: "0E54960C-8A5B-473D-8022-C4A47C651103"
   , assetsAccount: "ad630d24-f791-4d5a-8104-27595e273112"
   , generalShareAccount: "0E54960C-8A5B-473D-8022-C8847C651103"
   , discountAccount: "0E54960C-8A5B-473D-8022-C8847C651104"
   , manufacturingFacilityAccount: "0e54960c-8a5b-473d-8022-c4a47c651106"
   , maintenanceSectorAccount: "0e54960c-8a5b-473d-8022-c4a47c651105"
   , internalMaintenanceSectorAccount: "0E54960C-8A5B-473D-8022-C4A47C651107"
   , internalManufacturingFacilityAccount: "0E54960C-8A5B-473D-8022-C4A47C651108"
  )
 };
 //
 //

 // Natural
 private static DtoConfig Create(string id
  , string safeAccountId
  , string bankAccountId
  , string clientsAccountId
  , string expenseAccountId
  , string employeesAccountId
  , string supplyersAccountId
  , string purchaseInvoicesAccountId
  , string generalClientAccountAccountId
  , string generalSupplierAccountAccountId
  , string salesInvoicesAccountId
  , string marketerInvoicesAccountId
  , string assetsAccount
  , string generalShareAccount
  , string discountAccount
  , string manufacturingFacilityAccount
  , string internalManufacturingFacilityAccount
  , string maintenanceSectorAccount
  , string internalMaintenanceSectorAccount
 )
 {
  return new DtoConfig
  {
   Id = Identifier.Parse(id)
   ,
   SafeAccount = Guid.Parse(safeAccountId).ToString()
   ,
   BankAccount = Guid.Parse(bankAccountId).ToString()
   ,
   ClientsAccount = Guid.Parse(clientsAccountId).ToString()
   ,
   ExpenseAccount = Guid.Parse(expenseAccountId).ToString()
   ,
   EmployeesAccount = Guid.Parse(employeesAccountId).ToString()
   ,
   SupplyersAccount = Guid.Parse(supplyersAccountId).ToString()
   ,
   PurchaseInvoicesAccount = Guid.Parse(purchaseInvoicesAccountId).ToString()
   ,
   GeneralClientAccountAccount = Guid.Parse(generalClientAccountAccountId).ToString()
   ,
   GeneralSupplierAccountAccount = Guid.Parse(generalSupplierAccountAccountId).ToString()
   ,
   SalesInvoicesAccountId = Guid.Parse(salesInvoicesAccountId).ToString()
   ,
   MarketerInvoicesAccountId = Guid.Parse(marketerInvoicesAccountId).ToString()
   ,
   AssetsAccount = Guid.Parse(assetsAccount).ToString()
   ,
   GeneralShareAccount = Guid.Parse(generalShareAccount).ToString()
   ,
   DiscountAccount = Guid.Parse(discountAccount).ToString(),
   ManufacturingFacilitiesAccount = Guid.Parse(manufacturingFacilityAccount).ToString(),
   MaintenanceSectorsAccount = Guid.Parse(maintenanceSectorAccount).ToString(),
   InternalMaintenanceSectorsAccount = Guid.Parse(internalMaintenanceSectorAccount).ToString(),
   InternalManufacturingFacilitiesAccount = Guid.Parse(internalManufacturingFacilityAccount).ToString()
  };
 }
}