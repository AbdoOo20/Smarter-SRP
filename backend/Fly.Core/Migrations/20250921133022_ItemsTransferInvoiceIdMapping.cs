using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class ItemsTransferInvoiceIdMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TItemsTransferInvoiceDetail_TItemsTransferInvoice_TItemsTran~",
                table: "TItemsTransferInvoiceDetail");

            migrationBuilder.DropIndex(
                name: "IX_TItemsTransferInvoiceDetail_TItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail");

            migrationBuilder.DropColumn(
                name: "TItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoiceDetail_ItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail",
                column: "ItemsTransferInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TItemsTransferInvoiceDetail_TItemsTransferInvoice_ItemsTrans~",
                table: "TItemsTransferInvoiceDetail",
                column: "ItemsTransferInvoiceId",
                principalTable: "TItemsTransferInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TItemsTransferInvoiceDetail_TItemsTransferInvoice_ItemsTrans~",
                table: "TItemsTransferInvoiceDetail");

            migrationBuilder.DropIndex(
                name: "IX_TItemsTransferInvoiceDetail_ItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail");

            migrationBuilder.DropColumn(
                name: "ItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail");

            migrationBuilder.AddColumn<Guid>(
                name: "TItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoiceDetail_TItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail",
                column: "TItemsTransferInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TItemsTransferInvoiceDetail_TItemsTransferInvoice_TItemsTran~",
                table: "TItemsTransferInvoiceDetail",
                column: "TItemsTransferInvoiceId",
                principalTable: "TItemsTransferInvoice",
                principalColumn: "Id");
        }
    }
}
