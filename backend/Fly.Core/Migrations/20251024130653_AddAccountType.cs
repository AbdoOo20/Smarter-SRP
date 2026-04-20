using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatementAccountType",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651001"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651002"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651102"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651103"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651105"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651106"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651107"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c651108"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c4a47c65185f"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c8847c651103"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("0e54960c-8a5b-473d-8022-c8847c651104"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("1036cd62-716a-4b8c-a19f-40ee2584484f"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("13440fba-02cd-4f4b-843b-6159611c3fac"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("18847d06-8ff6-4e55-9b63-8318edbf6197"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("1c63e7c0-033d-435b-b55f-4a37cb6ab77c"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("23b638f5-71d4-4256-b99b-5b84b19c91a7"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("2b5ee402-49f1-444c-9a86-bf76e0ecbf90"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("2b5ee402-49f1-444c-9a86-bf76e0ecbf91"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("56844955-106c-4f9f-8f07-b8e33b3c2812"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("83a0c584-5013-4186-a337-2985d38b6544"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("9002ea94-02ff-47f0-907c-00a8360c440b"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("a65d59a6-4291-4bfe-b718-9c676a5b8578"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("ad630d24-f791-4d5a-8104-27595e273112"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da1"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da2"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6dc2"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6dc3"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"),
                column: "StatementAccountType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("e87b3121-fb01-489a-9081-b50f9e2e3a91"),
                column: "StatementAccountType",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatementAccountType",
                table: "Accounts");
        }
    }
}
