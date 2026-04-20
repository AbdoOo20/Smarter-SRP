using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class CategoryIdEntitySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TCategory",
                columns: new[] { "Id", "CategoryId", "Description", "Guid", "IsDeleted", "Name", "TenantId" },
                values: new object[] { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4ccb016"), null, "الفئة الأساسية التي تندرج عنها باقي الفئات.", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4ffb016"), false, "الفئة الأساسية", new Guid("00000000-0000-0000-0000-000000000000") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TCategory",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4ccb016"));
        }
    }
}
