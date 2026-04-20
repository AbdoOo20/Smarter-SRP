using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFactoringDetailSourceItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SourceCompositionId",
                table: "TFactoringInvoiceDetail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "SourceCompositionIndex",
                table: "TFactoringInvoiceDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SourceCompositionName",
                table: "TFactoringInvoiceDetail",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceCompositionId",
                table: "TFactoringInvoiceDetail");

            migrationBuilder.DropColumn(
                name: "SourceCompositionIndex",
                table: "TFactoringInvoiceDetail");

            migrationBuilder.DropColumn(
                name: "SourceCompositionName",
                table: "TFactoringInvoiceDetail");
        }
    }
}
