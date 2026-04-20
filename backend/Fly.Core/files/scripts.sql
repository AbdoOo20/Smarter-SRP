CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Accounts` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Number` bigint NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `OpeningDebtor` decimal(12,2) NOT NULL,
    `OpeningCreditor` decimal(12,2) NOT NULL,
    `ParentId` char(36) COLLATE ascii_general_ci NULL,
    `CurrencyId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Natural` int NOT NULL,
    `AccountCat` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_Accounts` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Accounts_Accounts_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TCategory` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CategoryId` char(36) COLLATE ascii_general_ci NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TCategory` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TCategory_TCategory_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `TCategory` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TConfig` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `DiscountAccount` longtext CHARACTER SET utf8mb4 NULL,
    `GeneralShareAccount` longtext CHARACTER SET utf8mb4 NULL,
    `MarketerInvoicesAccountId` longtext CHARACTER SET utf8mb4 NULL,
    `SalesInvoicesAccountId` longtext CHARACTER SET utf8mb4 NULL,
    `GeneralSupplierAccountAccount` longtext CHARACTER SET utf8mb4 NULL,
    `GeneralClientAccountAccount` longtext CHARACTER SET utf8mb4 NULL,
    `PurchaseInvoicesAccount` longtext CHARACTER SET utf8mb4 NULL,
    `BankAccount` longtext CHARACTER SET utf8mb4 NULL,
    `SafeAccount` longtext CHARACTER SET utf8mb4 NULL,
    `ClientsAccount` longtext CHARACTER SET utf8mb4 NOT NULL,
    `SupplyersAccount` longtext CHARACTER SET utf8mb4 NOT NULL,
    `EmployeesAccount` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ExpenseAccount` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AssetsAccount` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TConfig` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TCostCenter` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TCostCenter` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TCurrency` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `IsPrimary` tinyint(1) NOT NULL,
    `TransfereRate` decimal(12,2) NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Letter` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TCurrency` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TDepartment` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDepartment` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TEntry` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Number` bigint NOT NULL,
    `Date` datetime(6) NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `OprationType` int NULL,
    `RelatedIndex` int NULL,
    `RelatedId` char(36) COLLATE ascii_general_ci NULL,
    `DisplayNumber` bigint NOT NULL,
    `SerialNumber` bigint NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TEntry` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TExpenseTypes` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TExpenseTypes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TRoleGroup` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TRoleGroup` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TStore` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Address` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TStore` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TUnit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TUnit` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TAssets` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `DepreciationRate` decimal(12,2) NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TAssets` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TAssets_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `TBank` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TBank` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TBank_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `TClient` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Phone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Identifier` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TClient` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TClient_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketer` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Percentage` decimal(12,2) NOT NULL,
    `IsRepresentative` tinyint(1) NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Phone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Identifier` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `OpeningDebtor` decimal(12,2) NOT NULL,
    `OpeningCreditor` decimal(12,2) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketer` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketer_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSafe` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `PeymentMode` int NOT NULL,
    `LossPercent` decimal(12,2) NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSafe` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSafe_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSupplier` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `Phone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Identifier` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSupplier` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSupplier_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TItem` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `CategoryId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Photo` longtext CHARACTER SET utf8mb4 NULL,
    `Barcode` longtext CHARACTER SET utf8mb4 NULL,
    `ExpireDate` datetime(6) NULL,
    `Price` decimal(12,2) NOT NULL,
    `Cost` decimal(12,2) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TItem` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TItem_TCategory_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `TCategory` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TEmployee` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `Phone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Identifier` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `DepartmentId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TEmployee` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TEmployee_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TEmployee_TDepartment_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `TDepartment` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TEntryDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `EntryId` char(36) COLLATE ascii_general_ci NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Debit` decimal(12,2) NOT NULL,
    `Credit` decimal(12,2) NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TEntryDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TEntryDetail_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TEntryDetail_TEntry_EntryId` FOREIGN KEY (`EntryId`) REFERENCES `TEntry` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TUser` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Phone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Accesser` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Sult` longtext CHARACTER SET utf8mb4 NOT NULL,
    `RegesterTime` datetime(6) NOT NULL,
    `IsAdmin` tinyint(1) NOT NULL,
    `IsGhost` tinyint(1) NOT NULL,
    `IsRedy` tinyint(1) NOT NULL,
    `IsActive` tinyint(1) NOT NULL,
    `RoleGroupId` char(36) COLLATE ascii_general_ci NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TUser` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TUser_TRoleGroup_RoleGroupId` FOREIGN KEY (`RoleGroupId`) REFERENCES `TRoleGroup` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TDisbursementPermit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `CostCenterId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NULL,
    `Number` bigint NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDisbursementPermit` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TDisbursementPermit_TCostCenter_CostCenterId` FOREIGN KEY (`CostCenterId`) REFERENCES `TCostCenter` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDisbursementPermit_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TBankBranch` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `BankId` char(36) COLLATE ascii_general_ci NOT NULL,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TBankBranch` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TBankBranch_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TBankBranch_TBank_BankId` FOREIGN KEY (`BankId`) REFERENCES `TBank` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerClient` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `MarketerId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ClientId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerClient` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerClient_TClient_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `TClient` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerClient_TMarketer_MarketerId` FOREIGN KEY (`MarketerId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerDisbursementPermit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `TotalCost` decimal(12,2) NOT NULL,
    `Number` bigint NOT NULL,
    `MarketerId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerDisbursementPermit` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerDisbursementPermit_TMarketer_MarketerId` FOREIGN KEY (`MarketerId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerDisbursementPermit_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerSalesInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `TotalAmount` decimal(12,2) NOT NULL,
    `Number` bigint NOT NULL,
    `IsPaid` tinyint(1) NOT NULL,
    `ClientId` char(36) COLLATE ascii_general_ci NOT NULL,
    `MarketerId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Discount` decimal(12,2) NOT NULL,
    `DiscountType` int NOT NULL,
    `PeymentType` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerSalesInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerSalesInvoice_TClient_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `TClient` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerSalesInvoice_TMarketer_MarketerId` FOREIGN KEY (`MarketerId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerSharedEntry` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `MarketerId` char(36) COLLATE ascii_general_ci NOT NULL,
    `EntryId` char(36) COLLATE ascii_general_ci NOT NULL,
    `SharedAmount` decimal(12,2) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerSharedEntry` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerSharedEntry_TEntry_EntryId` FOREIGN KEY (`EntryId`) REFERENCES `TEntry` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerSharedEntry_TMarketer_MarketerId` FOREIGN KEY (`MarketerId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSalesInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `TotalAmount` decimal(12,2) NOT NULL,
    `SerialNumber` bigint NOT NULL,
    `Number` bigint NOT NULL,
    `IsPaid` tinyint(1) NOT NULL,
    `RepresentativeId` char(36) COLLATE ascii_general_ci NULL,
    `RepresentativePercentage` decimal(12,2) NOT NULL,
    `ClientId` char(36) COLLATE ascii_general_ci NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NULL,
    `MarketerStorageId` char(36) COLLATE ascii_general_ci NULL,
    `StorageType` int NOT NULL,
    `SafeId` char(36) COLLATE ascii_general_ci NULL,
    `Discount` decimal(12,2) NOT NULL,
    `DiscountType` int NOT NULL,
    `PeymentType` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSalesInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSalesInvoice_TClient_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `TClient` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSalesInvoice_TMarketer_MarketerStorageId` FOREIGN KEY (`MarketerStorageId`) REFERENCES `TMarketer` (`Id`),
    CONSTRAINT `FK_TSalesInvoice_TMarketer_RepresentativeId` FOREIGN KEY (`RepresentativeId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSalesInvoice_TSafe_SafeId` FOREIGN KEY (`SafeId`) REFERENCES `TSafe` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSalesInvoice_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TPurchaseInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Number` bigint NOT NULL,
    `IsPaid` tinyint(1) NOT NULL,
    `TotalCost` decimal(12,2) NOT NULL,
    `TotalAmount` decimal(12,2) NOT NULL,
    `Date` datetime(6) NOT NULL,
    `Discount` decimal(12,2) NOT NULL,
    `DiscountType` int NOT NULL,
    `SupplierId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NOT NULL,
    `SafeId` char(36) COLLATE ascii_general_ci NULL,
    `PeymentType` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TPurchaseInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TPurchaseInvoice_TSafe_SafeId` FOREIGN KEY (`SafeId`) REFERENCES `TSafe` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseInvoice_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseInvoice_TSupplier_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `TSupplier` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSupplyPermit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `SupplierId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `Number` bigint NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSupplyPermit` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSupplyPermit_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSupplyPermit_TSupplier_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `TSupplier` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TDiscountitedItem` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `ExpireDate` datetime(6) NOT NULL,
    `Number` bigint NOT NULL,
    `Ammount` decimal(12,2) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `TotalQuantity` decimal(12,2) NOT NULL,
    `ClientId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ReleatedId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDiscountitedItem` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TDiscountitedItem_TClient_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `TClient` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDiscountitedItem_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDiscountitedItem_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `TInventory` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Size` int NULL,
    `Color` int NULL,
    `ExpireDate` datetime(6) NULL,
    `Ingress` tinyint(1) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `TotalQuantity` decimal(12,2) NOT NULL,
    `RelatedId` char(36) COLLATE ascii_general_ci NULL,
    `RelatedIndex` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TInventory` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TInventory_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TInventory_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TInventory_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TItemUnit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `IsPrimary` tinyint(1) NOT NULL,
    `TransferRate` decimal(12,2) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TItemUnit` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TItemUnit_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TItemUnit_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerInventory` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `MarketerId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Size` int NULL,
    `Color` int NULL,
    `ExpireDate` datetime(6) NULL,
    `Ingress` tinyint(1) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `TotalQuantity` decimal(12,2) NOT NULL,
    `RelatedId` char(36) COLLATE ascii_general_ci NULL,
    `RelatedIndex` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerInventory` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerInventory_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerInventory_TMarketer_MarketerId` FOREIGN KEY (`MarketerId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerInventory_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TAttendanceDeparture` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `EmployeeId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Amount` decimal(12,2) NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Mode` int NOT NULL,
    `Date` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TAttendanceDeparture` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TAttendanceDeparture_TEmployee_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `TEmployee` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TDiscountIncrease` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Amount` decimal(12,2) NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NOT NULL,
    `EmployeeId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Mode` int NOT NULL,
    `Date` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDiscountIncrease` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TDiscountIncrease_TEmployee_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `TEmployee` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TEmployeeAssignment` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Amount` decimal(12,2) NOT NULL,
    `PeriodTime` int NULL,
    `EndDate` datetime(6) NULL,
    `StartDate` datetime(6) NULL,
    `Note` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AssignmentsType` int NOT NULL,
    `EmployeeId` char(36) COLLATE ascii_general_ci NOT NULL,
    `AssignmentsMoney` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TEmployeeAssignment` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TEmployeeAssignment_TEmployee_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `TEmployee` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TRole` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Key` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Windowname` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IsSecret` tinyint(1) NOT NULL,
    `TUserId` char(36) COLLATE ascii_general_ci NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TRole` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TRole_TUser_TUserId` FOREIGN KEY (`TUserId`) REFERENCES `TUser` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TDisbursementPermitDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `DisbursementPermitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDisbursementPermitDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TDisbursementPermitDetail_TDisbursementPermit_DisbursementPe~` FOREIGN KEY (`DisbursementPermitId`) REFERENCES `TDisbursementPermit` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDisbursementPermitDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDisbursementPermitDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerDisbursementPermitDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `MarketerDisbursementPermitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `Price` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerDisbursementPermitDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerDisbursementPermitDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerDisbursementPermitDetail_TMarketerDisbursementPermi~` FOREIGN KEY (`MarketerDisbursementPermitId`) REFERENCES `TMarketerDisbursementPermit` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerDisbursementPermitDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TMarketerSalesInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `MarketerSalesInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Price` decimal(12,2) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `Discount` decimal(12,2) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TMarketerSalesInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TMarketerSalesInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerSalesInvoiceDetail_TMarketerSalesInvoice_MarketerSa~` FOREIGN KEY (`MarketerSalesInvoiceId`) REFERENCES `TMarketerSalesInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TMarketerSalesInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSalesInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `SalesInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Price` decimal(12,2) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `Discount` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSalesInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSalesInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSalesInvoiceDetail_TSalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `TSalesInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSalesInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TPurchaseInvoiceCost` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `PurchaseInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Ammount` decimal(12,2) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TPurchaseInvoiceCost` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TPurchaseInvoiceCost_TPurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `TPurchaseInvoice` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TPurchaseInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `PurchaseInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `Price` decimal(12,2) NOT NULL,
    `Discount` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TPurchaseInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TPurchaseInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseInvoiceDetail_TPurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `TPurchaseInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSupplyPermitDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `SupplyPermitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSupplyPermitDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSupplyPermitDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSupplyPermitDetail_TSupplyPermit_SupplyPermitId` FOREIGN KEY (`SupplyPermitId`) REFERENCES `TSupplyPermit` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSupplyPermitDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TInventoryCost` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `IsReturn` tinyint(1) NOT NULL,
    `Cost` decimal(12,2) NOT NULL,
    `Price` decimal(12,2) NULL,
    `AvrageCost` decimal(12,2) NOT NULL,
    `AvragePiceCost` decimal(12,2) NULL,
    `SellPrice` decimal(12,2) NULL,
    `TInventoryId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TInventoryCost` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TInventoryCost_TInventory_TInventoryId` FOREIGN KEY (`TInventoryId`) REFERENCES `TInventory` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TRoleGroupDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `RoleGroupId` char(36) COLLATE ascii_general_ci NOT NULL,
    `RoleId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TRoleGroupDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TRoleGroupDetail_TRoleGroup_RoleGroupId` FOREIGN KEY (`RoleGroupId`) REFERENCES `TRoleGroup` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TRoleGroupDetail_TRole_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `TRole` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `Accounts` (`Id`, `AccountCat`, `CreateDate`, `CurrencyId`, `Guid`, `IsDeleted`, `Name`, `Natural`, `Number`, `OpeningCreditor`, `OpeningDebtor`, `ParentId`, `TenantId`)
VALUES ('0e54960c-8a5b-473d-8022-c4a47c65185f', 3, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مصروفات', 0, 5, 0.0, 0.0, NULL, 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('1c63e7c0-033d-435b-b55f-4a37cb6ab77c', 2, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'إيرادات', 1, 4, 0.0, 0.0, NULL, 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('56844955-106c-4f9f-8f07-b8e33b3c2812', 1, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'حقوق الملكية', 1, 3, 0.0, 0.0, NULL, 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('83a0c584-5013-4186-a337-2985d38b6544', 1, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'خصوم', 1, 2, 0.0, 0.0, NULL, 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('9002ea94-02ff-47f0-907c-00a8360c440b', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'أصول', 0, 1, 0.0, 0.0, NULL, 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8');

INSERT INTO `TConfig` (`Id`, `AssetsAccount`, `BankAccount`, `ClientsAccount`, `DiscountAccount`, `EmployeesAccount`, `ExpenseAccount`, `GeneralClientAccountAccount`, `GeneralShareAccount`, `GeneralSupplierAccountAccount`, `Guid`, `IsDeleted`, `MarketerInvoicesAccountId`, `PurchaseInvoicesAccount`, `SafeAccount`, `SalesInvoicesAccountId`, `SupplyersAccount`, `TenantId`)
VALUES ('adecdcbe-5c66-4069-9f41-ee29aa4a6da1', 'ad630d24-f791-4d5a-8104-27595e273112', 'adecdcbe-5c66-4069-9f41-ee29aa4a6da2', 'adecdcbe-5c66-4069-9f41-ee29aa4a6dc2', '0e54960c-8a5b-473d-8022-c8847c651104', '13440fba-02cd-4f4b-843b-6159611c3fac', '0e54960c-8a5b-473d-8022-c4a47c651001', 'adecdcbe-5c66-4069-9f41-ee29aa4a6dc3', '0e54960c-8a5b-473d-8022-c8847c651103', '2b5ee402-49f1-444c-9a86-bf76e0ecbf91', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, '0e54960c-8a5b-473d-8022-c4a47c651103', '0e54960c-8a5b-473d-8022-c4a47c651002', 'adecdcbe-5c66-4069-9f41-ee29aa4a6da1', '0e54960c-8a5b-473d-8022-c4a47c651102', '2b5ee402-49f1-444c-9a86-bf76e0ecbf90', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1');

INSERT INTO `TCurrency` (`Id`, `CreateDate`, `Guid`, `IsDeleted`, `IsPrimary`, `Letter`, `Name`, `TenantId`, `TransfereRate`)
VALUES ('fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', TIMESTAMP '2024-07-06 00:00:00', '14ebdb67-1d7d-41e2-b766-dee66415b487', FALSE, TRUE, 'LYD', 'دينار ليبي', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a8f', 0.0);

INSERT INTO `TRole` (`Id`, `Description`, `Guid`, `IsDeleted`, `IsSecret`, `Key`, `Name`, `TUserId`, `TenantId`, `Windowname`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4a00001', 'Performs the Show action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Show', 'Show Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00002', 'Performs the Create action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Create', 'Create Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00003', 'Performs the Update action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Update', 'Update Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00004', 'Performs the Delete action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Delete', 'Delete Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00005', 'Performs the Export action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Export', 'Export Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00006', 'Performs the Import action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Import', 'Import Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00007', 'Performs the Print action on the Account entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Account.Print', 'Print Account', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00008', 'Performs the Show action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Show', 'Show ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00009', 'Performs the Create action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Create', 'Create ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00010', 'Performs the Update action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Update', 'Update ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00011', 'Performs the Delete action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Delete', 'Delete ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00012', 'Performs the Export action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Export', 'Export ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00013', 'Performs the Import action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Import', 'Import ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00014', 'Performs the Print action on the ExpenseTypes entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'ExpenseTypes.Print', 'Print ExpenseTypes', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00015', 'Performs the Show action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Show', 'Show Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00016', 'Performs the Create action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Create', 'Create Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00017', 'Performs the Update action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Update', 'Update Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00018', 'Performs the Delete action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Delete', 'Delete Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00019', 'Performs the Export action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Export', 'Export Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00020', 'Performs the Import action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Import', 'Import Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00021', 'Performs the Print action on the Entry entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Entry.Print', 'Print Entry', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00022', 'Performs the Show action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Show', 'Show Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00023', 'Performs the Create action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Create', 'Create Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00024', 'Performs the Update action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Update', 'Update Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00025', 'Performs the Delete action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Delete', 'Delete Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00026', 'Performs the Export action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Export', 'Export Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00027', 'Performs the Import action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Import', 'Import Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00028', 'Performs the Print action on the Assets entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Assets.Print', 'Print Assets', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00029', 'Performs the Show action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Show', 'Show Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00030', 'Performs the Create action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Create', 'Create Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00031', 'Performs the Update action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Update', 'Update Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00032', 'Performs the Delete action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Delete', 'Delete Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00033', 'Performs the Export action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Export', 'Export Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00034', 'Performs the Import action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Import', 'Import Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00035', 'Performs the Print action on the Employee entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Employee.Print', 'Print Employee', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00036', 'Performs the Show action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Show', 'Show EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00037', 'Performs the Create action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Create', 'Create EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00038', 'Performs the Update action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Update', 'Update EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00039', 'Performs the Delete action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Delete', 'Delete EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00040', 'Performs the Export action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Export', 'Export EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00041', 'Performs the Import action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Import', 'Import EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00042', 'Performs the Print action on the EmployeeAssignment entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'EmployeeAssignment.Print', 'Print EmployeeAssignment', NULL, '00000000-0000-0000-0000-000000000000', '');
INSERT INTO `TRole` (`Id`, `Description`, `Guid`, `IsDeleted`, `IsSecret`, `Key`, `Name`, `TUserId`, `TenantId`, `Windowname`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4a00043', 'Performs the Show action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Show', 'Show Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00044', 'Performs the Create action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Create', 'Create Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00045', 'Performs the Update action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Update', 'Update Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00046', 'Performs the Delete action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Delete', 'Delete Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00047', 'Performs the Export action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Export', 'Export Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00048', 'Performs the Import action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Import', 'Import Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00049', 'Performs the Print action on the Department entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Department.Print', 'Print Department', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00050', 'Performs the Show action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Show', 'Show DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00051', 'Performs the Create action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Create', 'Create DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00052', 'Performs the Update action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Update', 'Update DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00053', 'Performs the Delete action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Delete', 'Delete DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00054', 'Performs the Export action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Export', 'Export DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00055', 'Performs the Import action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Import', 'Import DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00056', 'Performs the Print action on the DiscountIncrease entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'DiscountIncrease.Print', 'Print DiscountIncrease', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00057', 'Performs the Show action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Show', 'Show Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00058', 'Performs the Create action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Create', 'Create Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00059', 'Performs the Update action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Update', 'Update Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00060', 'Performs the Delete action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Delete', 'Delete Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00061', 'Performs the Export action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Export', 'Export Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00062', 'Performs the Import action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Import', 'Import Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00063', 'Performs the Print action on the Category entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Category.Print', 'Print Category', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00064', 'Performs the Show action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Show', 'Show Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00065', 'Performs the Create action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Create', 'Create Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00066', 'Performs the Update action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Update', 'Update Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00067', 'Performs the Delete action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Delete', 'Delete Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00068', 'Performs the Export action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Export', 'Export Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00069', 'Performs the Import action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Import', 'Import Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00070', 'Performs the Print action on the Store entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Store.Print', 'Print Store', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00071', 'Performs the Show action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Show', 'Show CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00072', 'Performs the Create action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Create', 'Create CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00073', 'Performs the Update action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Update', 'Update CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00074', 'Performs the Delete action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Delete', 'Delete CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00075', 'Performs the Export action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Export', 'Export CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00076', 'Performs the Import action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Import', 'Import CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00077', 'Performs the Print action on the CostCenter entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'CostCenter.Print', 'Print CostCenter', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00078', 'Performs the Show action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Show', 'Show Unit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00079', 'Performs the Create action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Create', 'Create Unit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00080', 'Performs the Update action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Update', 'Update Unit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00081', 'Performs the Delete action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Delete', 'Delete Unit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00082', 'Performs the Export action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Export', 'Export Unit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00083', 'Performs the Import action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Import', 'Import Unit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00084', 'Performs the Print action on the Unit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Unit.Print', 'Print Unit', NULL, '00000000-0000-0000-0000-000000000000', '');
INSERT INTO `TRole` (`Id`, `Description`, `Guid`, `IsDeleted`, `IsSecret`, `Key`, `Name`, `TUserId`, `TenantId`, `Windowname`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4a00085', 'Performs the Show action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Show', 'Show Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00086', 'Performs the Create action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Create', 'Create Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00087', 'Performs the Update action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Update', 'Update Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00088', 'Performs the Delete action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Delete', 'Delete Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00089', 'Performs the Export action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Export', 'Export Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00090', 'Performs the Import action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Import', 'Import Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00091', 'Performs the Print action on the Supplier entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Supplier.Print', 'Print Supplier', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00092', 'Performs the Show action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Show', 'Show Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00093', 'Performs the Create action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Create', 'Create Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00094', 'Performs the Update action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Update', 'Update Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00095', 'Performs the Delete action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Delete', 'Delete Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00096', 'Performs the Export action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Export', 'Export Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00097', 'Performs the Import action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Import', 'Import Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00098', 'Performs the Print action on the Item entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Item.Print', 'Print Item', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00099', 'Performs the Show action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Show', 'Show Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00100', 'Performs the Create action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Create', 'Create Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00101', 'Performs the Update action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Update', 'Update Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00102', 'Performs the Delete action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Delete', 'Delete Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00103', 'Performs the Export action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Export', 'Export Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00104', 'Performs the Import action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Import', 'Import Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00105', 'Performs the Print action on the Sales entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Sales.Print', 'Print Sales', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00106', 'Performs the Show action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Show', 'Show Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00107', 'Performs the Create action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Create', 'Create Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00108', 'Performs the Update action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Update', 'Update Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00109', 'Performs the Delete action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Delete', 'Delete Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00110', 'Performs the Export action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Export', 'Export Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00111', 'Performs the Import action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Import', 'Import Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00112', 'Performs the Print action on the Safe entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Safe.Print', 'Print Safe', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00113', 'Performs the Show action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Show', 'Show Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00114', 'Performs the Create action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Create', 'Create Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00115', 'Performs the Update action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Update', 'Update Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00116', 'Performs the Delete action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Delete', 'Delete Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00117', 'Performs the Export action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Export', 'Export Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00118', 'Performs the Import action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Import', 'Import Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00119', 'Performs the Print action on the Bank entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Bank.Print', 'Print Bank', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00120', 'Performs the Show action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Show', 'Show BankBranch', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00121', 'Performs the Create action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Create', 'Create BankBranch', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00122', 'Performs the Update action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Update', 'Update BankBranch', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00123', 'Performs the Delete action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Delete', 'Delete BankBranch', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00124', 'Performs the Export action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Export', 'Export BankBranch', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00125', 'Performs the Import action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Import', 'Import BankBranch', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00126', 'Performs the Print action on the BankBranch entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'BankBranch.Print', 'Print BankBranch', NULL, '00000000-0000-0000-0000-000000000000', '');
INSERT INTO `TRole` (`Id`, `Description`, `Guid`, `IsDeleted`, `IsSecret`, `Key`, `Name`, `TUserId`, `TenantId`, `Windowname`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4a00127', 'Performs the Show action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Show', 'Show Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00128', 'Performs the Create action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Create', 'Create Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00129', 'Performs the Update action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Update', 'Update Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00130', 'Performs the Delete action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Delete', 'Delete Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00131', 'Performs the Export action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Export', 'Export Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00132', 'Performs the Import action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Import', 'Import Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00133', 'Performs the Print action on the Client entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Client.Print', 'Print Client', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00134', 'Performs the Show action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Show', 'Show Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00135', 'Performs the Create action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Create', 'Create Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00136', 'Performs the Update action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Update', 'Update Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00137', 'Performs the Delete action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Delete', 'Delete Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00138', 'Performs the Export action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Export', 'Export Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00139', 'Performs the Import action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Import', 'Import Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00140', 'Performs the Print action on the Purchase entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Purchase.Print', 'Print Purchase', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00141', 'Performs the Show action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Show', 'Show FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00142', 'Performs the Create action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Create', 'Create FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00143', 'Performs the Update action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Update', 'Update FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00144', 'Performs the Delete action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Delete', 'Delete FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00145', 'Performs the Export action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Export', 'Export FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00146', 'Performs the Import action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Import', 'Import FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00147', 'Performs the Print action on the FinancingOperations entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'FinancingOperations.Print', 'Print FinancingOperations', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00148', 'Performs the Show action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Show', 'Show Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00149', 'Performs the Create action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Create', 'Create Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00150', 'Performs the Update action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Update', 'Update Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00151', 'Performs the Delete action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Delete', 'Delete Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00152', 'Performs the Export action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Export', 'Export Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00153', 'Performs the Import action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Import', 'Import Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00154', 'Performs the Print action on the Marketer entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Marketer.Print', 'Print Marketer', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00155', 'Performs the Show action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Show', 'Show MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00156', 'Performs the Create action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Create', 'Create MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00157', 'Performs the Update action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Update', 'Update MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00158', 'Performs the Delete action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Delete', 'Delete MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00159', 'Performs the Export action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Export', 'Export MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00160', 'Performs the Import action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Import', 'Import MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00161', 'Performs the Print action on the MarketerDisbursementPermit entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'MarketerDisbursementPermit.Print', 'Print MarketerDisbursementPermit', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00162', 'Performs the Show action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Show', 'Show Financial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00163', 'Performs the Create action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Create', 'Create Financial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00164', 'Performs the Update action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Update', 'Update Financial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00165', 'Performs the Delete action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Delete', 'Delete Financial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00166', 'Performs the Export action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Export', 'Export Financial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00167', 'Performs the Import action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Import', 'Import Financial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00168', 'Performs the Print action on the Financial entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Financial.Print', 'Print Financial', NULL, '00000000-0000-0000-0000-000000000000', '');
INSERT INTO `TRole` (`Id`, `Description`, `Guid`, `IsDeleted`, `IsSecret`, `Key`, `Name`, `TUserId`, `TenantId`, `Windowname`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4a00169', 'Performs the Show action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Show', 'Show StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00170', 'Performs the Create action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Create', 'Create StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00171', 'Performs the Update action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Update', 'Update StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00172', 'Performs the Delete action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Delete', 'Delete StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00173', 'Performs the Export action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Export', 'Export StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00174', 'Performs the Import action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Import', 'Import StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00175', 'Performs the Print action on the StatementsNavigator entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'StatementsNavigator.Print', 'Print StatementsNavigator', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00176', 'Performs the Show action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Show', 'Show Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00177', 'Performs the Create action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Create', 'Create Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00178', 'Performs the Update action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Update', 'Update Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00179', 'Performs the Delete action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Delete', 'Delete Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00180', 'Performs the Export action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Export', 'Export Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00181', 'Performs the Import action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Import', 'Import Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00182', 'Performs the Print action on the Config entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Config.Print', 'Print Config', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00183', 'Performs the Show action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Show', 'Show Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00184', 'Performs the Create action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Create', 'Create Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00185', 'Performs the Update action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Update', 'Update Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00186', 'Performs the Delete action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Delete', 'Delete Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00187', 'Performs the Export action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Export', 'Export Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00188', 'Performs the Import action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Import', 'Import Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00189', 'Performs the Print action on the Group entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'Group.Print', 'Print Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00190', 'Performs the Show action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Show', 'Show User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00191', 'Performs the Create action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Create', 'Create User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00192', 'Performs the Update action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Update', 'Update User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00193', 'Performs the Delete action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Delete', 'Delete User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00194', 'Performs the Export action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Export', 'Export User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00195', 'Performs the Import action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Import', 'Import User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00196', 'Performs the Print action on the User entity', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, 'User.Print', 'Print User', NULL, '00000000-0000-0000-0000-000000000000', '');

INSERT INTO `TUser` (`Id`, `Accesser`, `Guid`, `IsActive`, `IsAdmin`, `IsDeleted`, `IsGhost`, `IsRedy`, `Name`, `Password`, `Phone`, `RegesterTime`, `RoleGroupId`, `Sult`, `TenantId`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4acb001', '', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, TRUE, FALSE, TRUE, FALSE, 'Ghost', 'koad/tT8QGmZmk2B/s2iMALIvKdFe3wfFbwKWtbdhIKPoj2GaAQ/8/lobJxGgYP9cPyb3gfbY+Ery/zS6Mdvew==', '0924951905', TIMESTAMP '0001-01-01 00:00:00', NULL, 'SPcjqk+2nrGlvEnChIh3tMY2adLkzMIGypBaY4h2qcuep5N9wL5lrHekm1EOMaNRARwd67ZotceKY7eNRXXx3nBvmk4lbeG86xxAWZnuuXA3nnmLO//9k7Yx4IQbcLUwOUQOfmJbTKIn5RBYf12gMbOX+EKdPo93C/I2mlX0C2Y=', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1'),
('08b4dae1-2eef-4509-b1e6-1fa9b4acb002', '', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, FALSE, FALSE, FALSE, FALSE, 'User', 'iD+ZkASu6DmxMy6VVV+DO+EA3s+VEWzYCdmyKRELy8maXGluq2TWqQ0CfAZsHbHScBO8ib31QSHYiSezYTnVWQ==', '0915951905', TIMESTAMP '0001-01-01 00:00:00', NULL, '/BcPKD/+VMk2vI+8UobrzgSmvizsDgHHPgQzz65YrqH/2fL9nsd9/ju7StpKItqvg5Gxd78Je57NWvWH03aYo9d9pRwYY9sYgJdxmyBzAo7h7vK9G+35WwOI43RPFiIbidOv4fZjhZ8lf5J0+4Fh8l8FvdlIbS0+vb5lga0dxxA=', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1'),
('08b4dae1-2eef-4509-b1e6-1fa9b4acb003', '', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0', FALSE, TRUE, FALSE, FALSE, FALSE, 'Admin', 'dgNZRDuZ4vtReF151Y3RWLNio5IemIuS3OWYbPsbhfAyq+nqQ+4RQF953hbGpTeXjU4QxG7HsmZj8gB/qDbHuw==', '0924951904', TIMESTAMP '0001-01-01 00:00:00', NULL, 'sq/uSGqIZcbHYRlFWL81zClVi57NEEoJEx1FH2/lblMBx2YUh4P8BN52wf29UuFoP/6lbnYqpq/I8oyYkf9vPGIOlUUHFy4IKYC3TAi5aZLIUAYu5LDN09gSc72+Jf1e8SwTLST4e4jV0KdV4GvKJw27dcqZS88xzbhe/8zcdKY=', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1');

INSERT INTO `Accounts` (`Id`, `AccountCat`, `CreateDate`, `CurrencyId`, `Guid`, `IsDeleted`, `Name`, `Natural`, `Number`, `OpeningCreditor`, `OpeningDebtor`, `ParentId`, `TenantId`)
VALUES ('0e54960c-8a5b-473d-8022-c4a47c651001', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مصروفات عمومية', 0, 51, 0.0, 0.0, '0e54960c-8a5b-473d-8022-c4a47c65185f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('0e54960c-8a5b-473d-8022-c4a47c651002', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مشتريات', 0, 52, 0.0, 0.0, '0e54960c-8a5b-473d-8022-c4a47c65185f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('0e54960c-8a5b-473d-8022-c4a47c651102', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مبيعات', 1, 41, 0.0, 0.0, '1c63e7c0-033d-435b-b55f-4a37cb6ab77c', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('0e54960c-8a5b-473d-8022-c4a47c651103', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مبيعات المسوقين', 1, 41, 0.0, 0.0, '1c63e7c0-033d-435b-b55f-4a37cb6ab77c', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('0e54960c-8a5b-473d-8022-c8847c651103', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'حساب العمولات العام', 1, 41, 0.0, 0.0, '0e54960c-8a5b-473d-8022-c4a47c65185f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('0e54960c-8a5b-473d-8022-c8847c651104', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'حساب التخفيضات', 1, 41, 0.0, 0.0, '0e54960c-8a5b-473d-8022-c4a47c65185f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('18847d06-8ff6-4e55-9b63-8318edbf6197', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'أصول متداولة', 0, 12, 0.0, 0.0, '9002ea94-02ff-47f0-907c-00a8360c440b', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('ad630d24-f791-4d5a-8104-27595e273112', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'أصول ثابتة', 0, 11, 0.0, 0.0, '9002ea94-02ff-47f0-907c-00a8360c440b', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('dbcb6851-213c-4238-a2a9-46f0d359c878', 1, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'خصوم متداولة', 1, 22, 0.0, 0.0, '83a0c584-5013-4186-a337-2985d38b6544', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('e87b3121-fb01-489a-9081-b50f9e2e3a91', 1, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'خصوم ثابتة', 1, 21, 0.0, 0.0, '83a0c584-5013-4186-a337-2985d38b6544', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('1036cd62-716a-4b8c-a19f-40ee2584484f', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'سيارات', 0, 112, 0.0, 0.0, 'ad630d24-f791-4d5a-8104-27595e273112', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('13440fba-02cd-4f4b-843b-6159611c3fac', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'الموظفين', 1, 222, 0.0, 0.0, 'dbcb6851-213c-4238-a2a9-46f0d359c878', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('23b638f5-71d4-4256-b99b-5b84b19c91a7', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مباني', 0, 111, 0.0, 0.0, 'ad630d24-f791-4d5a-8104-27595e273112', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('2b5ee402-49f1-444c-9a86-bf76e0ecbf90', 1, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'الموردين', 1, 221, 0.0, 0.0, 'dbcb6851-213c-4238-a2a9-46f0d359c878', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('a65d59a6-4291-4bfe-b718-9c676a5b8578', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'أثاث', 0, 123, 0.0, 0.0, 'ad630d24-f791-4d5a-8104-27595e273112', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('adecdcbe-5c66-4069-9f41-ee29aa4a6da1', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'الخزائن', 0, 122, 0.0, 0.0, '18847d06-8ff6-4e55-9b63-8318edbf6197', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('adecdcbe-5c66-4069-9f41-ee29aa4a6da2', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'البنوك', 0, 123, 0.0, 0.0, '18847d06-8ff6-4e55-9b63-8318edbf6197', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('adecdcbe-5c66-4069-9f41-ee29aa4a6dc2', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'العملاء', 0, 121, 0.0, 0.0, '18847d06-8ff6-4e55-9b63-8318edbf6197', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('2b5ee402-49f1-444c-9a86-bf76e0ecbf91', 1, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'مورد عام', 1, 2211, 0.0, 0.0, '2b5ee402-49f1-444c-9a86-bf76e0ecbf90', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8'),
('adecdcbe-5c66-4069-9f41-ee29aa4a6dc3', 0, TIMESTAMP '2024-07-02 00:00:00', 'fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5', FALSE, 'عميل عام', 0, 1211, 0.0, 0.0, 'adecdcbe-5c66-4069-9f41-ee29aa4a6dc2', 'eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8');

CREATE INDEX `IX_Accounts_ParentId` ON `Accounts` (`ParentId`);

CREATE INDEX `IX_TAssets_AccountId` ON `TAssets` (`AccountId`);

CREATE INDEX `IX_TAttendanceDeparture_EmployeeId` ON `TAttendanceDeparture` (`EmployeeId`);

CREATE INDEX `IX_TBank_AccountId` ON `TBank` (`AccountId`);

CREATE INDEX `IX_TBankBranch_AccountId` ON `TBankBranch` (`AccountId`);

CREATE INDEX `IX_TBankBranch_BankId` ON `TBankBranch` (`BankId`);

CREATE INDEX `IX_TCategory_CategoryId` ON `TCategory` (`CategoryId`);

CREATE INDEX `IX_TClient_AccountId` ON `TClient` (`AccountId`);

CREATE INDEX `IX_TDisbursementPermit_CostCenterId` ON `TDisbursementPermit` (`CostCenterId`);

CREATE INDEX `IX_TDisbursementPermit_StoreId` ON `TDisbursementPermit` (`StoreId`);

CREATE INDEX `IX_TDisbursementPermitDetail_DisbursementPermitId` ON `TDisbursementPermitDetail` (`DisbursementPermitId`);

CREATE INDEX `IX_TDisbursementPermitDetail_ItemId` ON `TDisbursementPermitDetail` (`ItemId`);

CREATE INDEX `IX_TDisbursementPermitDetail_UnitId` ON `TDisbursementPermitDetail` (`UnitId`);

CREATE INDEX `IX_TDiscountIncrease_EmployeeId` ON `TDiscountIncrease` (`EmployeeId`);

CREATE INDEX `IX_TDiscountitedItem_ClientId` ON `TDiscountitedItem` (`ClientId`);

CREATE INDEX `IX_TDiscountitedItem_ItemId` ON `TDiscountitedItem` (`ItemId`);

CREATE INDEX `IX_TDiscountitedItem_UnitId` ON `TDiscountitedItem` (`UnitId`);

CREATE UNIQUE INDEX `IX_TEmployee_AccountId` ON `TEmployee` (`AccountId`);

CREATE INDEX `IX_TEmployee_DepartmentId` ON `TEmployee` (`DepartmentId`);

CREATE INDEX `IX_TEmployeeAssignment_EmployeeId` ON `TEmployeeAssignment` (`EmployeeId`);

CREATE INDEX `IX_TEntryDetail_AccountId` ON `TEntryDetail` (`AccountId`);

CREATE INDEX `IX_TEntryDetail_EntryId` ON `TEntryDetail` (`EntryId`);

CREATE INDEX `IX_TInventory_ItemId` ON `TInventory` (`ItemId`);

CREATE INDEX `IX_TInventory_StoreId` ON `TInventory` (`StoreId`);

CREATE INDEX `IX_TInventory_UnitId` ON `TInventory` (`UnitId`);

CREATE UNIQUE INDEX `IX_TInventoryCost_TInventoryId` ON `TInventoryCost` (`TInventoryId`);

CREATE INDEX `IX_TItem_CategoryId` ON `TItem` (`CategoryId`);

CREATE INDEX `IX_TItemUnit_ItemId` ON `TItemUnit` (`ItemId`);

CREATE INDEX `IX_TItemUnit_UnitId` ON `TItemUnit` (`UnitId`);

CREATE INDEX `IX_TMarketer_AccountId` ON `TMarketer` (`AccountId`);

CREATE INDEX `IX_TMarketerClient_ClientId` ON `TMarketerClient` (`ClientId`);

CREATE INDEX `IX_TMarketerClient_MarketerId` ON `TMarketerClient` (`MarketerId`);

CREATE INDEX `IX_TMarketerDisbursementPermit_MarketerId` ON `TMarketerDisbursementPermit` (`MarketerId`);

CREATE INDEX `IX_TMarketerDisbursementPermit_StoreId` ON `TMarketerDisbursementPermit` (`StoreId`);

CREATE INDEX `IX_TMarketerDisbursementPermitDetail_ItemId` ON `TMarketerDisbursementPermitDetail` (`ItemId`);

CREATE INDEX `IX_TMarketerDisbursementPermitDetail_MarketerDisbursementPermit~` ON `TMarketerDisbursementPermitDetail` (`MarketerDisbursementPermitId`);

CREATE INDEX `IX_TMarketerDisbursementPermitDetail_UnitId` ON `TMarketerDisbursementPermitDetail` (`UnitId`);

CREATE INDEX `IX_TMarketerInventory_ItemId` ON `TMarketerInventory` (`ItemId`);

CREATE INDEX `IX_TMarketerInventory_MarketerId` ON `TMarketerInventory` (`MarketerId`);

CREATE INDEX `IX_TMarketerInventory_UnitId` ON `TMarketerInventory` (`UnitId`);

CREATE INDEX `IX_TMarketerSalesInvoice_ClientId` ON `TMarketerSalesInvoice` (`ClientId`);

CREATE INDEX `IX_TMarketerSalesInvoice_MarketerId` ON `TMarketerSalesInvoice` (`MarketerId`);

CREATE INDEX `IX_TMarketerSalesInvoiceDetail_ItemId` ON `TMarketerSalesInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TMarketerSalesInvoiceDetail_MarketerSalesInvoiceId` ON `TMarketerSalesInvoiceDetail` (`MarketerSalesInvoiceId`);

CREATE INDEX `IX_TMarketerSalesInvoiceDetail_UnitId` ON `TMarketerSalesInvoiceDetail` (`UnitId`);

CREATE INDEX `IX_TMarketerSharedEntry_EntryId` ON `TMarketerSharedEntry` (`EntryId`);

CREATE INDEX `IX_TMarketerSharedEntry_MarketerId` ON `TMarketerSharedEntry` (`MarketerId`);

CREATE INDEX `IX_TPurchaseInvoice_SafeId` ON `TPurchaseInvoice` (`SafeId`);

CREATE INDEX `IX_TPurchaseInvoice_StoreId` ON `TPurchaseInvoice` (`StoreId`);

CREATE INDEX `IX_TPurchaseInvoice_SupplierId` ON `TPurchaseInvoice` (`SupplierId`);

CREATE INDEX `IX_TPurchaseInvoiceCost_PurchaseInvoiceId` ON `TPurchaseInvoiceCost` (`PurchaseInvoiceId`);

CREATE INDEX `IX_TPurchaseInvoiceDetail_ItemId` ON `TPurchaseInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TPurchaseInvoiceDetail_PurchaseInvoiceId` ON `TPurchaseInvoiceDetail` (`PurchaseInvoiceId`);

CREATE INDEX `IX_TPurchaseInvoiceDetail_UnitId` ON `TPurchaseInvoiceDetail` (`UnitId`);

CREATE INDEX `IX_TRole_TUserId` ON `TRole` (`TUserId`);

CREATE INDEX `IX_TRoleGroupDetail_RoleGroupId` ON `TRoleGroupDetail` (`RoleGroupId`);

CREATE INDEX `IX_TRoleGroupDetail_RoleId` ON `TRoleGroupDetail` (`RoleId`);

CREATE INDEX `IX_TSafe_AccountId` ON `TSafe` (`AccountId`);

CREATE INDEX `IX_TSalesInvoice_ClientId` ON `TSalesInvoice` (`ClientId`);

CREATE INDEX `IX_TSalesInvoice_MarketerStorageId` ON `TSalesInvoice` (`MarketerStorageId`);

CREATE INDEX `IX_TSalesInvoice_RepresentativeId` ON `TSalesInvoice` (`RepresentativeId`);

CREATE INDEX `IX_TSalesInvoice_SafeId` ON `TSalesInvoice` (`SafeId`);

CREATE INDEX `IX_TSalesInvoice_StoreId` ON `TSalesInvoice` (`StoreId`);

CREATE INDEX `IX_TSalesInvoiceDetail_ItemId` ON `TSalesInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TSalesInvoiceDetail_SalesInvoiceId` ON `TSalesInvoiceDetail` (`SalesInvoiceId`);

CREATE INDEX `IX_TSalesInvoiceDetail_UnitId` ON `TSalesInvoiceDetail` (`UnitId`);

CREATE INDEX `IX_TSupplier_AccountId` ON `TSupplier` (`AccountId`);

CREATE INDEX `IX_TSupplyPermit_StoreId` ON `TSupplyPermit` (`StoreId`);

CREATE INDEX `IX_TSupplyPermit_SupplierId` ON `TSupplyPermit` (`SupplierId`);

CREATE INDEX `IX_TSupplyPermitDetail_ItemId` ON `TSupplyPermitDetail` (`ItemId`);

CREATE INDEX `IX_TSupplyPermitDetail_SupplyPermitId` ON `TSupplyPermitDetail` (`SupplyPermitId`);

CREATE INDEX `IX_TSupplyPermitDetail_UnitId` ON `TSupplyPermitDetail` (`UnitId`);

CREATE INDEX `IX_TUser_RoleGroupId` ON `TUser` (`RoleGroupId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241102123325_2024.11.02-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TSalesInvoice` ADD `Notes` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `TPurchaseInvoice` ADD `Notes` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `TMarketerDisbursementPermit` ADD `Notes` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `TMarketerDisbursementPermit` ADD `SerialNumber` bigint NOT NULL DEFAULT 0;

ALTER TABLE `TMarketer` ADD `Salary` decimal(12,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `TClient` ADD `Address` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `TClient` ADD `MaxDebt` decimal(12,2) NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241104173900_2024-11-04--001', '8.0.11');

COMMIT;

START TRANSACTION;

CREATE TABLE `TQuantityCreditingInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `SerialNumber` bigint NOT NULL,
    `Number` bigint NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TQuantityCreditingInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TQuantityCreditingInvoice_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TQuantityCreditingInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `QuantityCreditingInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ExpireDate` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TQuantityCreditingInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TQuantityCreditingInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TQuantityCreditingInvoiceDetail_TQuantityCreditingInvoice_Qu~` FOREIGN KEY (`QuantityCreditingInvoiceId`) REFERENCES `TQuantityCreditingInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TQuantityCreditingInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_TQuantityCreditingInvoice_StoreId` ON `TQuantityCreditingInvoice` (`StoreId`);

CREATE INDEX `IX_TQuantityCreditingInvoiceDetail_ItemId` ON `TQuantityCreditingInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TQuantityCreditingInvoiceDetail_QuantityCreditingInvoiceId` ON `TQuantityCreditingInvoiceDetail` (`QuantityCreditingInvoiceId`);

CREATE INDEX `IX_TQuantityCreditingInvoiceDetail_UnitId` ON `TQuantityCreditingInvoiceDetail` (`UnitId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241113174042_2024.11.13-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TQuantityCreditingInvoiceDetail` ADD `Discount` decimal(12,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `TQuantityCreditingInvoiceDetail` ADD `Price` decimal(12,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `TQuantityCreditingInvoiceDetail` ADD `Quantity` decimal(12,2) NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241113190744_2024.11.13-002', '8.0.11');

COMMIT;

START TRANSACTION;

CREATE TABLE `TSellReturnInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `SerialNumber` bigint NOT NULL,
    `Number` bigint NOT NULL,
    `ClientId` char(36) COLLATE ascii_general_ci NULL,
    `InvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NULL,
    `PeymentType` int NOT NULL,
    `SafeId` char(36) COLLATE ascii_general_ci NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSellReturnInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSellReturnInvoice_TClient_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `TClient` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSellReturnInvoice_TSafe_SafeId` FOREIGN KEY (`SafeId`) REFERENCES `TSafe` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSellReturnInvoice_TSalesInvoice_InvoiceId` FOREIGN KEY (`InvoiceId`) REFERENCES `TSalesInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSellReturnInvoice_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TSellReturnInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `ReturnInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Price` decimal(12,2) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TSellReturnInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TSellReturnInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSellReturnInvoiceDetail_TSellReturnInvoice_ReturnInvoiceId` FOREIGN KEY (`ReturnInvoiceId`) REFERENCES `TSellReturnInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TSellReturnInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_TSellReturnInvoice_ClientId` ON `TSellReturnInvoice` (`ClientId`);

CREATE INDEX `IX_TSellReturnInvoice_InvoiceId` ON `TSellReturnInvoice` (`InvoiceId`);

CREATE INDEX `IX_TSellReturnInvoice_SafeId` ON `TSellReturnInvoice` (`SafeId`);

CREATE INDEX `IX_TSellReturnInvoice_StoreId` ON `TSellReturnInvoice` (`StoreId`);

CREATE INDEX `IX_TSellReturnInvoiceDetail_ItemId` ON `TSellReturnInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TSellReturnInvoiceDetail_ReturnInvoiceId` ON `TSellReturnInvoiceDetail` (`ReturnInvoiceId`);

CREATE INDEX `IX_TSellReturnInvoiceDetail_UnitId` ON `TSellReturnInvoiceDetail` (`UnitId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241115070730_2024.11.15-001', '8.0.11');

COMMIT;

START TRANSACTION;

UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00001'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00001';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00002'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00002';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00003'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00003';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00004'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00004';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00005'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00005';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00006'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00006';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00007'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00007';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00008'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00008';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00009'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00009';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00010'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00010';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00011'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00011';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00012'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00012';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00013'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00013';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00014'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00014';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00015'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00015';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00016'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00016';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00017'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00017';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00018'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00018';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00019'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00019';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00020'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00020';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00021'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00021';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00022'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00022';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00023'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00023';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00024'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00024';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00025'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00025';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00026'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00026';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00027'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00027';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00028'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00028';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00029'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00029';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00030'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00030';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00031'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00031';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00032'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00032';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00033'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00033';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00034'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00034';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00035'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00035';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00036'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00036';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00037'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00037';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00038'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00038';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00039'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00039';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00040'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00040';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00041'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00041';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00042'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00042';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00043'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00043';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00044'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00044';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00045'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00045';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00046'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00046';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00047'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00047';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00048'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00048';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00049'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00049';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00050'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00050';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00051'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00051';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00052'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00052';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00053'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00053';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00054'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00054';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00055'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00055';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00056'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00056';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00057'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00057';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00058'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00058';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00059'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00059';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00060'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00060';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00061'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00061';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00062'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00062';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00063'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00063';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00064'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00064';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00065'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00065';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00066'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00066';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00067'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00067';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00068'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00068';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00069'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00069';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00070'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00070';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00071'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00071';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00072'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00072';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00073'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00073';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00074'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00074';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00075'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00075';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00076'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00076';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00077'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00077';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00078'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00078';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00079'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00079';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00080'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00080';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00081'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00081';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00082'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00082';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00083'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00083';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00084'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00084';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00085'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00085';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00086'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00086';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00087'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00087';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00088'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00088';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00089'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00089';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00090'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00090';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00091'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00091';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00092'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00092';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00093'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00093';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00094'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00094';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00095'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00095';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00096'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00096';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00097'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00097';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00098'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00098';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00099'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00099';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00100'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00100';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00101'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00101';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00102'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00102';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00103'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00103';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00104'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00104';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00105'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00105';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00106'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00106';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00107'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00107';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00108'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00108';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00109'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00109';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00110'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00110';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00111'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00111';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00112'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00112';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00113'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00113';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00114'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00114';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00115'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00115';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00116'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00116';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00117'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00117';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00118'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00118';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00119'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00119';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00120'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00120';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00121'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00121';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00122'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00122';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00123'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00123';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00124'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00124';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00125'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00125';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00126'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00126';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00127'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00127';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00128'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00128';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00129'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00129';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00130'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00130';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00131'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00131';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00132'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00132';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00133'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00133';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00134', `Key` = 'SellReturnInvoice.Show', `Name` = 'Show SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00134';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00135', `Key` = 'SellReturnInvoice.Create', `Name` = 'Create SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00135';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00136', `Key` = 'SellReturnInvoice.Update', `Name` = 'Update SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00136';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00137', `Key` = 'SellReturnInvoice.Delete', `Name` = 'Delete SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00137';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00138', `Key` = 'SellReturnInvoice.Export', `Name` = 'Export SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00138';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00139', `Key` = 'SellReturnInvoice.Import', `Name` = 'Import SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00139';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the SellReturnInvoice entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00140', `Key` = 'SellReturnInvoice.Print', `Name` = 'Print SellReturnInvoice'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00140';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00141', `Key` = 'QuantityCrediting.Show', `Name` = 'Show QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00141';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00142', `Key` = 'QuantityCrediting.Create', `Name` = 'Create QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00142';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00143', `Key` = 'QuantityCrediting.Update', `Name` = 'Update QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00143';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00144', `Key` = 'QuantityCrediting.Delete', `Name` = 'Delete QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00144';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00145', `Key` = 'QuantityCrediting.Export', `Name` = 'Export QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00145';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00146', `Key` = 'QuantityCrediting.Import', `Name` = 'Import QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00146';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the QuantityCrediting entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00147', `Key` = 'QuantityCrediting.Print', `Name` = 'Print QuantityCrediting'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00147';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00148', `Key` = 'Purchase.Show', `Name` = 'Show Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00148';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00149', `Key` = 'Purchase.Create', `Name` = 'Create Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00149';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00150', `Key` = 'Purchase.Update', `Name` = 'Update Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00150';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00151', `Key` = 'Purchase.Delete', `Name` = 'Delete Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00151';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00152', `Key` = 'Purchase.Export', `Name` = 'Export Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00152';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00153', `Key` = 'Purchase.Import', `Name` = 'Import Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00153';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the Purchase entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00154', `Key` = 'Purchase.Print', `Name` = 'Print Purchase'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00154';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00155', `Key` = 'FinancingOperations.Show', `Name` = 'Show FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00155';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00156', `Key` = 'FinancingOperations.Create', `Name` = 'Create FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00156';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00157', `Key` = 'FinancingOperations.Update', `Name` = 'Update FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00157';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00158', `Key` = 'FinancingOperations.Delete', `Name` = 'Delete FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00158';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00159', `Key` = 'FinancingOperations.Export', `Name` = 'Export FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00159';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00160', `Key` = 'FinancingOperations.Import', `Name` = 'Import FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00160';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the FinancingOperations entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00161', `Key` = 'FinancingOperations.Print', `Name` = 'Print FinancingOperations'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00161';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00162', `Key` = 'Marketer.Show', `Name` = 'Show Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00162';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00163', `Key` = 'Marketer.Create', `Name` = 'Create Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00163';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00164', `Key` = 'Marketer.Update', `Name` = 'Update Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00164';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00165', `Key` = 'Marketer.Delete', `Name` = 'Delete Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00165';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00166', `Key` = 'Marketer.Export', `Name` = 'Export Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00166';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00167', `Key` = 'Marketer.Import', `Name` = 'Import Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00167';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the Marketer entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00168', `Key` = 'Marketer.Print', `Name` = 'Print Marketer'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00168';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00169', `Key` = 'MarketerDisbursementPermit.Show', `Name` = 'Show MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00169';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00170', `Key` = 'MarketerDisbursementPermit.Create', `Name` = 'Create MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00170';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00171', `Key` = 'MarketerDisbursementPermit.Update', `Name` = 'Update MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00171';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00172', `Key` = 'MarketerDisbursementPermit.Delete', `Name` = 'Delete MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00172';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00173', `Key` = 'MarketerDisbursementPermit.Export', `Name` = 'Export MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00173';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00174', `Key` = 'MarketerDisbursementPermit.Import', `Name` = 'Import MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00174';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the MarketerDisbursementPermit entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00175', `Key` = 'MarketerDisbursementPermit.Print', `Name` = 'Print MarketerDisbursementPermit'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00175';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00176', `Key` = 'Financial.Show', `Name` = 'Show Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00176';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00177', `Key` = 'Financial.Create', `Name` = 'Create Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00177';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00178', `Key` = 'Financial.Update', `Name` = 'Update Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00178';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00179', `Key` = 'Financial.Delete', `Name` = 'Delete Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00179';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00180', `Key` = 'Financial.Export', `Name` = 'Export Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00180';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00181', `Key` = 'Financial.Import', `Name` = 'Import Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00181';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the Financial entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00182', `Key` = 'Financial.Print', `Name` = 'Print Financial'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00182';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00183', `Key` = 'StatementsNavigator.Show', `Name` = 'Show StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00183';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00184', `Key` = 'StatementsNavigator.Create', `Name` = 'Create StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00184';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00185', `Key` = 'StatementsNavigator.Update', `Name` = 'Update StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00185';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00186', `Key` = 'StatementsNavigator.Delete', `Name` = 'Delete StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00186';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00187', `Key` = 'StatementsNavigator.Export', `Name` = 'Export StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00187';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00188', `Key` = 'StatementsNavigator.Import', `Name` = 'Import StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00188';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the StatementsNavigator entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00189', `Key` = 'StatementsNavigator.Print', `Name` = 'Print StatementsNavigator'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00189';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Show action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00190', `Key` = 'Config.Show', `Name` = 'Show Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00190';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Create action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00191', `Key` = 'Config.Create', `Name` = 'Create Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00191';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Update action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00192', `Key` = 'Config.Update', `Name` = 'Update Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00192';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Delete action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00193', `Key` = 'Config.Delete', `Name` = 'Delete Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00193';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Export action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00194', `Key` = 'Config.Export', `Name` = 'Export Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00194';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Import action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00195', `Key` = 'Config.Import', `Name` = 'Import Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00195';
SELECT ROW_COUNT();


UPDATE `TRole` SET `Description` = 'Performs the Print action on the Config entity', `Guid` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00196', `Key` = 'Config.Print', `Name` = 'Print Config'
WHERE `Id` = '08b4dae1-2eef-4509-b1e6-1fa9b4a00196';
SELECT ROW_COUNT();


INSERT INTO `TRole` (`Id`, `Description`, `Guid`, `IsDeleted`, `IsSecret`, `Key`, `Name`, `TUserId`, `TenantId`, `Windowname`)
VALUES ('08b4dae1-2eef-4509-b1e6-1fa9b4a00197', 'Performs the Show action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00197', FALSE, FALSE, 'Group.Show', 'Show Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00198', 'Performs the Create action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00198', FALSE, FALSE, 'Group.Create', 'Create Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00199', 'Performs the Update action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00199', FALSE, FALSE, 'Group.Update', 'Update Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00200', 'Performs the Delete action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00200', FALSE, FALSE, 'Group.Delete', 'Delete Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00201', 'Performs the Export action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00201', FALSE, FALSE, 'Group.Export', 'Export Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00202', 'Performs the Import action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00202', FALSE, FALSE, 'Group.Import', 'Import Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00203', 'Performs the Print action on the Group entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00203', FALSE, FALSE, 'Group.Print', 'Print Group', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00204', 'Performs the Show action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00204', FALSE, FALSE, 'User.Show', 'Show User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00205', 'Performs the Create action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00205', FALSE, FALSE, 'User.Create', 'Create User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00206', 'Performs the Update action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00206', FALSE, FALSE, 'User.Update', 'Update User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00207', 'Performs the Delete action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00207', FALSE, FALSE, 'User.Delete', 'Delete User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00208', 'Performs the Export action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00208', FALSE, FALSE, 'User.Export', 'Export User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00209', 'Performs the Import action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00209', FALSE, FALSE, 'User.Import', 'Import User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-2eef-4509-b1e6-1fa9b4a00210', 'Performs the Print action on the User entity', '08b4dae1-2eef-4509-b1e6-1fa9b4a00210', FALSE, FALSE, 'User.Print', 'Print User', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400001', 'Performs the Show action on the AccountMovements Report', '08b4dae1-3eef-4509-b1e6-869379400001', FALSE, FALSE, 'report.AccountMovements-Show', 'report Show AccountMovements', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400002', 'Performs the Show action on the TheClientDebtion Report', '08b4dae1-3eef-4509-b1e6-869379400002', FALSE, FALSE, 'report.TheClientDebtion-Show', 'report Show TheClientDebtion', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400003', 'Performs the Show action on the TheClientDebtionDetails Report', '08b4dae1-3eef-4509-b1e6-869379400003', FALSE, FALSE, 'report.TheClientDebtionDetails-Show', 'report Show TheClientDebtionDetails', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400004', 'Performs the Show action on the ClientsOverdueDebts Report', '08b4dae1-3eef-4509-b1e6-869379400004', FALSE, FALSE, 'report.ClientsOverdueDebts-Show', 'report Show ClientsOverdueDebts', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400005', 'Performs the Show action on the TheCSupplierDebtion Report', '08b4dae1-3eef-4509-b1e6-869379400005', FALSE, FALSE, 'report.TheCSupplierDebtion-Show', 'report Show TheCSupplierDebtion', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400006', 'Performs the Show action on the TheRepresentativeDebtionDetails Report', '08b4dae1-3eef-4509-b1e6-869379400006', FALSE, FALSE, 'report.TheRepresentativeDebtionDetails-Show', 'report Show TheRepresentativeDebtionDetails', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400007', 'Performs the Show action on the MarketerCredtions Report', '08b4dae1-3eef-4509-b1e6-869379400007', FALSE, FALSE, 'report.MarketerCredtions-Show', 'report Show MarketerCredtions', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400008', 'Performs the Show action on the ItemsStoragesQuantities Report', '08b4dae1-3eef-4509-b1e6-869379400008', FALSE, FALSE, 'report.ItemsStoragesQuantities-Show', 'report Show ItemsStoragesQuantities', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400009', 'Performs the Show action on the ItemsMarkersQuantities Report', '08b4dae1-3eef-4509-b1e6-869379400009', FALSE, FALSE, 'report.ItemsMarkersQuantities-Show', 'report Show ItemsMarkersQuantities', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400010', 'Performs the Show action on the ItemsMovements Report', '08b4dae1-3eef-4509-b1e6-869379400010', FALSE, FALSE, 'report.ItemsMovements-Show', 'report Show ItemsMovements', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400011', 'Performs the Show action on the RepresentativeItemsMovements Report', '08b4dae1-3eef-4509-b1e6-869379400011', FALSE, FALSE, 'report.RepresentativeItemsMovements-Show', 'report Show RepresentativeItemsMovements', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400012', 'Performs the Show action on the MissedSerial Report', '08b4dae1-3eef-4509-b1e6-869379400012', FALSE, FALSE, 'report.MissedSerial-Show', 'report Show MissedSerial', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400013', 'Performs the Show action on the SalesItemsProfit2 Report', '08b4dae1-3eef-4509-b1e6-869379400013', FALSE, FALSE, 'report.SalesItemsProfit2-Show', 'report Show SalesItemsProfit2', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400014', 'Performs the Show action on the SalesCollectionInsights Report', '08b4dae1-3eef-4509-b1e6-869379400014', FALSE, FALSE, 'report.SalesCollectionInsights-Show', 'report Show SalesCollectionInsights', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400015', 'Performs the Show action on the MaxDiscountItemsBased Report', '08b4dae1-3eef-4509-b1e6-869379400015', FALSE, FALSE, 'report.MaxDiscountItemsBased-Show', 'report Show MaxDiscountItemsBased', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400016', 'Performs the Show action on the DiscountItemsBased Report', '08b4dae1-3eef-4509-b1e6-869379400016', FALSE, FALSE, 'report.DiscountItemsBased-Show', 'report Show DiscountItemsBased', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400017', 'Performs the Show action on the RepresentativeAmounts Report', '08b4dae1-3eef-4509-b1e6-869379400017', FALSE, FALSE, 'report.RepresentativeAmounts-Show', 'report Show RepresentativeAmounts', NULL, '00000000-0000-0000-0000-000000000000', ''),
('08b4dae1-3eef-4509-b1e6-869379400018', 'Performs the Show action on the FinancialHestory Report', '08b4dae1-3eef-4509-b1e6-869379400018', FALSE, FALSE, 'report.FinancialHestory-Show', 'report Show FinancialHestory', NULL, '00000000-0000-0000-0000-000000000000', '');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241123115046_2024.11.23-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TPurchaseInvoice` ADD `IsCostOnSupplier` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241129191847_2024.11.29-001', '8.0.11');

COMMIT;

START TRANSACTION;

CREATE TABLE `TCompanyInfo` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Type` longtext CHARACTER SET utf8mb4 NULL,
    `Address` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber1` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber2` longtext CHARACTER SET utf8mb4 NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `InvoiceNotice` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TCompanyInfo` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `TCompanyInfo` (`Id`, `Address`, `Email`, `Guid`, `InvoiceNotice`, `IsDeleted`, `Name`, `PhoneNumber1`, `PhoneNumber2`, `TenantId`, `Type`)
VALUES ('81e1b645-a306-404d-aff0-361fe9fde041', 'بلعون، بنغازي', 'almmtech3@gmail.com', '81e1b645-a306-404d-aff0-361fe9fde042', 'تنبيهات الفواتير', FALSE, 'المنظومة المتجددة', '+218911919187', '+218911919187', '81e1b645-a306-404d-aff0-361fe9fde043', 'لققنية المعلومات');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241208030622_2024.12.08-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TPurchaseInvoice` MODIFY COLUMN `SupplierId` char(36) COLLATE ascii_general_ci NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241208184501_2024.12.08-003', '8.0.11');

COMMIT;

START TRANSACTION;

CREATE TABLE `TEmployeeSalaryCountingPermit` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Number` int NOT NULL,
    `Total` decimal(12,2) NOT NULL,
    `TotalDebit` decimal(12,2) NOT NULL,
    `TotalCredit` decimal(12,2) NOT NULL,
    `WorkTime` datetime(6) NOT NULL,
    `WorkMounth` int NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TEmployeeSalaryCountingPermit` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `TEmployeeSalaryCounting` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `EmployeeSalaryCountingPermitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Total` decimal(12,2) NOT NULL,
    `Salaty` decimal(12,2) NOT NULL,
    `TotalDebit` decimal(12,2) NOT NULL,
    `TotalCredit` decimal(12,2) NOT NULL,
    `EmployeeId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TEmployeeSalaryCounting` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TEmployeeSalaryCounting_TEmployeeSalaryCountingPermit_Employ~` FOREIGN KEY (`EmployeeSalaryCountingPermitId`) REFERENCES `TEmployeeSalaryCountingPermit` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_TEmployeeSalaryCounting_TEmployee_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `TEmployee` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_TEmployeeSalaryCounting_EmployeeId` ON `TEmployeeSalaryCounting` (`EmployeeId`);

CREATE INDEX `IX_TEmployeeSalaryCounting_EmployeeSalaryCountingPermitId` ON `TEmployeeSalaryCounting` (`EmployeeSalaryCountingPermitId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241212120309_2024.12.12-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TEmployeeSalaryCountingPermit` MODIFY COLUMN `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6);

CREATE TABLE `TPurchaseReturnInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Date` datetime(6) NOT NULL,
    `SerialNumber` bigint NOT NULL,
    `Number` bigint NOT NULL,
    `SupplierId` char(36) COLLATE ascii_general_ci NULL,
    `InvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NULL,
    `PeymentType` int NOT NULL,
    `SafeId` char(36) COLLATE ascii_general_ci NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TPurchaseReturnInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TPurchaseReturnInvoice_TPurchaseInvoice_InvoiceId` FOREIGN KEY (`InvoiceId`) REFERENCES `TPurchaseInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseReturnInvoice_TSafe_SafeId` FOREIGN KEY (`SafeId`) REFERENCES `TSafe` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseReturnInvoice_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseReturnInvoice_TSupplier_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `TSupplier` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TPurchaseReturnInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `ReturnInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Price` decimal(12,2) NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TPurchaseReturnInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TPurchaseReturnInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseReturnInvoiceDetail_TPurchaseReturnInvoice_ReturnIn~` FOREIGN KEY (`ReturnInvoiceId`) REFERENCES `TPurchaseReturnInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TPurchaseReturnInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_TPurchaseReturnInvoice_InvoiceId` ON `TPurchaseReturnInvoice` (`InvoiceId`);

CREATE INDEX `IX_TPurchaseReturnInvoice_SafeId` ON `TPurchaseReturnInvoice` (`SafeId`);

CREATE INDEX `IX_TPurchaseReturnInvoice_StoreId` ON `TPurchaseReturnInvoice` (`StoreId`);

CREATE INDEX `IX_TPurchaseReturnInvoice_SupplierId` ON `TPurchaseReturnInvoice` (`SupplierId`);

CREATE INDEX `IX_TPurchaseReturnInvoiceDetail_ItemId` ON `TPurchaseReturnInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TPurchaseReturnInvoiceDetail_ReturnInvoiceId` ON `TPurchaseReturnInvoiceDetail` (`ReturnInvoiceId`);

CREATE INDEX `IX_TPurchaseReturnInvoiceDetail_UnitId` ON `TPurchaseReturnInvoiceDetail` (`UnitId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241214122548_2024.12.14-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TPurchaseInvoice` ADD `SerialNumber` bigint NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241214124432_2024.12.14-002', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TPurchaseReturnInvoice` MODIFY COLUMN `InvoiceId` char(36) COLLATE ascii_general_ci NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241227125153_2024.12.27-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TSellReturnInvoice` MODIFY COLUMN `InvoiceId` char(36) COLLATE ascii_general_ci NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250106150926_2025.01.06-001', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TEntry` ADD `Tag` int NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250217080541_2025.02.17-001-addTagInEntry', '8.0.11');

COMMIT;

START TRANSACTION;

CREATE TABLE `TDamagedInvoice` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Number` bigint NOT NULL,
    `Date` datetime(6) NOT NULL,
    `SerialNumber` bigint NOT NULL,
    `TotalCost` decimal(12,2) NOT NULL,
    `TotalAmount` decimal(12,2) NOT NULL,
    `MarketerStoreId` char(36) COLLATE ascii_general_ci NULL,
    `StoreId` char(36) COLLATE ascii_general_ci NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDamagedInvoice` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TDamagedInvoice_TMarketer_MarketerStoreId` FOREIGN KEY (`MarketerStoreId`) REFERENCES `TMarketer` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDamagedInvoice_TStore_StoreId` FOREIGN KEY (`StoreId`) REFERENCES `TStore` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `TDamagedInvoiceDetail` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `DamagedInvoiceId` char(36) COLLATE ascii_general_ci NOT NULL,
    `UnitId` char(36) COLLATE ascii_general_ci NOT NULL,
    `ItemId` char(36) COLLATE ascii_general_ci NOT NULL,
    `Quantity` decimal(12,2) NOT NULL,
    `ExpireDate` datetime(6) NULL,
    `TDamagedInvoiceId` char(36) COLLATE ascii_general_ci NULL,
    `Guid` char(36) COLLATE ascii_general_ci NOT NULL,
    `CreateDate` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `IsDeleted` tinyint(1) NOT NULL,
    `TenantId` char(36) COLLATE ascii_general_ci NOT NULL,
    CONSTRAINT `PK_TDamagedInvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TDamagedInvoiceDetail_TDamagedInvoice_DamagedInvoiceId` FOREIGN KEY (`DamagedInvoiceId`) REFERENCES `TDamagedInvoice` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDamagedInvoiceDetail_TDamagedInvoice_TDamagedInvoiceId` FOREIGN KEY (`TDamagedInvoiceId`) REFERENCES `TDamagedInvoice` (`Id`),
    CONSTRAINT `FK_TDamagedInvoiceDetail_TItem_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `TItem` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TDamagedInvoiceDetail_TUnit_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `TUnit` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_TDamagedInvoice_MarketerStoreId` ON `TDamagedInvoice` (`MarketerStoreId`);

CREATE INDEX `IX_TDamagedInvoice_StoreId` ON `TDamagedInvoice` (`StoreId`);

CREATE INDEX `IX_TDamagedInvoiceDetail_DamagedInvoiceId` ON `TDamagedInvoiceDetail` (`DamagedInvoiceId`);

CREATE INDEX `IX_TDamagedInvoiceDetail_ItemId` ON `TDamagedInvoiceDetail` (`ItemId`);

CREATE INDEX `IX_TDamagedInvoiceDetail_TDamagedInvoiceId` ON `TDamagedInvoiceDetail` (`TDamagedInvoiceId`);

CREATE INDEX `IX_TDamagedInvoiceDetail_UnitId` ON `TDamagedInvoiceDetail` (`UnitId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250217173104_2025.02.17-005-AddDamagedInvoice', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE `TDamagedInvoiceDetail` DROP FOREIGN KEY `FK_TDamagedInvoiceDetail_TDamagedInvoice_TDamagedInvoiceId`;

ALTER TABLE `TDamagedInvoiceDetail` DROP INDEX `IX_TDamagedInvoiceDetail_TDamagedInvoiceId`;

ALTER TABLE `TDamagedInvoiceDetail` DROP COLUMN `TDamagedInvoiceId`;

ALTER TABLE `TSalesInvoice` ADD `DiscountAmount` decimal(12,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `TSalesInvoice` ADD `NetAmount` decimal(12,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `TPurchaseInvoice` ADD `DiscountAmount` decimal(12,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `TPurchaseInvoice` ADD `NetAmount` decimal(12,2) NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250309180918_2025.03.09-005-add-netamount_discountAmount', '8.0.11');

COMMIT;

START TRANSACTION;



-- Create a stored procedure to handle the logic
CREATE PROCEDURE RecalculateInvoiceAmounts()
BEGIN

	-- XX
	-- CREATE TEMPORARY TABLE TempResults (
    --     InvoiceId CHAR(36),
    --     TotalAmount DECIMAL(18, 2),
    --     ItemDiscountAmount DECIMAL(18, 2),
    --     InvoiceDiscountAmount DECIMAL(18, 2),
    --     NetAmount DECIMAL(18, 2)
    -- );

    -- Declare variables to hold the calculated amounts
    DECLARE totalAmount DECIMAL(18, 2);
    DECLARE itemDiscountAmount DECIMAL(18, 2);
    DECLARE invoiceDiscountAmount DECIMAL(18, 2);
    DECLARE netAmount DECIMAL(18, 2);

    -- Declare variables to hold the current invoice details
    DECLARE currentInvoiceId char(36);
    DECLARE currentInvoiceDiscount DECIMAL(18, 2);
    DECLARE currentInvoiceDiscountType VARCHAR(50);
	 	 
    -- Declare a flag to check if the cursor has more rows
    DECLARE done INT DEFAULT FALSE;
	 	 
	-- Declare a cursor to loop through all rows in TSalesInvoice
    DECLARE invoiceCursor CURSOR FOR
    SELECT Id, Discount, DiscountType FROM TSalesInvoice;

    -- Declare a handler for when the cursor has no more rows
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    -- Open the cursor
    OPEN invoiceCursor;

    -- Start looping through the invoices
    invoiceLoop: LOOP
        -- Fetch the next invoice details
        FETCH invoiceCursor INTO currentInvoiceId, currentInvoiceDiscount, currentInvoiceDiscountType;

        -- Exit the loop if no more rows
        IF done THEN
            LEAVE invoiceLoop;
        END IF;

        -- Calculate the TotalAmount and item-level DiscountAmount for the current invoice
        SELECT 
            SUM(d.Price * d.Quantity) AS TotalAmount,
            SUM(d.Discount) AS ItemDiscountAmount
        INTO 
            totalAmount, itemDiscountAmount
        FROM 
            TSalesInvoiceDetail d
        WHERE 
            d.SalesInvoiceId = currentInvoiceId AND (NOT d.IsDeleted);

        -- Calculate the invoice-level discount based on DiscountType
        IF currentInvoiceDiscountType = 1 THEN
            -- If discount is a percentage, calculate it as a percentage of the total amount
            SET invoiceDiscountAmount = (totalAmount - itemDiscountAmount) * (currentInvoiceDiscount / 100);
        ELSE
            -- If discount is static, use the discount value directly
            SET invoiceDiscountAmount = currentInvoiceDiscount;
        END IF;

        -- Calculate the NetAmount after applying both item-level and invoice-level discounts
        SET netAmount = (totalAmount - itemDiscountAmount) - invoiceDiscountAmount;

        -- Update the TSalesInvoice table with the calculated amounts
        UPDATE TSalesInvoice
        SET 
            TotalAmount = IFNULL(totalAmount,0),
            DiscountAmount = IFNULL(invoiceDiscountAmount,0),
            NetAmount = IFNULL(netAmount,0)
        WHERE 
            Id = currentInvoiceId;

	    -- INSERT INTO TempResults (InvoiceId, TotalAmount, ItemDiscountAmount, InvoiceDiscountAmount, NetAmount)
   	    -- VALUES (currentInvoiceId, totalAmount, itemDiscountAmount, invoiceDiscountAmount, netAmount);
    END LOOP;

    -- Close the cursor
    CLOSE invoiceCursor;
   
	  -- SELECT * FROM TempResults;
      --   DROP TEMPORARY TABLE IF EXISTS TempResults;
END


-- Call the stored procedure to execute the logic
-- CALL RecalculateInvoiceAmounts();
-- DROP PROCEDURE IF EXISTS RecalculateInvoiceAmounts;



INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250311082834_2025.03.11-001', '8.0.11');

COMMIT;

START TRANSACTION;

 UPDATE tinventory as x SET	x.Quantity = -x.Quantity, x.TotalQuantity=-x.TotalQuantity WHERE x.Quantity < 0

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250316082815_2025.03.16-fixMinusQuantityCrediting-', '8.0.11');

COMMIT;

