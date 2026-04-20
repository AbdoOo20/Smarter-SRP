using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddReceivedPaymentInAdvance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReceivedFeeInAdvance",
                table: "TMaintenanceInvoice",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReceivedFeeInAdvance",
                table: "TFactoringInvoice",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedFeeInAdvance",
                table: "TMaintenanceInvoice");

            migrationBuilder.DropColumn(
                name: "ReceivedFeeInAdvance",
                table: "TFactoringInvoice");
        }
    }
}
