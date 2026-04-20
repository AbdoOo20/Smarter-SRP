using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeparateFinancingOpsRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00190"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the ManufacturingFacility entity", "ManufacturingFacility.Show", "Show ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00191"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the ManufacturingFacility entity", "ManufacturingFacility.Create", "Create ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00192"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the ManufacturingFacility entity", "ManufacturingFacility.Update", "Update ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00193"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the ManufacturingFacility entity", "ManufacturingFacility.Delete", "Delete ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00194"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the ManufacturingFacility entity", "ManufacturingFacility.Export", "Export ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00195"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the ManufacturingFacility entity", "ManufacturingFacility.Import", "Import ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00196"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the ManufacturingFacility entity", "ManufacturingFacility.Print", "Print ManufacturingFacility" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00197"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the FactoringInvoice entity", "FactoringInvoice.Show", "Show FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00198"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the FactoringInvoice entity", "FactoringInvoice.Create", "Create FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00199"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the FactoringInvoice entity", "FactoringInvoice.Update", "Update FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00200"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the FactoringInvoice entity", "FactoringInvoice.Delete", "Delete FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00201"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the FactoringInvoice entity", "FactoringInvoice.Export", "Export FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00202"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the FactoringInvoice entity", "FactoringInvoice.Import", "Import FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00203"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the FactoringInvoice entity", "FactoringInvoice.Print", "Print FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00204"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MaintenanceSector entity", "MaintenanceSector.Show", "Show MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00205"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MaintenanceSector entity", "MaintenanceSector.Create", "Create MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00206"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MaintenanceSector entity", "MaintenanceSector.Update", "Update MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00207"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MaintenanceSector entity", "MaintenanceSector.Delete", "Delete MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00208"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MaintenanceSector entity", "MaintenanceSector.Export", "Export MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00209"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MaintenanceSector entity", "MaintenanceSector.Import", "Import MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00210"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MaintenanceSector entity", "MaintenanceSector.Print", "Print MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00211"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MaintenanceInvoice entity", "MaintenanceInvoice.Show", "Show MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00212"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MaintenanceInvoice entity", "MaintenanceInvoice.Create", "Create MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00213"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MaintenanceInvoice entity", "MaintenanceInvoice.Update", "Update MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00214"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MaintenanceInvoice entity", "MaintenanceInvoice.Delete", "Delete MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00215"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MaintenanceInvoice entity", "MaintenanceInvoice.Export", "Export MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00216"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MaintenanceInvoice entity", "MaintenanceInvoice.Import", "Import MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00217"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MaintenanceInvoice entity", "MaintenanceInvoice.Print", "Print MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00218"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Marketer entity", "Marketer.Show", "Show Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00219"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Marketer entity", "Marketer.Create", "Create Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00220"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Marketer entity", "Marketer.Update", "Update Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00221"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Marketer entity", "Marketer.Delete", "Delete Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00222"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Marketer entity", "Marketer.Export", "Export Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00223"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Marketer entity", "Marketer.Import", "Import Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00224"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Marketer entity", "Marketer.Print", "Print Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00225"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Show", "Show MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00226"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Create", "Create MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00227"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Update", "Update MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00228"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Delete", "Delete MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00229"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Export", "Export MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00230"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Import", "Import MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00231"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Print", "Print MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00232"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Financial entity", "Financial.Show", "Show Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00233"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Financial entity", "Financial.Create", "Create Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00234"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Financial entity", "Financial.Update", "Update Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00235"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Financial entity", "Financial.Delete", "Delete Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00236"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Financial entity", "Financial.Export", "Export Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00237"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Financial entity", "Financial.Import", "Import Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00238"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Financial entity", "Financial.Print", "Print Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00239"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Config entity", "Config.Show", "Show Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00240"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Config entity", "Config.Create", "Create Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00241"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Config entity", "Config.Update", "Update Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00242"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Config entity", "Config.Delete", "Delete Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00243"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Config entity", "Config.Export", "Export Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00244"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Config entity", "Config.Import", "Import Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00245"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Config entity", "Config.Print", "Print Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00246"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Group entity", "Group.Show", "Show Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00247"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Group entity", "Group.Create", "Create Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00248"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Group entity", "Group.Update", "Update Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00249"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Group entity", "Group.Delete", "Delete Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00250"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Group entity", "Group.Export", "Export Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00251"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Group entity", "Group.Import", "Import Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00252"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Group entity", "Group.Print", "Print Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00253"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the User entity", "User.Show", "Show User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00254"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the User entity", "User.Create", "Create User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00255"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the User entity", "User.Update", "Update User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00256"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the User entity", "User.Delete", "Delete User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00257"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the User entity", "User.Export", "Export User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00258"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the User entity", "User.Import", "Import User" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00259"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the User entity", "User.Print", "Print User" });

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500012"), "Performs the Show action on the MaintenanceInvoicesByStatus Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500012"), false, false, "StatementsNavigator.MaintenanceInvoicesByStatus-Show", "MaintenanceInvoicesByStatus", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500013"), "Performs the Show action on the FactoringInvoicesByStatus Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500013"), false, false, "StatementsNavigator.FactoringInvoicesByStatus-Show", "FactoringInvoicesByStatus", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500014"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500014"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500015"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500015"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500016"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500016"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500017"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500017"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500018"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500018"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500019"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500019"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957516500020"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-957516500020"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-957486300001"), "Performs the Create action on the AccountPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-957486300001"), false, false, "FinancialOperation.AccountPaymentReceipt-Create", "AccountPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-957486300002"), "Performs the Create action on the DiscountItemPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-957486300002"), false, false, "FinancialOperation.DiscountItemPaymentReceipt-Create", "DiscountItemPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-957486300003"), "Performs the Create action on the ExpensePaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-957486300003"), false, false, "FinancialOperation.ExpensePaymentReceipt-Create", "ExpensePaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-957486300004"), "Performs the Create action on the ClientPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-957486300004"), false, false, "FinancialOperation.ClientPaymentReceipt-Create", "ClientPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-957486300005"), "Performs the Create action on the EmployeePaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-957486300005"), false, false, "FinancialOperation.EmployeePaymentReceipt-Create", "EmployeePaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-957486300006"), "Performs the Create action on the SupplierPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-957486300006"), false, false, "FinancialOperation.SupplierPaymentReceipt-Create", "SupplierPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500018"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500019"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957516500020"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-957486300001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-957486300002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-957486300003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-957486300004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-957486300005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-957486300006"));

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

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00204"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the FactoringInvoice entity", "FactoringInvoice.Show", "Show FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00205"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the FactoringInvoice entity", "FactoringInvoice.Create", "Create FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00206"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the FactoringInvoice entity", "FactoringInvoice.Update", "Update FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00207"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the FactoringInvoice entity", "FactoringInvoice.Delete", "Delete FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00208"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the FactoringInvoice entity", "FactoringInvoice.Export", "Export FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00209"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the FactoringInvoice entity", "FactoringInvoice.Import", "Import FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00210"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the FactoringInvoice entity", "FactoringInvoice.Print", "Print FactoringInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00211"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MaintenanceSector entity", "MaintenanceSector.Show", "Show MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00212"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MaintenanceSector entity", "MaintenanceSector.Create", "Create MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00213"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MaintenanceSector entity", "MaintenanceSector.Update", "Update MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00214"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MaintenanceSector entity", "MaintenanceSector.Delete", "Delete MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00215"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MaintenanceSector entity", "MaintenanceSector.Export", "Export MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00216"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MaintenanceSector entity", "MaintenanceSector.Import", "Import MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00217"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MaintenanceSector entity", "MaintenanceSector.Print", "Print MaintenanceSector" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00218"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MaintenanceInvoice entity", "MaintenanceInvoice.Show", "Show MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00219"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MaintenanceInvoice entity", "MaintenanceInvoice.Create", "Create MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00220"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MaintenanceInvoice entity", "MaintenanceInvoice.Update", "Update MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00221"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MaintenanceInvoice entity", "MaintenanceInvoice.Delete", "Delete MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00222"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MaintenanceInvoice entity", "MaintenanceInvoice.Export", "Export MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00223"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MaintenanceInvoice entity", "MaintenanceInvoice.Import", "Import MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00224"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MaintenanceInvoice entity", "MaintenanceInvoice.Print", "Print MaintenanceInvoice" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00225"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Marketer entity", "Marketer.Show", "Show Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00226"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Marketer entity", "Marketer.Create", "Create Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00227"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Marketer entity", "Marketer.Update", "Update Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00228"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Marketer entity", "Marketer.Delete", "Delete Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00229"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Marketer entity", "Marketer.Export", "Export Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00230"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Marketer entity", "Marketer.Import", "Import Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00231"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Marketer entity", "Marketer.Print", "Print Marketer" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00232"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Show", "Show MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00233"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Create", "Create MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00234"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Update", "Update MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00235"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Delete", "Delete MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00236"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Export", "Export MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00237"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Import", "Import MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00238"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the MarketerDisbursementPermit entity", "MarketerDisbursementPermit.Print", "Print MarketerDisbursementPermit" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00239"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Financial entity", "Financial.Show", "Show Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00240"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Financial entity", "Financial.Create", "Create Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00241"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Financial entity", "Financial.Update", "Update Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00242"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Financial entity", "Financial.Delete", "Delete Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00243"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Financial entity", "Financial.Export", "Export Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00244"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Financial entity", "Financial.Import", "Import Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00245"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Financial entity", "Financial.Print", "Print Financial" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00246"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Config entity", "Config.Show", "Show Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00247"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Config entity", "Config.Create", "Create Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00248"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Config entity", "Config.Update", "Update Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00249"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Config entity", "Config.Delete", "Delete Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00250"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Config entity", "Config.Export", "Export Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00251"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Config entity", "Config.Import", "Import Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00252"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Config entity", "Config.Print", "Print Config" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00253"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Show action on the Group entity", "Group.Show", "Show Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00254"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Create action on the Group entity", "Group.Create", "Create Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00255"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Update action on the Group entity", "Group.Update", "Update Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00256"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Delete action on the Group entity", "Group.Delete", "Delete Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00257"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Export action on the Group entity", "Group.Export", "Export Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00258"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Import action on the Group entity", "Group.Import", "Import Group" });

            migrationBuilder.UpdateData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00259"),
                columns: new[] { "Description", "Key", "Name" },
                values: new object[] { "Performs the Print action on the Group entity", "Group.Print", "Print Group" });

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
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
    }
}
