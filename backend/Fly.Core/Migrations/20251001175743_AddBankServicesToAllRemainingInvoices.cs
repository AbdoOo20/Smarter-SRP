using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddBankServicesToAllRemainingInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "TPurchaseInvoice",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "TMaintenanceInvoice",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "TFactoringInvoice",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoice_BankId",
                table: "TPurchaseInvoice",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoice_BankId",
                table: "TMaintenanceInvoice",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoice_BankId",
                table: "TFactoringInvoice",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_TFactoringInvoice_TBank_BankId",
                table: "TFactoringInvoice",
                column: "BankId",
                principalTable: "TBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TMaintenanceInvoice_TBank_BankId",
                table: "TMaintenanceInvoice",
                column: "BankId",
                principalTable: "TBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TPurchaseInvoice_TBank_BankId",
                table: "TPurchaseInvoice",
                column: "BankId",
                principalTable: "TBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TFactoringInvoice_TBank_BankId",
                table: "TFactoringInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_TMaintenanceInvoice_TBank_BankId",
                table: "TMaintenanceInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_TPurchaseInvoice_TBank_BankId",
                table: "TPurchaseInvoice");

            migrationBuilder.DropIndex(
                name: "IX_TPurchaseInvoice_BankId",
                table: "TPurchaseInvoice");

            migrationBuilder.DropIndex(
                name: "IX_TMaintenanceInvoice_BankId",
                table: "TMaintenanceInvoice");

            migrationBuilder.DropIndex(
                name: "IX_TFactoringInvoice_BankId",
                table: "TFactoringInvoice");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "TPurchaseInvoice");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "TMaintenanceInvoice");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "TFactoringInvoice");
        }
    }
}
