using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultBankToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultBankId",
                table: "TUser",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VisibleBanks",
                table: "TUser",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb001"),
                columns: new[] { "DefaultBankId", "VisibleBanks" },
                values: new object[] { null, "[]" });

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb002"),
                columns: new[] { "DefaultBankId", "VisibleBanks" },
                values: new object[] { null, "[]" });

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb003"),
                columns: new[] { "DefaultBankId", "VisibleBanks" },
                values: new object[] { null, "[]" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultBankId",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "VisibleBanks",
                table: "TUser");
        }
    }
}
