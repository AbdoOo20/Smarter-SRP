using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class ClientRepresentativeReceiptRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500012"), "Performs the Show action on the MaintenanceInvoicesByStatus Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500012"), false, false, "StatementsNavigator.MaintenanceInvoicesByStatus-Show", "MaintenanceInvoicesByStatus", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500013"), "Performs the Show action on the FactoringInvoicesByStatus Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500013"), false, false, "StatementsNavigator.FactoringInvoicesByStatus-Show", "FactoringInvoicesByStatus", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500014"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500014"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500015"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500015"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500016"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500016"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500017"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500017"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500018"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500018"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500019"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500019"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-858868500020"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-858868500020"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700001"), "Performs the Create action on the AccountPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700001"), false, false, "FinancialOperation.AccountPaymentReceipt-Create", "AccountPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700002"), "Performs the Create action on the DiscountItemPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700002"), false, false, "FinancialOperation.DiscountItemPaymentReceipt-Create", "DiscountItemPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700003"), "Performs the Create action on the ExpensePaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700003"), false, false, "FinancialOperation.ExpensePaymentReceipt-Create", "ExpensePaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700004"), "Performs the Create action on the ClientPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700004"), false, false, "FinancialOperation.ClientPaymentReceipt-Create", "ClientPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700005"), "Performs the Create action on the EmployeePaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700005"), false, false, "FinancialOperation.EmployeePaymentReceipt-Create", "EmployeePaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700006"), "Performs the Create action on the SupplierPaymentReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700006"), false, false, "FinancialOperation.SupplierPaymentReceipt-Create", "SupplierPaymentReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e7-858845700007"), "Performs the Create action on the ClientRepresentativeReceipt Report", new Guid("08b4dae1-3eef-4509-b1e7-858845700007"), false, false, "FinancialOperation.ClientRepresentativeReceipt-Create", "ClientRepresentativeReceipt", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500018"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500019"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-858868500020"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e7-858845700007"));

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
    }
}
