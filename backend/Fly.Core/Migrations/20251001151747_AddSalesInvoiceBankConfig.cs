using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesInvoiceBankConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "TSalesInvoice",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb001"),
                column: "VisibleBanks",
                value: "");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb002"),
                column: "VisibleBanks",
                value: "");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb003"),
                column: "VisibleBanks",
                value: "");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoice_BankId",
                table: "TSalesInvoice",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_TSalesInvoice_TBank_BankId",
                table: "TSalesInvoice",
                column: "BankId",
                principalTable: "TBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TSalesInvoice_TBank_BankId",
                table: "TSalesInvoice");

            migrationBuilder.DropIndex(
                name: "IX_TSalesInvoice_BankId",
                table: "TSalesInvoice");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "TSalesInvoice");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb001"),
                column: "VisibleBanks",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb002"),
                column: "VisibleBanks",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb003"),
                column: "VisibleBanks",
                value: "[]");
        }
    }
}
