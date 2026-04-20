using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class costnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-234539200018"));

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-957217100018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-957217100018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100001"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100002"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100003"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100004"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100005"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100006"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100007"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100008"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100009"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100010"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100011"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100012"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100013"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100014"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100015"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100016"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100017"));

            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-957217100018"));

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-234539200018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-234539200018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });
        }
    }
}
