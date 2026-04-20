using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDamageItemsPerm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[] { new Guid("08b4dae1-3eef-4509-b1e6-223321200023"), "Performs the Show action on the DamageItems Report", new Guid("08b4dae1-3eef-4509-b1e6-223321200023"), false, false, "StatementsNavigator.DamageItems-Show", "DamageItems", null, new Guid("00000000-0000-0000-0000-000000000000"), "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRole",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-3eef-4509-b1e6-223321200023"));
        }
    }
}
