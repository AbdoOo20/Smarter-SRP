using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositionIndexToFactoringInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompositionIndex",
                table: "TFactoringInvoiceComposition",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompositionIndex",
                table: "TFactoringInvoiceComposition");
        }
    }
}
