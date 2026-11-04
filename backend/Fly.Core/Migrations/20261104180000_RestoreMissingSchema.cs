using Fly.Core.Persistence.Ef.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fly.Core.Migrations
{
    /// <summary>
    /// Creates the TItemBarcode table and the TItemUnit.Barcode column. Both are in the
    /// model snapshot but no migration ever created them, so a database built from the
    /// migrations failed on the items screen with "Table 'titembarcode' doesn't exist".
    ///
    /// Each step only runs when the table or column is missing, so it is safe on
    /// existing customer databases that already have them. The snapshot already
    /// contains both, so no snapshot change is needed.
    /// </summary>
    [DbContext(typeof(DbApplicationContext))]
    [Migration("20261104180000_RestoreMissingSchema")]
    public partial class RestoreMissingSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS `TItemBarcode` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemTypeName` longtext CHARACTER SET utf8mb4 NULL,
    `Barcode` longtext CHARACTER SET utf8mb4 NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TItemBarcode` PRIMARY KEY (`Id`),
    KEY `IX_TItemBarcode_ItemId` (`ItemId`),
    CONSTRAINT `FK_TItemBarcode_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;");

            // TItemUnit.Barcode is also in the model with no migration. MySQL has no
            // "ADD COLUMN IF NOT EXISTS", so add it only when information_schema says
            // it is missing.
            migrationBuilder.Sql(@"
SET @stmt = (
    SELECT IF(COUNT(*) = 0,
        'ALTER TABLE `TItemUnit` ADD `Barcode` longtext CHARACTER SET utf8mb4 NULL',
        'DO 0')
    FROM information_schema.columns
    WHERE table_schema = DATABASE()
      AND LOWER(table_name) = 'titemunit'
      AND LOWER(column_name) = 'barcode'
);
PREPARE restore_missing_schema FROM @stmt;
EXECUTE restore_missing_schema;
DEALLOCATE PREPARE restore_missing_schema;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty: on existing databases this migration may have
            // changed nothing, so dropping the table here could delete customer data.
        }
    }
}
