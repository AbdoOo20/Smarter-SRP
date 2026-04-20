using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConfigForInternalMaintenanceAndFactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TConfig",
                keyColumn: "Id",
                keyValue: new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da1"),
                columns: new[] { "InternalMaintenanceSectorsAccount", "InternalManufacturingFacilitiesAccount" },
                values: new object[] { "0e54960c-8a5b-473d-8022-c4a47c651107", "0e54960c-8a5b-473d-8022-c4a47c651108" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TConfig",
                keyColumn: "Id",
                keyValue: new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da1"),
                columns: new[] { "InternalMaintenanceSectorsAccount", "InternalManufacturingFacilitiesAccount" },
                values: new object[] { "dbcb6851-213c-4238-a2a9-46f0d359c878", "dbcb6851-213c-4238-a2a9-46f0d359c878" });
        }
    }
}
