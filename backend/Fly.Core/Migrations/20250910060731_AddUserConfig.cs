using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddUserConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultPaymentType",
                table: "TUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultSafeId",
                table: "TUser",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DefaultStorageId",
                table: "TUser",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DefaultStorageType",
                table: "TUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisibleSafes",
                table: "TUser",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VisibleStorages",
                table: "TUser",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb001"),
                columns: new[] { "DefaultPaymentType", "DefaultSafeId", "DefaultStorageId", "DefaultStorageType", "VisibleSafes", "VisibleStorages" },
                values: new object[] { 0, null, null, 0, "", "" });

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb002"),
                columns: new[] { "DefaultPaymentType", "DefaultSafeId", "DefaultStorageId", "DefaultStorageType", "VisibleSafes", "VisibleStorages" },
                values: new object[] { 0, null, null, 0, "", "" });

            migrationBuilder.UpdateData(
                table: "TUser",
                keyColumn: "Id",
                keyValue: new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb003"),
                columns: new[] { "DefaultPaymentType", "DefaultSafeId", "DefaultStorageId", "DefaultStorageType", "VisibleSafes", "VisibleStorages" },
                values: new object[] { 0, null, null, 0, "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPaymentType",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "DefaultSafeId",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "DefaultStorageId",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "DefaultStorageType",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "VisibleSafes",
                table: "TUser");

            migrationBuilder.DropColumn(
                name: "VisibleStorages",
                table: "TUser");
        }
    }
}
