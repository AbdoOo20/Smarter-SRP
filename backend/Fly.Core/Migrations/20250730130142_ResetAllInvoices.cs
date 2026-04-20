using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class ResetAllInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TFactoringInvoiceComposition_TFactoringInvoice_TFactoringInv~",
                table: "TFactoringInvoiceComposition");

            migrationBuilder.DropForeignKey(
                name: "FK_TFactoringInvoiceCost_TFactoringInvoice_TFactoringInvoiceId",
                table: "TFactoringInvoiceCost");

            migrationBuilder.DropForeignKey(
                name: "FK_TFactoringInvoiceDetail_TFactoringInvoice_TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TMaintenanceInvoiceCost_TMaintenanceInvoice_TMaintenanceInvo~",
                table: "TMaintenanceInvoiceCost");

            migrationBuilder.DropForeignKey(
                name: "FK_TMaintenanceInvoiceDetail_TMaintenanceInvoice_TMaintenanceIn~",
                table: "TMaintenanceInvoiceDetail");

            migrationBuilder.DropIndex(
                name: "IX_TMaintenanceInvoiceDetail_TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceDetail");

            migrationBuilder.DropIndex(
                name: "IX_TMaintenanceInvoiceCost_TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceCost");

            migrationBuilder.DropIndex(
                name: "IX_TFactoringInvoiceDetail_TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail");

            migrationBuilder.DropIndex(
                name: "IX_TFactoringInvoiceCost_TFactoringInvoiceId",
                table: "TFactoringInvoiceCost");

            migrationBuilder.DropIndex(
                name: "IX_TFactoringInvoiceComposition_TFactoringInvoiceId",
                table: "TFactoringInvoiceComposition");

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-931507700018"));

            migrationBuilder.DropColumn(
                name: "TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceDetail");

            migrationBuilder.DropColumn(
                name: "TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceCost");

            migrationBuilder.DropColumn(
                name: "TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail");

            migrationBuilder.DropColumn(
                name: "TFactoringInvoiceId",
                table: "TFactoringInvoiceCost");

            migrationBuilder.DropColumn(
                name: "TFactoringInvoiceId",
                table: "TFactoringInvoiceComposition");

            migrationBuilder.AlterColumn<Guid>(
                name: "FactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "FactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "FactoringInvoiceId",
                table: "TFactoringInvoiceComposition",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-602828800018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-602828800018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-602828800018"));

            migrationBuilder.AddColumn<Guid>(
                name: "TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceDetail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceCost",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "FactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "FactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TFactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "FactoringInvoiceId",
                table: "TFactoringInvoiceComposition",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TFactoringInvoiceId",
                table: "TFactoringInvoiceComposition",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-931507700018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-931507700018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceDetail_TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceDetail",
                column: "TMaintenanceInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceCost_TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceCost",
                column: "TMaintenanceInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceDetail_TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                column: "TFactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceCost_TFactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                column: "TFactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceComposition_TFactoringInvoiceId",
                table: "TFactoringInvoiceComposition",
                column: "TFactoringInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TFactoringInvoiceComposition_TFactoringInvoice_TFactoringInv~",
                table: "TFactoringInvoiceComposition",
                column: "TFactoringInvoiceId",
                principalTable: "TFactoringInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TFactoringInvoiceCost_TFactoringInvoice_TFactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                column: "TFactoringInvoiceId",
                principalTable: "TFactoringInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TFactoringInvoiceDetail_TFactoringInvoice_TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                column: "TFactoringInvoiceId",
                principalTable: "TFactoringInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TMaintenanceInvoiceCost_TMaintenanceInvoice_TMaintenanceInvo~",
                table: "TMaintenanceInvoiceCost",
                column: "TMaintenanceInvoiceId",
                principalTable: "TMaintenanceInvoice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TMaintenanceInvoiceDetail_TMaintenanceInvoice_TMaintenanceIn~",
                table: "TMaintenanceInvoiceDetail",
                column: "TMaintenanceInvoiceId",
                principalTable: "TMaintenanceInvoice",
                principalColumn: "Id");
        }
    }
}
