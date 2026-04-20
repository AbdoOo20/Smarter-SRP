using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class ItemsTransferInvoiceInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TItemsTransferInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InvoiceNumber = table.Column<long>(type: "bigint", nullable: false),
                    FromStorageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ToStorageId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ToStorageName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransferStatus = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItemsTransferInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TItemsTransferInvoice_TStore_FromStorageId",
                        column: x => x.FromStorageId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TItemsTransferInvoice_TStore_ToStorageId",
                        column: x => x.ToStorageId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TItemsTransferInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TItemsTransferInvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItemsTransferInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TItemsTransferInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TItemsTransferInvoiceDetail_TItemsTransferInvoice_TItemsTran~",
                        column: x => x.TItemsTransferInvoiceId,
                        principalTable: "TItemsTransferInvoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TItemsTransferInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00260"), "Performs the Show action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00260"), false, false, "ItemsTransferInvoice.Show", "Show ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00261"), "Performs the Create action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00261"), false, false, "ItemsTransferInvoice.Create", "Create ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00262"), "Performs the Update action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00262"), false, false, "ItemsTransferInvoice.Update", "Update ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00263"), "Performs the Delete action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00263"), false, false, "ItemsTransferInvoice.Delete", "Delete ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00264"), "Performs the Export action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00264"), false, false, "ItemsTransferInvoice.Export", "Export ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00265"), "Performs the Import action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00265"), false, false, "ItemsTransferInvoice.Import", "Import ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00266"), "Performs the Print action on the ItemsTransferInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00266"), false, false, "ItemsTransferInvoice.Print", "Print ItemsTransferInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoice_FromStorageId",
                table: "TItemsTransferInvoice",
                column: "FromStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoice_ToStorageId",
                table: "TItemsTransferInvoice",
                column: "ToStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoiceDetail_ItemId",
                table: "TItemsTransferInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoiceDetail_TItemsTransferInvoiceId",
                table: "TItemsTransferInvoiceDetail",
                column: "TItemsTransferInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemsTransferInvoiceDetail_UnitId",
                table: "TItemsTransferInvoiceDetail",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TItemsTransferInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TItemsTransferInvoice");

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
        }
    }
}
