using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddPrincipalRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-467172700018"));

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00099"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the ItemVariance entity", "ItemVariance.Show", "Show ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00100"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the ItemVariance entity", "ItemVariance.Create", "Create ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00101"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the ItemVariance entity", "ItemVariance.Update", "Update ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00102"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the ItemVariance entity", "ItemVariance.Delete", "Delete ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00103"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the ItemVariance entity", "ItemVariance.Export", "Export ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00104"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the ItemVariance entity", "ItemVariance.Import", "Import ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00105"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the ItemVariance entity", "ItemVariance.Print", "Print ItemVariance" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00106"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the ItemComposition entity", "ItemComposition.Show", "Show ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00107"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the ItemComposition entity", "ItemComposition.Create", "Create ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00108"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the ItemComposition entity", "ItemComposition.Update", "Update ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00109"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the ItemComposition entity", "ItemComposition.Delete", "Delete ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00110"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the ItemComposition entity", "ItemComposition.Export", "Export ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00111"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the ItemComposition entity", "ItemComposition.Import", "Import ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00112"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the ItemComposition entity", "ItemComposition.Print", "Print ItemComposition" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00113"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the SupplyPermit entity", "SupplyPermit.Show", "Show SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00114"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the SupplyPermit entity", "SupplyPermit.Create", "Create SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00115"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the SupplyPermit entity", "SupplyPermit.Update", "Update SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00116"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the SupplyPermit entity", "SupplyPermit.Delete", "Delete SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00117"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the SupplyPermit entity", "SupplyPermit.Export", "Export SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00118"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the SupplyPermit entity", "SupplyPermit.Import", "Import SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00119"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the SupplyPermit entity", "SupplyPermit.Print", "Print SupplyPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00120"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the DisbursementPermit entity", "DisbursementPermit.Show", "Show DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00121"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the DisbursementPermit entity", "DisbursementPermit.Create", "Create DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00122"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the DisbursementPermit entity", "DisbursementPermit.Update", "Update DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00123"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the DisbursementPermit entity", "DisbursementPermit.Delete", "Delete DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00124"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the DisbursementPermit entity", "DisbursementPermit.Export", "Export DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00125"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the DisbursementPermit entity", "DisbursementPermit.Import", "Import DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00126"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the DisbursementPermit entity", "DisbursementPermit.Print", "Print DisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00127"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Sales entity", "Sales.Show", "Show Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00128"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Sales entity", "Sales.Create", "Create Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00129"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Sales entity", "Sales.Update", "Update Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00130"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Sales entity", "Sales.Delete", "Delete Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00131"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Sales entity", "Sales.Export", "Export Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00132"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Sales entity", "Sales.Import", "Import Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00133"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Sales entity", "Sales.Print", "Print Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00134"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Safe entity", "Safe.Show", "Show Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00135"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Safe entity", "Safe.Create", "Create Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00136"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Safe entity", "Safe.Update", "Update Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00137"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Safe entity", "Safe.Delete", "Delete Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00138"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Safe entity", "Safe.Export", "Export Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00139"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Safe entity", "Safe.Import", "Import Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00140"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Safe entity", "Safe.Print", "Print Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00141"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Bank entity", "Bank.Show", "Show Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00142"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Bank entity", "Bank.Create", "Create Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00143"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Bank entity", "Bank.Update", "Update Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00144"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Bank entity", "Bank.Delete", "Delete Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00145"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Bank entity", "Bank.Export", "Export Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00146"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Bank entity", "Bank.Import", "Import Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00147"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Bank entity", "Bank.Print", "Print Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00148"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the BankBranch entity", "BankBranch.Show", "Show BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00149"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the BankBranch entity", "BankBranch.Create", "Create BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00150"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the BankBranch entity", "BankBranch.Update", "Update BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00151"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the BankBranch entity", "BankBranch.Delete", "Delete BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00152"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the BankBranch entity", "BankBranch.Export", "Export BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00153"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the BankBranch entity", "BankBranch.Import", "Import BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00154"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the BankBranch entity", "BankBranch.Print", "Print BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00155"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Client entity", "Client.Show", "Show Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00156"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Client entity", "Client.Create", "Create Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00157"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Client entity", "Client.Update", "Update Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00158"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Client entity", "Client.Delete", "Delete Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00159"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Client entity", "Client.Export", "Export Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00160"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Client entity", "Client.Import", "Import Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00161"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Client entity", "Client.Print", "Print Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00162"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the SellReturnInvoice entity", "SellReturnInvoice.Show", "Show SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00163"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the SellReturnInvoice entity", "SellReturnInvoice.Create", "Create SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00164"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the SellReturnInvoice entity", "SellReturnInvoice.Update", "Update SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00165"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the SellReturnInvoice entity", "SellReturnInvoice.Delete", "Delete SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00166"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the SellReturnInvoice entity", "SellReturnInvoice.Export", "Export SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00167"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the SellReturnInvoice entity", "SellReturnInvoice.Import", "Import SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00168"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the SellReturnInvoice entity", "SellReturnInvoice.Print", "Print SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00169"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the QuantityCrediting entity", "QuantityCrediting.Show", "Show QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00170"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the QuantityCrediting entity", "QuantityCrediting.Create", "Create QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00171"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the QuantityCrediting entity", "QuantityCrediting.Update", "Update QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00172"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the QuantityCrediting entity", "QuantityCrediting.Delete", "Delete QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00173"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the QuantityCrediting entity", "QuantityCrediting.Export", "Export QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00174"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the QuantityCrediting entity", "QuantityCrediting.Import", "Import QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00175"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the QuantityCrediting entity", "QuantityCrediting.Print", "Print QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00176"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the FastSales entity", "FastSales.Show", "Show FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00177"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the FastSales entity", "FastSales.Create", "Create FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00178"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the FastSales entity", "FastSales.Update", "Update FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00179"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the FastSales entity", "FastSales.Delete", "Delete FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00180"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the FastSales entity", "FastSales.Export", "Export FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00181"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the FastSales entity", "FastSales.Import", "Import FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00182"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the FastSales entity", "FastSales.Print", "Print FastSales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00183"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Purchase entity", "Purchase.Show", "Show Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00184"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Purchase entity", "Purchase.Create", "Create Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00185"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Purchase entity", "Purchase.Update", "Update Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00186"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Purchase entity", "Purchase.Delete", "Delete Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00187"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Purchase entity", "Purchase.Export", "Export Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00188"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Purchase entity", "Purchase.Import", "Import Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00189"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Purchase entity", "Purchase.Print", "Print Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00190"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the FinancingOperations entity", "FinancingOperations.Show", "Show FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00191"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the FinancingOperations entity", "FinancingOperations.Create", "Create FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00192"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the FinancingOperations entity", "FinancingOperations.Update", "Update FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00193"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the FinancingOperations entity", "FinancingOperations.Delete", "Delete FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00194"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the FinancingOperations entity", "FinancingOperations.Export", "Export FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00195"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the FinancingOperations entity", "FinancingOperations.Import", "Import FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00196"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the FinancingOperations entity", "FinancingOperations.Print", "Print FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00197"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the ManufacturingFacility entity", "ManufacturingFacility.Show", "Show ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00198"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the ManufacturingFacility entity", "ManufacturingFacility.Create", "Create ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00199"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the ManufacturingFacility entity", "ManufacturingFacility.Update", "Update ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00200"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the ManufacturingFacility entity", "ManufacturingFacility.Delete", "Delete ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00201"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the ManufacturingFacility entity", "ManufacturingFacility.Export", "Export ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00202"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the ManufacturingFacility entity", "ManufacturingFacility.Import", "Import ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00203"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the ManufacturingFacility entity", "ManufacturingFacility.Print", "Print ManufacturingFacility" });

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00204"), "Performs the Show action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00204"), false, false, "FactoringInvoice.Show", "Show FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00205"), "Performs the Create action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00205"), false, false, "FactoringInvoice.Create", "Create FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00206"), "Performs the Update action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00206"), false, false, "FactoringInvoice.Update", "Update FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00207"), "Performs the Delete action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00207"), false, false, "FactoringInvoice.Delete", "Delete FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00208"), "Performs the Export action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00208"), false, false, "FactoringInvoice.Export", "Export FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00209"), "Performs the Import action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00209"), false, false, "FactoringInvoice.Import", "Import FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00210"), "Performs the Print action on the FactoringInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00210"), false, false, "FactoringInvoice.Print", "Print FactoringInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00211"), "Performs the Show action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00211"), false, false, "MaintenanceSector.Show", "Show MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00212"), "Performs the Create action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00212"), false, false, "MaintenanceSector.Create", "Create MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00213"), "Performs the Update action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00213"), false, false, "MaintenanceSector.Update", "Update MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00214"), "Performs the Delete action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00214"), false, false, "MaintenanceSector.Delete", "Delete MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00215"), "Performs the Export action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00215"), false, false, "MaintenanceSector.Export", "Export MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00216"), "Performs the Import action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00216"), false, false, "MaintenanceSector.Import", "Import MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00217"), "Performs the Print action on the MaintenanceSector entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00217"), false, false, "MaintenanceSector.Print", "Print MaintenanceSector", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00218"), "Performs the Show action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00218"), false, false, "MaintenanceInvoice.Show", "Show MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00219"), "Performs the Create action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00219"), false, false, "MaintenanceInvoice.Create", "Create MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00220"), "Performs the Update action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00220"), false, false, "MaintenanceInvoice.Update", "Update MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00221"), "Performs the Delete action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00221"), false, false, "MaintenanceInvoice.Delete", "Delete MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00222"), "Performs the Export action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00222"), false, false, "MaintenanceInvoice.Export", "Export MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00223"), "Performs the Import action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00223"), false, false, "MaintenanceInvoice.Import", "Import MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00224"), "Performs the Print action on the MaintenanceInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00224"), false, false, "MaintenanceInvoice.Print", "Print MaintenanceInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00225"), "Performs the Show action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00225"), false, false, "Marketer.Show", "Show Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00226"), "Performs the Create action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00226"), false, false, "Marketer.Create", "Create Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00227"), "Performs the Update action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00227"), false, false, "Marketer.Update", "Update Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00228"), "Performs the Delete action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00228"), false, false, "Marketer.Delete", "Delete Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00229"), "Performs the Export action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00229"), false, false, "Marketer.Export", "Export Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00230"), "Performs the Import action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00230"), false, false, "Marketer.Import", "Import Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00231"), "Performs the Print action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00231"), false, false, "Marketer.Print", "Print Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00232"), "Performs the Show action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00232"), false, false, "MarketerDisbursementPermit.Show", "Show MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00233"), "Performs the Create action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00233"), false, false, "MarketerDisbursementPermit.Create", "Create MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00234"), "Performs the Update action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00234"), false, false, "MarketerDisbursementPermit.Update", "Update MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00235"), "Performs the Delete action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00235"), false, false, "MarketerDisbursementPermit.Delete", "Delete MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00236"), "Performs the Export action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00236"), false, false, "MarketerDisbursementPermit.Export", "Export MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00237"), "Performs the Import action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00237"), false, false, "MarketerDisbursementPermit.Import", "Import MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00238"), "Performs the Print action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00238"), false, false, "MarketerDisbursementPermit.Print", "Print MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00239"), "Performs the Show action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00239"), false, false, "Financial.Show", "Show Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00240"), "Performs the Create action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00240"), false, false, "Financial.Create", "Create Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00241"), "Performs the Update action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00241"), false, false, "Financial.Update", "Update Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00242"), "Performs the Delete action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00242"), false, false, "Financial.Delete", "Delete Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00243"), "Performs the Export action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00243"), false, false, "Financial.Export", "Export Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00244"), "Performs the Import action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00244"), false, false, "Financial.Import", "Import Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00245"), "Performs the Print action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00245"), false, false, "Financial.Print", "Print Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00246"), "Performs the Show action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00246"), false, false, "Config.Show", "Show Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00247"), "Performs the Create action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00247"), false, false, "Config.Create", "Create Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00248"), "Performs the Update action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00248"), false, false, "Config.Update", "Update Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00249"), "Performs the Delete action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00249"), false, false, "Config.Delete", "Delete Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00250"), "Performs the Export action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00250"), false, false, "Config.Export", "Export Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00251"), "Performs the Import action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00251"), false, false, "Config.Import", "Import Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00252"), "Performs the Print action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00252"), false, false, "Config.Print", "Print Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00253"), "Performs the Show action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00253"), false, false, "Group.Show", "Show Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00254"), "Performs the Create action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00254"), false, false, "Group.Create", "Create Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00255"), "Performs the Update action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00255"), false, false, "Group.Update", "Update Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00256"), "Performs the Delete action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00256"), false, false, "Group.Delete", "Delete Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00257"), "Performs the Export action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00257"), false, false, "Group.Export", "Export Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00258"), "Performs the Import action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00258"), false, false, "Group.Import", "Import Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00259"), "Performs the Print action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00259"), false, false, "Group.Print", "Print Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00260"), "Performs the Show action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00260"), false, false, "User.Show", "Show User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00261"), "Performs the Create action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00261"), false, false, "User.Create", "Create User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00262"), "Performs the Update action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00262"), false, false, "User.Update", "Update User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00263"), "Performs the Delete action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00263"), false, false, "User.Delete", "Delete User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00264"), "Performs the Export action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00264"), false, false, "User.Export", "Export User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00265"), "Performs the Import action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00265"), false, false, "User.Import", "Import User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00266"), "Performs the Print action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00266"), false, false, "User.Print", "Print User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-879791800018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-879791800018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00204"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00205"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00206"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00207"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00208"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00209"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00210"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00211"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00212"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00213"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00214"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00215"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00216"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00217"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00218"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00219"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00220"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00221"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00222"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00223"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00224"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00225"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00226"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00227"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00228"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00229"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00230"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00231"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00232"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00233"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00234"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00235"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00236"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00237"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00238"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00239"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00240"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00241"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00242"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00243"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00244"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00245"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00246"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00247"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00248"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00249"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00250"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00251"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00252"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00253"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00254"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00255"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00256"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00257"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00258"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00259"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00260"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00261"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00262"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00263"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00264"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00265"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00266"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-879791800018"));

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00099"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Sales entity", "Sales.Show", "Show Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00100"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Sales entity", "Sales.Create", "Create Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00101"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Sales entity", "Sales.Update", "Update Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00102"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Sales entity", "Sales.Delete", "Delete Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00103"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Sales entity", "Sales.Export", "Export Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00104"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Sales entity", "Sales.Import", "Import Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00105"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Sales entity", "Sales.Print", "Print Sales" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00106"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Safe entity", "Safe.Show", "Show Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00107"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Safe entity", "Safe.Create", "Create Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00108"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Safe entity", "Safe.Update", "Update Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00109"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Safe entity", "Safe.Delete", "Delete Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00110"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Safe entity", "Safe.Export", "Export Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00111"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Safe entity", "Safe.Import", "Import Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00112"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Safe entity", "Safe.Print", "Print Safe" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00113"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Bank entity", "Bank.Show", "Show Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00114"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Bank entity", "Bank.Create", "Create Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00115"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Bank entity", "Bank.Update", "Update Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00116"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Bank entity", "Bank.Delete", "Delete Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00117"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Bank entity", "Bank.Export", "Export Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00118"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Bank entity", "Bank.Import", "Import Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00119"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Bank entity", "Bank.Print", "Print Bank" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00120"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the BankBranch entity", "BankBranch.Show", "Show BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00121"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the BankBranch entity", "BankBranch.Create", "Create BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00122"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the BankBranch entity", "BankBranch.Update", "Update BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00123"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the BankBranch entity", "BankBranch.Delete", "Delete BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00124"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the BankBranch entity", "BankBranch.Export", "Export BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00125"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the BankBranch entity", "BankBranch.Import", "Import BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00126"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the BankBranch entity", "BankBranch.Print", "Print BankBranch" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00127"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Client entity", "Client.Show", "Show Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00128"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Client entity", "Client.Create", "Create Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00129"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Client entity", "Client.Update", "Update Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00130"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Client entity", "Client.Delete", "Delete Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00131"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Client entity", "Client.Export", "Export Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00132"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Client entity", "Client.Import", "Import Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00133"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Client entity", "Client.Print", "Print Client" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00134"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the SellReturnInvoice entity", "SellReturnInvoice.Show", "Show SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00135"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the SellReturnInvoice entity", "SellReturnInvoice.Create", "Create SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00136"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the SellReturnInvoice entity", "SellReturnInvoice.Update", "Update SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00137"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the SellReturnInvoice entity", "SellReturnInvoice.Delete", "Delete SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00138"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the SellReturnInvoice entity", "SellReturnInvoice.Export", "Export SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00139"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the SellReturnInvoice entity", "SellReturnInvoice.Import", "Import SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00140"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the SellReturnInvoice entity", "SellReturnInvoice.Print", "Print SellReturnInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00141"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the QuantityCrediting entity", "QuantityCrediting.Show", "Show QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00142"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the QuantityCrediting entity", "QuantityCrediting.Create", "Create QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00143"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the QuantityCrediting entity", "QuantityCrediting.Update", "Update QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00144"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the QuantityCrediting entity", "QuantityCrediting.Delete", "Delete QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00145"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the QuantityCrediting entity", "QuantityCrediting.Export", "Export QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00146"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the QuantityCrediting entity", "QuantityCrediting.Import", "Import QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00147"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the QuantityCrediting entity", "QuantityCrediting.Print", "Print QuantityCrediting" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00148"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Purchase entity", "Purchase.Show", "Show Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00149"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Purchase entity", "Purchase.Create", "Create Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00150"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Purchase entity", "Purchase.Update", "Update Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00151"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Purchase entity", "Purchase.Delete", "Delete Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00152"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Purchase entity", "Purchase.Export", "Export Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00153"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Purchase entity", "Purchase.Import", "Import Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00154"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Purchase entity", "Purchase.Print", "Print Purchase" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00155"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the FinancingOperations entity", "FinancingOperations.Show", "Show FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00156"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the FinancingOperations entity", "FinancingOperations.Create", "Create FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00157"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the FinancingOperations entity", "FinancingOperations.Update", "Update FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00158"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the FinancingOperations entity", "FinancingOperations.Delete", "Delete FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00159"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the FinancingOperations entity", "FinancingOperations.Export", "Export FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00160"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the FinancingOperations entity", "FinancingOperations.Import", "Import FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00161"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the FinancingOperations entity", "FinancingOperations.Print", "Print FinancingOperations" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00162"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Marketer entity", "Marketer.Show", "Show Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00163"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Marketer entity", "Marketer.Create", "Create Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00164"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Marketer entity", "Marketer.Update", "Update Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00165"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Marketer entity", "Marketer.Delete", "Delete Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00166"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Marketer entity", "Marketer.Export", "Export Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00167"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Marketer entity", "Marketer.Import", "Import Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00168"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Marketer entity", "Marketer.Print", "Print Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00169"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Show", "Show MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00170"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Create", "Create MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00171"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Update", "Update MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00172"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Delete", "Delete MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00173"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Export", "Export MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00174"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Import", "Import MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00175"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Print", "Print MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00176"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Financial entity", "Financial.Show", "Show Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00177"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Financial entity", "Financial.Create", "Create Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00178"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Financial entity", "Financial.Update", "Update Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00179"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Financial entity", "Financial.Delete", "Delete Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00180"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Financial entity", "Financial.Export", "Export Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00181"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Financial entity", "Financial.Import", "Import Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00182"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Financial entity", "Financial.Print", "Print Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00183"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Config entity", "Config.Show", "Show Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00184"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Config entity", "Config.Create", "Create Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00185"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Config entity", "Config.Update", "Update Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00186"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Config entity", "Config.Delete", "Delete Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00187"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Config entity", "Config.Export", "Export Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00188"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Config entity", "Config.Import", "Import Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00189"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Config entity", "Config.Print", "Print Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00190"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Group entity", "Group.Show", "Show Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00191"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Group entity", "Group.Create", "Create Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00192"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Group entity", "Group.Update", "Update Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00193"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Group entity", "Group.Delete", "Delete Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00194"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Group entity", "Group.Export", "Export Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00195"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Group entity", "Group.Import", "Import Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00196"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Group entity", "Group.Print", "Print Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00197"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the User entity", "User.Show", "Show User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00198"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the User entity", "User.Create", "Create User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00199"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the User entity", "User.Update", "Update User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00200"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the User entity", "User.Delete", "Delete User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00201"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the User entity", "User.Export", "Export User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00202"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the User entity", "User.Import", "Import User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00203"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the User entity", "User.Print", "Print User" });

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-467172700018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-467172700018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }
    }
}
