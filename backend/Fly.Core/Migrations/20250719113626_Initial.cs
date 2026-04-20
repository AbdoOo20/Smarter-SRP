using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fly.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OpeningDebtor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    OpeningCreditor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CurrencyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Natural = table.Column<int>(type: "int", nullable: false),
                    AccountCat = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TCategory_TCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TCompanyInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber1 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber2 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvoiceNotice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCompanyInfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DiscountAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneralShareAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarketerInvoicesAccountId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalesInvoicesAccountId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneralSupplierAccountAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GeneralClientAccountAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchaseInvoicesAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaintenanceSectorsAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalMaintenanceSectorsAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManufacturingFacilitiesAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalManufacturingFacilitiesAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SafeAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientsAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplyersAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeesAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpenseAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssetsAccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TConfig", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TCostCenter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCostCenter", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TCurrency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsPrimary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TransfereRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Letter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCurrency", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDepartment", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEmployeeSalaryCountingPermit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalDebit = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    WorkTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WorkMounth = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEmployeeSalaryCountingPermit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OprationType = table.Column<int>(type: "int", nullable: true),
                    RelatedIndex = table.Column<int>(type: "int", nullable: true),
                    RelatedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DisplayNumber = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: true),
                    Tag = table.Column<int>(type: "int", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEntry", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TExpenseTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TItemVariance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItemVariance", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TRoleGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRoleGroup", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TStore", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUnit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepreciationRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TAssets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TBank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBank_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaxDebt = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TClient_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMaintenanceSector",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Percentage = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OpeningDebtor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    OpeningCreditor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMaintenanceSector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMaintenanceSector_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TManufacturingFacility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Percentage = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OpeningDebtor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    OpeningCreditor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TManufacturingFacility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TManufacturingFacility_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Percentage = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    IsRepresentative = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OpeningDebtor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    OpeningCreditor = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketer_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSafe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PeymentMode = table.Column<int>(type: "int", nullable: false),
                    LossPercent = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSafe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSafe_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSupplier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSupplier_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Photo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Barcode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TItem_TCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TItemComposition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItemComposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TItemComposition_TCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Identifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DepartmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TEmployee_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TEmployee_TDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "TDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEntryDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EntryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Debit = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEntryDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TEntryDetail_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TEntryDetail_TEntry_EntryId",
                        column: x => x.EntryId,
                        principalTable: "TEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Accesser = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sult = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegesterTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsGhost = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRedy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoleGroupId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TUser_TRoleGroup_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalTable: "TRoleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDisbursementPermit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CostCenterId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDisbursementPermit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TDisbursementPermit_TCostCenter_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "TCostCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDisbursementPermit_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TQuantityCreditingInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TQuantityCreditingInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TQuantityCreditingInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TBankBranch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBankBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBankBranch_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TBankBranch_TBank_BankId",
                        column: x => x.BankId,
                        principalTable: "TBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDamagedInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    MarketerStoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDamagedInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TDamagedInvoice_TMarketer_MarketerStoreId",
                        column: x => x.MarketerStoreId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDamagedInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MarketerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerClient_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerClient_TMarketer_MarketerId",
                        column: x => x.MarketerId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerDisbursementPermit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    MarketerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerDisbursementPermit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerDisbursementPermit_TMarketer_MarketerId",
                        column: x => x.MarketerId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerDisbursementPermit_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerSalesInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MarketerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    PeymentType = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerSalesInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerSalesInvoice_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerSalesInvoice_TMarketer_MarketerId",
                        column: x => x.MarketerId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerSharedEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MarketerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EntryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SharedAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerSharedEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerSharedEntry_TEntry_EntryId",
                        column: x => x.EntryId,
                        principalTable: "TEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerSharedEntry_TMarketer_MarketerId",
                        column: x => x.MarketerId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TFactoringInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsInternal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SafeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ManufacturingFacilityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CollectionRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    FactoringStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFactoringInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoice_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoice_TManufacturingFacility_ManufacturingFacili~",
                        column: x => x.ManufacturingFacilityId,
                        principalTable: "TManufacturingFacility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoice_TSafe_SafeId",
                        column: x => x.SafeId,
                        principalTable: "TSafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMaintenanceInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsInternal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SafeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CollectionRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    MaintenanceStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMaintenanceInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoice_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoice_TSafe_SafeId",
                        column: x => x.SafeId,
                        principalTable: "TSafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSalesInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RepresentativeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RepresentativePercentage = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MarketerStorageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StorageType = table.Column<int>(type: "int", nullable: false),
                    SafeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    PeymentType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NetAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSalesInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSalesInvoice_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSalesInvoice_TMarketer_MarketerStorageId",
                        column: x => x.MarketerStorageId,
                        principalTable: "TMarketer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TSalesInvoice_TMarketer_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSalesInvoice_TSafe_SafeId",
                        column: x => x.SafeId,
                        principalTable: "TSafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSalesInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TPurchaseInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SafeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PeymentType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCostOnSupplier = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPurchaseInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoice_TSafe_SafeId",
                        column: x => x.SafeId,
                        principalTable: "TSafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoice_TSupplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "TSupplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSupplyPermit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SupplierId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSupplyPermit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSupplyPermit_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSupplyPermit_TSupplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "TSupplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDiscountitedItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Ammount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalQuantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReleatedId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDiscountitedItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TDiscountitedItem_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDiscountitedItem_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDiscountitedItem_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<int>(type: "int", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ingress = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalQuantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RelatedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RelatedIndex = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TInventory_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TInventory_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TInventory_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TItemUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsPrimary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TransferRate = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItemUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TItemUnit_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TItemUnit_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MarketerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<int>(type: "int", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ingress = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalQuantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RelatedId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RelatedIndex = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerInventory_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerInventory_TMarketer_MarketerId",
                        column: x => x.MarketerId,
                        principalTable: "TMarketer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerInventory_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TItemCompositionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemCompositionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemVarianceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TItemCompositionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TItemCompositionDetail_TItemComposition_ItemCompositionId",
                        column: x => x.ItemCompositionId,
                        principalTable: "TItemComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TItemCompositionDetail_TItemVariance_ItemVarianceId",
                        column: x => x.ItemVarianceId,
                        principalTable: "TItemVariance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TItemCompositionDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TItemCompositionDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TAttendanceDeparture",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAttendanceDeparture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TAttendanceDeparture_TEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "TEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDiscountIncrease",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDiscountIncrease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TDiscountIncrease_TEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "TEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEmployeeAssignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    PeriodTime = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssignmentsType = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AssignmentsMoney = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEmployeeAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TEmployeeAssignment_TEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "TEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TEmployeeSalaryCounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EmployeeSalaryCountingPermitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Total = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Salaty = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalDebit = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEmployeeSalaryCounting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TEmployeeSalaryCounting_TEmployeeSalaryCountingPermit_Employ~",
                        column: x => x.EmployeeSalaryCountingPermitId,
                        principalTable: "TEmployeeSalaryCountingPermit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TEmployeeSalaryCounting_TEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "TEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Key = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Windowname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSecret = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TRole_TUser_TUserId",
                        column: x => x.TUserId,
                        principalTable: "TUser",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDisbursementPermitDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DisbursementPermitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDisbursementPermitDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TDisbursementPermitDetail_TDisbursementPermit_DisbursementPe~",
                        column: x => x.DisbursementPermitId,
                        principalTable: "TDisbursementPermit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDisbursementPermitDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDisbursementPermitDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TQuantityCreditingInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuantityCreditingInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TQuantityCreditingInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TQuantityCreditingInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TQuantityCreditingInvoiceDetail_TQuantityCreditingInvoice_Qu~",
                        column: x => x.QuantityCreditingInvoiceId,
                        principalTable: "TQuantityCreditingInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TQuantityCreditingInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TDamagedInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DamagedInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDamagedInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TDamagedInvoiceDetail_TDamagedInvoice_DamagedInvoiceId",
                        column: x => x.DamagedInvoiceId,
                        principalTable: "TDamagedInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDamagedInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TDamagedInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerDisbursementPermitDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MarketerDisbursementPermitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerDisbursementPermitDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerDisbursementPermitDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerDisbursementPermitDetail_TMarketerDisbursementPermi~",
                        column: x => x.MarketerDisbursementPermitId,
                        principalTable: "TMarketerDisbursementPermit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerDisbursementPermitDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMarketerSalesInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MarketerSalesInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMarketerSalesInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMarketerSalesInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerSalesInvoiceDetail_TMarketerSalesInvoice_MarketerSa~",
                        column: x => x.MarketerSalesInvoiceId,
                        principalTable: "TMarketerSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMarketerSalesInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TFactoringInvoiceComposition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FactoringInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemCompositionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TFactoringInvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFactoringInvoiceComposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceComposition_TFactoringInvoice_FactoringInvo~",
                        column: x => x.FactoringInvoiceId,
                        principalTable: "TFactoringInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceComposition_TFactoringInvoice_TFactoringInv~",
                        column: x => x.TFactoringInvoiceId,
                        principalTable: "TFactoringInvoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceComposition_TItem_ItemCompositionId",
                        column: x => x.ItemCompositionId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TFactoringInvoiceCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FactoringInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CostName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TFactoringInvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFactoringInvoiceCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceCost_TFactoringInvoice_FactoringInvoiceId",
                        column: x => x.FactoringInvoiceId,
                        principalTable: "TFactoringInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceCost_TFactoringInvoice_TFactoringInvoiceId",
                        column: x => x.TFactoringInvoiceId,
                        principalTable: "TFactoringInvoice",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TFactoringInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FactoringInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TFactoringInvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFactoringInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceDetail_TFactoringInvoice_FactoringInvoiceId",
                        column: x => x.FactoringInvoiceId,
                        principalTable: "TFactoringInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceDetail_TFactoringInvoice_TFactoringInvoiceId",
                        column: x => x.TFactoringInvoiceId,
                        principalTable: "TFactoringInvoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TFactoringInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMaintenanceInvoiceCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaintenanceInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CostName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TMaintenanceInvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMaintenanceInvoiceCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceCost_TMaintenanceInvoice_MaintenanceInvoi~",
                        column: x => x.MaintenanceInvoiceId,
                        principalTable: "TMaintenanceInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceCost_TMaintenanceInvoice_TMaintenanceInvo~",
                        column: x => x.TMaintenanceInvoiceId,
                        principalTable: "TMaintenanceInvoice",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TMaintenanceInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaintenanceInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemVarianceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    TMaintenanceInvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMaintenanceInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceDetail_TItemVariance_ItemVarianceId",
                        column: x => x.ItemVarianceId,
                        principalTable: "TItemVariance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceDetail_TMaintenanceInvoice_MaintenanceInv~",
                        column: x => x.MaintenanceInvoiceId,
                        principalTable: "TMaintenanceInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceDetail_TMaintenanceInvoice_TMaintenanceIn~",
                        column: x => x.TMaintenanceInvoiceId,
                        principalTable: "TMaintenanceInvoice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TMaintenanceInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSalesInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SalesInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSalesInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSalesInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSalesInvoiceDetail_TSalesInvoice_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "TSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSalesInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSellReturnInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PeymentType = table.Column<int>(type: "int", nullable: false),
                    SafeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSellReturnInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoice_TClient_ClientId",
                        column: x => x.ClientId,
                        principalTable: "TClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoice_TSafe_SafeId",
                        column: x => x.SafeId,
                        principalTable: "TSafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoice_TSalesInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TPurchaseInvoiceCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PurchaseInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ammount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPurchaseInvoiceCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoiceCost_TPurchaseInvoice_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "TPurchaseInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TPurchaseInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PurchaseInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPurchaseInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoiceDetail_TPurchaseInvoice_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "TPurchaseInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TPurchaseReturnInvoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InvoiceId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    StoreId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PeymentType = table.Column<int>(type: "int", nullable: false),
                    SafeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPurchaseReturnInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoice_TPurchaseInvoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TPurchaseInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoice_TSafe_SafeId",
                        column: x => x.SafeId,
                        principalTable: "TSafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoice_TStore_StoreId",
                        column: x => x.StoreId,
                        principalTable: "TStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoice_TSupplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "TSupplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSupplyPermitDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SupplyPermitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSupplyPermitDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSupplyPermitDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSupplyPermitDetail_TSupplyPermit_SupplyPermitId",
                        column: x => x.SupplyPermitId,
                        principalTable: "TSupplyPermit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSupplyPermitDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TInventoryCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsReturn = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    AvrageCost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    AvragePiceCost = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    SellPrice = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: true),
                    TInventoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TInventoryCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TInventoryCost_TInventory_TInventoryId",
                        column: x => x.TInventoryId,
                        principalTable: "TInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TRoleGroupDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRoleGroupDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TRoleGroupDetail_TRoleGroup_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalTable: "TRoleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TRoleGroupDetail_TRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TSellReturnInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReturnInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSellReturnInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoiceDetail_TSellReturnInvoice_ReturnInvoiceId",
                        column: x => x.ReturnInvoiceId,
                        principalTable: "TSellReturnInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TSellReturnInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TPurchaseReturnInvoiceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReturnInvoiceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPurchaseReturnInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoiceDetail_TItem_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoiceDetail_TPurchaseReturnInvoice_ReturnIn~",
                        column: x => x.ReturnInvoiceId,
                        principalTable: "TPurchaseReturnInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TPurchaseReturnInvoiceDetail_TUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCat", "CreateDate", "CurrencyId", "Guid", "IsDeleted", "Name", "Natural", "Number", "OpeningCreditor", "OpeningDebtor", "ParentId", "TenantId" },
                values: new object[,]
                {
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c65185f"), 3, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مصروفات", 0, 5L, 0m, 0m, null, new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("1c63e7c0-033d-435b-b55f-4a37cb6ab77c"), 2, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "إيرادات", 1, 4L, 0m, 0m, null, new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("56844955-106c-4f9f-8f07-b8e33b3c2812"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "حقوق الملكية", 1, 3L, 0m, 0m, null, new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("83a0c584-5013-4186-a337-2985d38b6544"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "خصوم", 1, 2L, 0m, 0m, null, new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("9002ea94-02ff-47f0-907c-00a8360c440b"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أصول", 0, 1L, 0m, 0m, null, new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") }
                });

            migrationBuilder.InsertData(
                table: "TCompanyInfo",
                columns: new[] { "Id", "Address", "Email", "Guid", "InvoiceNotice", "IsDeleted", "Name", "PhoneNumber1", "PhoneNumber2", "TenantId", "Type" },
                values: new object[] { new Guid("81e1b645-a306-404d-aff0-361fe9fde041"), "بلعون، بنغازي", "almmtech3@gmail.com", new Guid("81e1b645-a306-404d-aff0-361fe9fde042"), "تنبيهات الفواتير", false, "المنظومة المتجددة", "+218911919187", "+218911919187", new Guid("81e1b645-a306-404d-aff0-361fe9fde043"), "لققنية المعلومات" });

            migrationBuilder.InsertData(
                table: "TConfig",
                columns: new[] { "Id", "AssetsAccount", "BankAccount", "ClientsAccount", "DiscountAccount", "EmployeesAccount", "ExpenseAccount", "GeneralClientAccountAccount", "GeneralShareAccount", "GeneralSupplierAccountAccount", "Guid", "InternalMaintenanceSectorsAccount", "InternalManufacturingFacilitiesAccount", "IsDeleted", "MaintenanceSectorsAccount", "ManufacturingFacilitiesAccount", "MarketerInvoicesAccountId", "PurchaseInvoicesAccount", "SafeAccount", "SalesInvoicesAccountId", "SupplyersAccount", "TenantId" },
                values: new object[] { new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da1"), "ad630d24-f791-4d5a-8104-27595e273112", "adecdcbe-5c66-4069-9f41-ee29aa4a6da2", "adecdcbe-5c66-4069-9f41-ee29aa4a6dc2", "0e54960c-8a5b-473d-8022-c8847c651104", "13440fba-02cd-4f4b-843b-6159611c3fac", "0e54960c-8a5b-473d-8022-c4a47c651001", "adecdcbe-5c66-4069-9f41-ee29aa4a6dc3", "0e54960c-8a5b-473d-8022-c8847c651103", "2b5ee402-49f1-444c-9a86-bf76e0ecbf91", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0"), "dbcb6851-213c-4238-a2a9-46f0d359c878", "dbcb6851-213c-4238-a2a9-46f0d359c878", false, "0e54960c-8a5b-473d-8022-c4a47c651105", "0e54960c-8a5b-473d-8022-c4a47c651106", "0e54960c-8a5b-473d-8022-c4a47c651103", "0e54960c-8a5b-473d-8022-c4a47c651002", "adecdcbe-5c66-4069-9f41-ee29aa4a6da1", "0e54960c-8a5b-473d-8022-c4a47c651102", "2b5ee402-49f1-444c-9a86-bf76e0ecbf90", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1") });

            migrationBuilder.InsertData(
                table: "TCurrency",
                columns: new[] { "Id", "CreateDate", "Guid", "IsDeleted", "IsPrimary", "Letter", "Name", "TenantId", "TransfereRate" },
                values: new object[] { new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new DateTime(2024, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("14ebdb67-1d7d-41e2-b766-dee66415b487"), false, true, "LYD", "دينار ليبي", new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a8f"), 0m });

            migrationBuilder.InsertData(
                table: "TRole",
                columns: new[] { "Id", "Description", "Guid", "IsDeleted", "IsSecret", "Key", "Name", "TUserId", "TenantId", "Windowname" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00001"), "Performs the Show action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00001"), false, false, "Account.Show", "Show Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00002"), "Performs the Create action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00002"), false, false, "Account.Create", "Create Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00003"), "Performs the Update action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00003"), false, false, "Account.Update", "Update Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00004"), "Performs the Delete action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00004"), false, false, "Account.Delete", "Delete Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00005"), "Performs the Export action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00005"), false, false, "Account.Export", "Export Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00006"), "Performs the Import action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00006"), false, false, "Account.Import", "Import Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00007"), "Performs the Print action on the Account entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00007"), false, false, "Account.Print", "Print Account", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00008"), "Performs the Show action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00008"), false, false, "ExpenseTypes.Show", "Show ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00009"), "Performs the Create action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00009"), false, false, "ExpenseTypes.Create", "Create ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00010"), "Performs the Update action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00010"), false, false, "ExpenseTypes.Update", "Update ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00011"), "Performs the Delete action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00011"), false, false, "ExpenseTypes.Delete", "Delete ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00012"), "Performs the Export action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00012"), false, false, "ExpenseTypes.Export", "Export ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00013"), "Performs the Import action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00013"), false, false, "ExpenseTypes.Import", "Import ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00014"), "Performs the Print action on the ExpenseTypes entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00014"), false, false, "ExpenseTypes.Print", "Print ExpenseTypes", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00015"), "Performs the Show action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00015"), false, false, "Entry.Show", "Show Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00016"), "Performs the Create action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00016"), false, false, "Entry.Create", "Create Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00017"), "Performs the Update action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00017"), false, false, "Entry.Update", "Update Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00018"), "Performs the Delete action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00018"), false, false, "Entry.Delete", "Delete Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00019"), "Performs the Export action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00019"), false, false, "Entry.Export", "Export Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00020"), "Performs the Import action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00020"), false, false, "Entry.Import", "Import Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00021"), "Performs the Print action on the Entry entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00021"), false, false, "Entry.Print", "Print Entry", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00022"), "Performs the Show action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00022"), false, false, "Assets.Show", "Show Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00023"), "Performs the Create action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00023"), false, false, "Assets.Create", "Create Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00024"), "Performs the Update action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00024"), false, false, "Assets.Update", "Update Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00025"), "Performs the Delete action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00025"), false, false, "Assets.Delete", "Delete Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00026"), "Performs the Export action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00026"), false, false, "Assets.Export", "Export Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00027"), "Performs the Import action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00027"), false, false, "Assets.Import", "Import Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00028"), "Performs the Print action on the Assets entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00028"), false, false, "Assets.Print", "Print Assets", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00029"), "Performs the Show action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00029"), false, false, "Employee.Show", "Show Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00030"), "Performs the Create action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00030"), false, false, "Employee.Create", "Create Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00031"), "Performs the Update action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00031"), false, false, "Employee.Update", "Update Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00032"), "Performs the Delete action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00032"), false, false, "Employee.Delete", "Delete Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00033"), "Performs the Export action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00033"), false, false, "Employee.Export", "Export Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00034"), "Performs the Import action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00034"), false, false, "Employee.Import", "Import Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00035"), "Performs the Print action on the Employee entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00035"), false, false, "Employee.Print", "Print Employee", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00036"), "Performs the Show action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00036"), false, false, "EmployeeAssignment.Show", "Show EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00037"), "Performs the Create action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00037"), false, false, "EmployeeAssignment.Create", "Create EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00038"), "Performs the Update action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00038"), false, false, "EmployeeAssignment.Update", "Update EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00039"), "Performs the Delete action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00039"), false, false, "EmployeeAssignment.Delete", "Delete EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00040"), "Performs the Export action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00040"), false, false, "EmployeeAssignment.Export", "Export EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00041"), "Performs the Import action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00041"), false, false, "EmployeeAssignment.Import", "Import EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00042"), "Performs the Print action on the EmployeeAssignment entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00042"), false, false, "EmployeeAssignment.Print", "Print EmployeeAssignment", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00043"), "Performs the Show action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00043"), false, false, "Department.Show", "Show Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00044"), "Performs the Create action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00044"), false, false, "Department.Create", "Create Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00045"), "Performs the Update action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00045"), false, false, "Department.Update", "Update Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00046"), "Performs the Delete action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00046"), false, false, "Department.Delete", "Delete Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00047"), "Performs the Export action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00047"), false, false, "Department.Export", "Export Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00048"), "Performs the Import action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00048"), false, false, "Department.Import", "Import Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00049"), "Performs the Print action on the Department entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00049"), false, false, "Department.Print", "Print Department", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00050"), "Performs the Show action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00050"), false, false, "DiscountIncrease.Show", "Show DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00051"), "Performs the Create action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00051"), false, false, "DiscountIncrease.Create", "Create DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00052"), "Performs the Update action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00052"), false, false, "DiscountIncrease.Update", "Update DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00053"), "Performs the Delete action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00053"), false, false, "DiscountIncrease.Delete", "Delete DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00054"), "Performs the Export action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00054"), false, false, "DiscountIncrease.Export", "Export DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00055"), "Performs the Import action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00055"), false, false, "DiscountIncrease.Import", "Import DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00056"), "Performs the Print action on the DiscountIncrease entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00056"), false, false, "DiscountIncrease.Print", "Print DiscountIncrease", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00057"), "Performs the Show action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00057"), false, false, "Category.Show", "Show Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00058"), "Performs the Create action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00058"), false, false, "Category.Create", "Create Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00059"), "Performs the Update action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00059"), false, false, "Category.Update", "Update Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00060"), "Performs the Delete action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00060"), false, false, "Category.Delete", "Delete Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00061"), "Performs the Export action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00061"), false, false, "Category.Export", "Export Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00062"), "Performs the Import action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00062"), false, false, "Category.Import", "Import Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00063"), "Performs the Print action on the Category entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00063"), false, false, "Category.Print", "Print Category", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00064"), "Performs the Show action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00064"), false, false, "Store.Show", "Show Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00065"), "Performs the Create action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00065"), false, false, "Store.Create", "Create Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00066"), "Performs the Update action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00066"), false, false, "Store.Update", "Update Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00067"), "Performs the Delete action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00067"), false, false, "Store.Delete", "Delete Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00068"), "Performs the Export action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00068"), false, false, "Store.Export", "Export Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00069"), "Performs the Import action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00069"), false, false, "Store.Import", "Import Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00070"), "Performs the Print action on the Store entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00070"), false, false, "Store.Print", "Print Store", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00071"), "Performs the Show action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00071"), false, false, "CostCenter.Show", "Show CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00072"), "Performs the Create action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00072"), false, false, "CostCenter.Create", "Create CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00073"), "Performs the Update action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00073"), false, false, "CostCenter.Update", "Update CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00074"), "Performs the Delete action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00074"), false, false, "CostCenter.Delete", "Delete CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00075"), "Performs the Export action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00075"), false, false, "CostCenter.Export", "Export CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00076"), "Performs the Import action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00076"), false, false, "CostCenter.Import", "Import CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00077"), "Performs the Print action on the CostCenter entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00077"), false, false, "CostCenter.Print", "Print CostCenter", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00078"), "Performs the Show action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00078"), false, false, "Unit.Show", "Show Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00079"), "Performs the Create action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00079"), false, false, "Unit.Create", "Create Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00080"), "Performs the Update action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00080"), false, false, "Unit.Update", "Update Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00081"), "Performs the Delete action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00081"), false, false, "Unit.Delete", "Delete Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00082"), "Performs the Export action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00082"), false, false, "Unit.Export", "Export Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00083"), "Performs the Import action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00083"), false, false, "Unit.Import", "Import Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00084"), "Performs the Print action on the Unit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00084"), false, false, "Unit.Print", "Print Unit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00085"), "Performs the Show action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00085"), false, false, "Supplier.Show", "Show Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00086"), "Performs the Create action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00086"), false, false, "Supplier.Create", "Create Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00087"), "Performs the Update action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00087"), false, false, "Supplier.Update", "Update Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00088"), "Performs the Delete action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00088"), false, false, "Supplier.Delete", "Delete Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00089"), "Performs the Export action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00089"), false, false, "Supplier.Export", "Export Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00090"), "Performs the Import action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00090"), false, false, "Supplier.Import", "Import Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00091"), "Performs the Print action on the Supplier entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00091"), false, false, "Supplier.Print", "Print Supplier", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00092"), "Performs the Show action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00092"), false, false, "Item.Show", "Show Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00093"), "Performs the Create action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00093"), false, false, "Item.Create", "Create Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00094"), "Performs the Update action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00094"), false, false, "Item.Update", "Update Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00095"), "Performs the Delete action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00095"), false, false, "Item.Delete", "Delete Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00096"), "Performs the Export action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00096"), false, false, "Item.Export", "Export Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00097"), "Performs the Import action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00097"), false, false, "Item.Import", "Import Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00098"), "Performs the Print action on the Item entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00098"), false, false, "Item.Print", "Print Item", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00099"), "Performs the Show action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00099"), false, false, "Sales.Show", "Show Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00100"), "Performs the Create action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00100"), false, false, "Sales.Create", "Create Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00101"), "Performs the Update action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00101"), false, false, "Sales.Update", "Update Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00102"), "Performs the Delete action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00102"), false, false, "Sales.Delete", "Delete Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00103"), "Performs the Export action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00103"), false, false, "Sales.Export", "Export Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00104"), "Performs the Import action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00104"), false, false, "Sales.Import", "Import Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00105"), "Performs the Print action on the Sales entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00105"), false, false, "Sales.Print", "Print Sales", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00106"), "Performs the Show action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00106"), false, false, "Safe.Show", "Show Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00107"), "Performs the Create action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00107"), false, false, "Safe.Create", "Create Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00108"), "Performs the Update action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00108"), false, false, "Safe.Update", "Update Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00109"), "Performs the Delete action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00109"), false, false, "Safe.Delete", "Delete Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00110"), "Performs the Export action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00110"), false, false, "Safe.Export", "Export Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00111"), "Performs the Import action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00111"), false, false, "Safe.Import", "Import Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00112"), "Performs the Print action on the Safe entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00112"), false, false, "Safe.Print", "Print Safe", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00113"), "Performs the Show action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00113"), false, false, "Bank.Show", "Show Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00114"), "Performs the Create action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00114"), false, false, "Bank.Create", "Create Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00115"), "Performs the Update action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00115"), false, false, "Bank.Update", "Update Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00116"), "Performs the Delete action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00116"), false, false, "Bank.Delete", "Delete Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00117"), "Performs the Export action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00117"), false, false, "Bank.Export", "Export Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00118"), "Performs the Import action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00118"), false, false, "Bank.Import", "Import Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00119"), "Performs the Print action on the Bank entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00119"), false, false, "Bank.Print", "Print Bank", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00120"), "Performs the Show action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00120"), false, false, "BankBranch.Show", "Show BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00121"), "Performs the Create action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00121"), false, false, "BankBranch.Create", "Create BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00122"), "Performs the Update action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00122"), false, false, "BankBranch.Update", "Update BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00123"), "Performs the Delete action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00123"), false, false, "BankBranch.Delete", "Delete BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00124"), "Performs the Export action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00124"), false, false, "BankBranch.Export", "Export BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00125"), "Performs the Import action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00125"), false, false, "BankBranch.Import", "Import BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00126"), "Performs the Print action on the BankBranch entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00126"), false, false, "BankBranch.Print", "Print BankBranch", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00127"), "Performs the Show action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00127"), false, false, "Client.Show", "Show Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00128"), "Performs the Create action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00128"), false, false, "Client.Create", "Create Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00129"), "Performs the Update action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00129"), false, false, "Client.Update", "Update Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00130"), "Performs the Delete action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00130"), false, false, "Client.Delete", "Delete Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00131"), "Performs the Export action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00131"), false, false, "Client.Export", "Export Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00132"), "Performs the Import action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00132"), false, false, "Client.Import", "Import Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00133"), "Performs the Print action on the Client entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00133"), false, false, "Client.Print", "Print Client", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00134"), "Performs the Show action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00134"), false, false, "SellReturnInvoice.Show", "Show SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00135"), "Performs the Create action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00135"), false, false, "SellReturnInvoice.Create", "Create SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00136"), "Performs the Update action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00136"), false, false, "SellReturnInvoice.Update", "Update SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00137"), "Performs the Delete action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00137"), false, false, "SellReturnInvoice.Delete", "Delete SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00138"), "Performs the Export action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00138"), false, false, "SellReturnInvoice.Export", "Export SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00139"), "Performs the Import action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00139"), false, false, "SellReturnInvoice.Import", "Import SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00140"), "Performs the Print action on the SellReturnInvoice entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00140"), false, false, "SellReturnInvoice.Print", "Print SellReturnInvoice", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00141"), "Performs the Show action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00141"), false, false, "QuantityCrediting.Show", "Show QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00142"), "Performs the Create action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00142"), false, false, "QuantityCrediting.Create", "Create QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00143"), "Performs the Update action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00143"), false, false, "QuantityCrediting.Update", "Update QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00144"), "Performs the Delete action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00144"), false, false, "QuantityCrediting.Delete", "Delete QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00145"), "Performs the Export action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00145"), false, false, "QuantityCrediting.Export", "Export QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00146"), "Performs the Import action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00146"), false, false, "QuantityCrediting.Import", "Import QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00147"), "Performs the Print action on the QuantityCrediting entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00147"), false, false, "QuantityCrediting.Print", "Print QuantityCrediting", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00148"), "Performs the Show action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00148"), false, false, "Purchase.Show", "Show Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00149"), "Performs the Create action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00149"), false, false, "Purchase.Create", "Create Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00150"), "Performs the Update action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00150"), false, false, "Purchase.Update", "Update Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00151"), "Performs the Delete action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00151"), false, false, "Purchase.Delete", "Delete Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00152"), "Performs the Export action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00152"), false, false, "Purchase.Export", "Export Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00153"), "Performs the Import action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00153"), false, false, "Purchase.Import", "Import Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00154"), "Performs the Print action on the Purchase entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00154"), false, false, "Purchase.Print", "Print Purchase", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00155"), "Performs the Show action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00155"), false, false, "FinancingOperations.Show", "Show FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00156"), "Performs the Create action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00156"), false, false, "FinancingOperations.Create", "Create FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00157"), "Performs the Update action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00157"), false, false, "FinancingOperations.Update", "Update FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00158"), "Performs the Delete action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00158"), false, false, "FinancingOperations.Delete", "Delete FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00159"), "Performs the Export action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00159"), false, false, "FinancingOperations.Export", "Export FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00160"), "Performs the Import action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00160"), false, false, "FinancingOperations.Import", "Import FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00161"), "Performs the Print action on the FinancingOperations entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00161"), false, false, "FinancingOperations.Print", "Print FinancingOperations", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00162"), "Performs the Show action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00162"), false, false, "Marketer.Show", "Show Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00163"), "Performs the Create action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00163"), false, false, "Marketer.Create", "Create Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00164"), "Performs the Update action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00164"), false, false, "Marketer.Update", "Update Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00165"), "Performs the Delete action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00165"), false, false, "Marketer.Delete", "Delete Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00166"), "Performs the Export action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00166"), false, false, "Marketer.Export", "Export Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00167"), "Performs the Import action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00167"), false, false, "Marketer.Import", "Import Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00168"), "Performs the Print action on the Marketer entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00168"), false, false, "Marketer.Print", "Print Marketer", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00169"), "Performs the Show action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00169"), false, false, "MarketerDisbursementPermit.Show", "Show MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00170"), "Performs the Create action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00170"), false, false, "MarketerDisbursementPermit.Create", "Create MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00171"), "Performs the Update action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00171"), false, false, "MarketerDisbursementPermit.Update", "Update MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00172"), "Performs the Delete action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00172"), false, false, "MarketerDisbursementPermit.Delete", "Delete MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00173"), "Performs the Export action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00173"), false, false, "MarketerDisbursementPermit.Export", "Export MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00174"), "Performs the Import action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00174"), false, false, "MarketerDisbursementPermit.Import", "Import MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00175"), "Performs the Print action on the MarketerDisbursementPermit entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00175"), false, false, "MarketerDisbursementPermit.Print", "Print MarketerDisbursementPermit", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00176"), "Performs the Show action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00176"), false, false, "Financial.Show", "Show Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00177"), "Performs the Create action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00177"), false, false, "Financial.Create", "Create Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00178"), "Performs the Update action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00178"), false, false, "Financial.Update", "Update Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00179"), "Performs the Delete action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00179"), false, false, "Financial.Delete", "Delete Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00180"), "Performs the Export action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00180"), false, false, "Financial.Export", "Export Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00181"), "Performs the Import action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00181"), false, false, "Financial.Import", "Import Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00182"), "Performs the Print action on the Financial entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00182"), false, false, "Financial.Print", "Print Financial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00183"), "Performs the Show action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00183"), false, false, "Config.Show", "Show Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00184"), "Performs the Create action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00184"), false, false, "Config.Create", "Create Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00185"), "Performs the Update action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00185"), false, false, "Config.Update", "Update Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00186"), "Performs the Delete action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00186"), false, false, "Config.Delete", "Delete Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00187"), "Performs the Export action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00187"), false, false, "Config.Export", "Export Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00188"), "Performs the Import action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00188"), false, false, "Config.Import", "Import Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00189"), "Performs the Print action on the Config entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00189"), false, false, "Config.Print", "Print Config", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00190"), "Performs the Show action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00190"), false, false, "Group.Show", "Show Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00191"), "Performs the Create action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00191"), false, false, "Group.Create", "Create Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00192"), "Performs the Update action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00192"), false, false, "Group.Update", "Update Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00193"), "Performs the Delete action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00193"), false, false, "Group.Delete", "Delete Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00194"), "Performs the Export action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00194"), false, false, "Group.Export", "Export Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00195"), "Performs the Import action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00195"), false, false, "Group.Import", "Import Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00196"), "Performs the Print action on the Group entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00196"), false, false, "Group.Print", "Print Group", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00197"), "Performs the Show action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00197"), false, false, "User.Show", "Show User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00198"), "Performs the Create action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00198"), false, false, "User.Create", "Create User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00199"), "Performs the Update action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00199"), false, false, "User.Update", "Update User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00200"), "Performs the Delete action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00200"), false, false, "User.Delete", "Delete User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00201"), "Performs the Export action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00201"), false, false, "User.Export", "Export User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00202"), "Performs the Import action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00202"), false, false, "User.Import", "Import User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00203"), "Performs the Print action on the User entity", new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4a00203"), false, false, "User.Print", "Print User", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400001"), "Performs the Show action on the AccountMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400001"), false, false, "StatementsNavigator.AccountMovements-Show", "AccountMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400002"), "Performs the Show action on the TheClientDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400002"), false, false, "StatementsNavigator.TheClientDebtion-Show", "TheClientDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400003"), "Performs the Show action on the TheClientDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400003"), false, false, "StatementsNavigator.TheClientDebtionDetails-Show", "TheClientDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400004"), "Performs the Show action on the ClientsOverdueDebts Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400004"), false, false, "StatementsNavigator.ClientsOverdueDebts-Show", "ClientsOverdueDebts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400005"), "Performs the Show action on the TheCSupplierDebtion Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400005"), false, false, "StatementsNavigator.TheCSupplierDebtion-Show", "TheCSupplierDebtion", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400006"), "Performs the Show action on the TheRepresentativeDebtionDetails Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400006"), false, false, "StatementsNavigator.TheRepresentativeDebtionDetails-Show", "TheRepresentativeDebtionDetails", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400007"), "Performs the Show action on the MarketerCredtions Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400007"), false, false, "StatementsNavigator.MarketerCredtions-Show", "MarketerCredtions", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400008"), "Performs the Show action on the ItemsStoragesQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400008"), false, false, "StatementsNavigator.ItemsStoragesQuantities-Show", "ItemsStoragesQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400009"), "Performs the Show action on the ItemsMarkersQuantities Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400009"), false, false, "StatementsNavigator.ItemsMarkersQuantities-Show", "ItemsMarkersQuantities", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400010"), "Performs the Show action on the ItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400010"), false, false, "StatementsNavigator.ItemsMovements-Show", "ItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400011"), "Performs the Show action on the RepresentativeItemsMovements Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400011"), false, false, "StatementsNavigator.RepresentativeItemsMovements-Show", "RepresentativeItemsMovements", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400012"), "Performs the Show action on the MissedSerial Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400012"), false, false, "StatementsNavigator.MissedSerial-Show", "MissedSerial", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400013"), "Performs the Show action on the SalesItemsProfit2 Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400013"), false, false, "StatementsNavigator.SalesItemsProfit2-Show", "SalesItemsProfit2", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400014"), "Performs the Show action on the SalesCollectionInsights Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400014"), false, false, "StatementsNavigator.SalesCollectionInsights-Show", "SalesCollectionInsights", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400015"), "Performs the Show action on the MaxDiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400015"), false, false, "StatementsNavigator.MaxDiscountItemsBased-Show", "MaxDiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400016"), "Performs the Show action on the DiscountItemsBased Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400016"), false, false, "StatementsNavigator.DiscountItemsBased-Show", "DiscountItemsBased", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400017"), "Performs the Show action on the RepresentativeAmounts Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400017"), false, false, "StatementsNavigator.RepresentativeAmounts-Show", "RepresentativeAmounts", null, new Guid("00000000-0000-0000-0000-000000000000"), "" },
                    { new Guid("08b4dae1-3eef-4509-b1e6-149062400018"), "Performs the Show action on the FinancialHestory Report", new Guid("08b4dae1-3eef-4509-b1e6-149062400018"), false, false, "StatementsNavigator.FinancialHestory-Show", "FinancialHestory", null, new Guid("00000000-0000-0000-0000-000000000000"), "" }
                });

            migrationBuilder.InsertData(
                table: "TUser",
                columns: new[] { "Id", "Accesser", "Guid", "IsActive", "IsAdmin", "IsDeleted", "IsGhost", "IsRedy", "Name", "Password", "Phone", "RegesterTime", "RoleGroupId", "Sult", "TenantId" },
                values: new object[,]
                {
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb001"), "", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0"), false, true, false, true, false, "Ghost", "koad/tT8QGmZmk2B/s2iMALIvKdFe3wfFbwKWtbdhIKPoj2GaAQ/8/lobJxGgYP9cPyb3gfbY+Ery/zS6Mdvew==", "0924951905", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "SPcjqk+2nrGlvEnChIh3tMY2adLkzMIGypBaY4h2qcuep5N9wL5lrHekm1EOMaNRARwd67ZotceKY7eNRXXx3nBvmk4lbeG86xxAWZnuuXA3nnmLO//9k7Yx4IQbcLUwOUQOfmJbTKIn5RBYf12gMbOX+EKdPo93C/I2mlX0C2Y=", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1") },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb002"), "", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0"), false, false, false, false, false, "User", "iD+ZkASu6DmxMy6VVV+DO+EA3s+VEWzYCdmyKRELy8maXGluq2TWqQ0CfAZsHbHScBO8ib31QSHYiSezYTnVWQ==", "0915951905", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "/BcPKD/+VMk2vI+8UobrzgSmvizsDgHHPgQzz65YrqH/2fL9nsd9/ju7StpKItqvg5Gxd78Je57NWvWH03aYo9d9pRwYY9sYgJdxmyBzAo7h7vK9G+35WwOI43RPFiIbidOv4fZjhZ8lf5J0+4Fh8l8FvdlIbS0+vb5lga0dxxA=", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1") },
                    { new Guid("08b4dae1-2eef-4509-b1e6-1fa9b4acb003"), "", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba0"), false, true, false, true, false, "Admin", "dgNZRDuZ4vtReF151Y3RWLNio5IemIuS3OWYbPsbhfAyq+nqQ+4RQF953hbGpTeXjU4QxG7HsmZj8gB/qDbHuw==", "1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "sq/uSGqIZcbHYRlFWL81zClVi57NEEoJEx1FH2/lblMBx2YUh4P8BN52wf29UuFoP/6lbnYqpq/I8oyYkf9vPGIOlUUHFy4IKYC3TAi5aZLIUAYu5LDN09gSc72+Jf1e8SwTLST4e4jV0KdV4GvKJw27dcqZS88xzbhe/8zcdKY=", new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabba1") }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountCat", "CreateDate", "CurrencyId", "Guid", "IsDeleted", "Name", "Natural", "Number", "OpeningCreditor", "OpeningDebtor", "ParentId", "TenantId" },
                values: new object[,]
                {
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651001"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مصروفات عمومية", 0, 51L, 0m, 0m, new Guid("0e54960c-8a5b-473d-8022-c4a47c65185f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651002"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مشتريات", 0, 52L, 0m, 0m, new Guid("0e54960c-8a5b-473d-8022-c4a47c65185f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651102"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مبيعات", 1, 41L, 0m, 0m, new Guid("1c63e7c0-033d-435b-b55f-4a37cb6ab77c"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651103"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مبيعات المسوقين", 1, 41L, 0m, 0m, new Guid("1c63e7c0-033d-435b-b55f-4a37cb6ab77c"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c8847c651103"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "حساب العمولات العام", 1, 41L, 0m, 0m, new Guid("0e54960c-8a5b-473d-8022-c4a47c65185f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c8847c651104"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "حساب التخفيضات", 1, 41L, 0m, 0m, new Guid("0e54960c-8a5b-473d-8022-c4a47c65185f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("18847d06-8ff6-4e55-9b63-8318edbf6197"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أصول متداولة", 0, 12L, 0m, 0m, new Guid("9002ea94-02ff-47f0-907c-00a8360c440b"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("ad630d24-f791-4d5a-8104-27595e273112"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أصول ثابتة", 0, 11L, 0m, 0m, new Guid("9002ea94-02ff-47f0-907c-00a8360c440b"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "خصوم متداولة", 1, 22L, 0m, 0m, new Guid("83a0c584-5013-4186-a337-2985d38b6544"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("e87b3121-fb01-489a-9081-b50f9e2e3a91"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "خصوم ثابتة", 1, 21L, 0m, 0m, new Guid("83a0c584-5013-4186-a337-2985d38b6544"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651105"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أعمال صيانة خارجية", 1, 61L, 0m, 0m, new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651106"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أوامر تصنيع خارجية", 1, 71L, 0m, 0m, new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651107"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أعمال صيانة داخلية", 1, 91L, 0m, 0m, new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("0e54960c-8a5b-473d-8022-c4a47c651108"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أوامر صناعة داخلية", 1, 81L, 0m, 0m, new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("1036cd62-716a-4b8c-a19f-40ee2584484f"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "سيارات", 0, 112L, 0m, 0m, new Guid("ad630d24-f791-4d5a-8104-27595e273112"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("13440fba-02cd-4f4b-843b-6159611c3fac"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "الموظفين", 1, 222L, 0m, 0m, new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("23b638f5-71d4-4256-b99b-5b84b19c91a7"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مباني", 0, 111L, 0m, 0m, new Guid("ad630d24-f791-4d5a-8104-27595e273112"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("2b5ee402-49f1-444c-9a86-bf76e0ecbf90"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "الموردين", 1, 221L, 0m, 0m, new Guid("dbcb6851-213c-4238-a2a9-46f0d359c878"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("a65d59a6-4291-4bfe-b718-9c676a5b8578"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "أثاث", 0, 123L, 0m, 0m, new Guid("ad630d24-f791-4d5a-8104-27595e273112"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da1"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "الخزائن", 0, 122L, 0m, 0m, new Guid("18847d06-8ff6-4e55-9b63-8318edbf6197"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6da2"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "البنوك", 0, 123L, 0m, 0m, new Guid("18847d06-8ff6-4e55-9b63-8318edbf6197"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6dc2"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "العملاء", 0, 121L, 0m, 0m, new Guid("18847d06-8ff6-4e55-9b63-8318edbf6197"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("2b5ee402-49f1-444c-9a86-bf76e0ecbf91"), 1, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "مورد عام", 1, 2211L, 0m, 0m, new Guid("2b5ee402-49f1-444c-9a86-bf76e0ecbf90"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") },
                    { new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6dc3"), 0, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("fc75d0d6-b4f0-4d5d-953c-ce6f44324a4f"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd5"), false, "عميل عام", 0, 1211L, 0m, 0m, new Guid("adecdcbe-5c66-4069-9f41-ee29aa4a6dc2"), new Guid("eebb6ed7-85ef-40d4-ad79-8f9d4bbabbd8") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentId",
                table: "Accounts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TAssets_AccountId",
                table: "TAssets",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TAttendanceDeparture_EmployeeId",
                table: "TAttendanceDeparture",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TBank_AccountId",
                table: "TBank",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TBankBranch_AccountId",
                table: "TBankBranch",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TBankBranch_BankId",
                table: "TBankBranch",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_TCategory_CategoryId",
                table: "TCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TClient_AccountId",
                table: "TClient",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TDamagedInvoice_MarketerStoreId",
                table: "TDamagedInvoice",
                column: "MarketerStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TDamagedInvoice_StoreId",
                table: "TDamagedInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TDamagedInvoiceDetail_DamagedInvoiceId",
                table: "TDamagedInvoiceDetail",
                column: "DamagedInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TDamagedInvoiceDetail_ItemId",
                table: "TDamagedInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TDamagedInvoiceDetail_UnitId",
                table: "TDamagedInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TDisbursementPermit_CostCenterId",
                table: "TDisbursementPermit",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_TDisbursementPermit_StoreId",
                table: "TDisbursementPermit",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TDisbursementPermitDetail_DisbursementPermitId",
                table: "TDisbursementPermitDetail",
                column: "DisbursementPermitId");

            migrationBuilder.CreateIndex(
                name: "IX_TDisbursementPermitDetail_ItemId",
                table: "TDisbursementPermitDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TDisbursementPermitDetail_UnitId",
                table: "TDisbursementPermitDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TDiscountIncrease_EmployeeId",
                table: "TDiscountIncrease",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TDiscountitedItem_ClientId",
                table: "TDiscountitedItem",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TDiscountitedItem_ItemId",
                table: "TDiscountitedItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TDiscountitedItem_UnitId",
                table: "TDiscountitedItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TEmployee_AccountId",
                table: "TEmployee",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TEmployee_DepartmentId",
                table: "TEmployee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TEmployeeAssignment_EmployeeId",
                table: "TEmployeeAssignment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TEmployeeSalaryCounting_EmployeeId",
                table: "TEmployeeSalaryCounting",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TEmployeeSalaryCounting_EmployeeSalaryCountingPermitId",
                table: "TEmployeeSalaryCounting",
                column: "EmployeeSalaryCountingPermitId");

            migrationBuilder.CreateIndex(
                name: "IX_TEntryDetail_AccountId",
                table: "TEntryDetail",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TEntryDetail_EntryId",
                table: "TEntryDetail",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoice_ClientId",
                table: "TFactoringInvoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoice_ManufacturingFacilityId",
                table: "TFactoringInvoice",
                column: "ManufacturingFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoice_SafeId",
                table: "TFactoringInvoice",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoice_StoreId",
                table: "TFactoringInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceComposition_FactoringInvoiceId",
                table: "TFactoringInvoiceComposition",
                column: "FactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceComposition_ItemCompositionId",
                table: "TFactoringInvoiceComposition",
                column: "ItemCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceComposition_TFactoringInvoiceId",
                table: "TFactoringInvoiceComposition",
                column: "TFactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceCost_FactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                column: "FactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceCost_TFactoringInvoiceId",
                table: "TFactoringInvoiceCost",
                column: "TFactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceDetail_FactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                column: "FactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceDetail_ItemId",
                table: "TFactoringInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceDetail_TFactoringInvoiceId",
                table: "TFactoringInvoiceDetail",
                column: "TFactoringInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TFactoringInvoiceDetail_UnitId",
                table: "TFactoringInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TInventory_ItemId",
                table: "TInventory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TInventory_StoreId",
                table: "TInventory",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TInventory_UnitId",
                table: "TInventory",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TInventoryCost_TInventoryId",
                table: "TInventoryCost",
                column: "TInventoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TItem_CategoryId",
                table: "TItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemComposition_CategoryId",
                table: "TItemComposition",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemCompositionDetail_ItemCompositionId",
                table: "TItemCompositionDetail",
                column: "ItemCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemCompositionDetail_ItemId",
                table: "TItemCompositionDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemCompositionDetail_ItemVarianceId",
                table: "TItemCompositionDetail",
                column: "ItemVarianceId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemCompositionDetail_UnitId",
                table: "TItemCompositionDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemUnit_ItemId",
                table: "TItemUnit",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TItemUnit_UnitId",
                table: "TItemUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoice_ClientId",
                table: "TMaintenanceInvoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoice_SafeId",
                table: "TMaintenanceInvoice",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoice_StoreId",
                table: "TMaintenanceInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceCost_MaintenanceInvoiceId",
                table: "TMaintenanceInvoiceCost",
                column: "MaintenanceInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceCost_TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceCost",
                column: "TMaintenanceInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceDetail_ItemId",
                table: "TMaintenanceInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceDetail_ItemVarianceId",
                table: "TMaintenanceInvoiceDetail",
                column: "ItemVarianceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceDetail_MaintenanceInvoiceId",
                table: "TMaintenanceInvoiceDetail",
                column: "MaintenanceInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceDetail_TMaintenanceInvoiceId",
                table: "TMaintenanceInvoiceDetail",
                column: "TMaintenanceInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceInvoiceDetail_UnitId",
                table: "TMaintenanceInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TMaintenanceSector_AccountId",
                table: "TMaintenanceSector",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TManufacturingFacility_AccountId",
                table: "TManufacturingFacility",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TMarketer_AccountId",
                table: "TMarketer",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerClient_ClientId",
                table: "TMarketerClient",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerClient_MarketerId",
                table: "TMarketerClient",
                column: "MarketerId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerDisbursementPermit_MarketerId",
                table: "TMarketerDisbursementPermit",
                column: "MarketerId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerDisbursementPermit_StoreId",
                table: "TMarketerDisbursementPermit",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerDisbursementPermitDetail_ItemId",
                table: "TMarketerDisbursementPermitDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerDisbursementPermitDetail_MarketerDisbursementPermit~",
                table: "TMarketerDisbursementPermitDetail",
                column: "MarketerDisbursementPermitId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerDisbursementPermitDetail_UnitId",
                table: "TMarketerDisbursementPermitDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerInventory_ItemId",
                table: "TMarketerInventory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerInventory_MarketerId",
                table: "TMarketerInventory",
                column: "MarketerId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerInventory_UnitId",
                table: "TMarketerInventory",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSalesInvoice_ClientId",
                table: "TMarketerSalesInvoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSalesInvoice_MarketerId",
                table: "TMarketerSalesInvoice",
                column: "MarketerId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSalesInvoiceDetail_ItemId",
                table: "TMarketerSalesInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSalesInvoiceDetail_MarketerSalesInvoiceId",
                table: "TMarketerSalesInvoiceDetail",
                column: "MarketerSalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSalesInvoiceDetail_UnitId",
                table: "TMarketerSalesInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSharedEntry_EntryId",
                table: "TMarketerSharedEntry",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_TMarketerSharedEntry_MarketerId",
                table: "TMarketerSharedEntry",
                column: "MarketerId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoice_SafeId",
                table: "TPurchaseInvoice",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoice_StoreId",
                table: "TPurchaseInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoice_SupplierId",
                table: "TPurchaseInvoice",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoiceCost_PurchaseInvoiceId",
                table: "TPurchaseInvoiceCost",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoiceDetail_ItemId",
                table: "TPurchaseInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoiceDetail_PurchaseInvoiceId",
                table: "TPurchaseInvoiceDetail",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseInvoiceDetail_UnitId",
                table: "TPurchaseInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoice_InvoiceId",
                table: "TPurchaseReturnInvoice",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoice_SafeId",
                table: "TPurchaseReturnInvoice",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoice_StoreId",
                table: "TPurchaseReturnInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoice_SupplierId",
                table: "TPurchaseReturnInvoice",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoiceDetail_ItemId",
                table: "TPurchaseReturnInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoiceDetail_ReturnInvoiceId",
                table: "TPurchaseReturnInvoiceDetail",
                column: "ReturnInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TPurchaseReturnInvoiceDetail_UnitId",
                table: "TPurchaseReturnInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TQuantityCreditingInvoice_StoreId",
                table: "TQuantityCreditingInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TQuantityCreditingInvoiceDetail_ItemId",
                table: "TQuantityCreditingInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TQuantityCreditingInvoiceDetail_QuantityCreditingInvoiceId",
                table: "TQuantityCreditingInvoiceDetail",
                column: "QuantityCreditingInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TQuantityCreditingInvoiceDetail_UnitId",
                table: "TQuantityCreditingInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TRole_TUserId",
                table: "TRole",
                column: "TUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TRoleGroupDetail_RoleGroupId",
                table: "TRoleGroupDetail",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TRoleGroupDetail_RoleId",
                table: "TRoleGroupDetail",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TSafe_AccountId",
                table: "TSafe",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoice_ClientId",
                table: "TSalesInvoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoice_MarketerStorageId",
                table: "TSalesInvoice",
                column: "MarketerStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoice_RepresentativeId",
                table: "TSalesInvoice",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoice_SafeId",
                table: "TSalesInvoice",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoice_StoreId",
                table: "TSalesInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoiceDetail_ItemId",
                table: "TSalesInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoiceDetail_SalesInvoiceId",
                table: "TSalesInvoiceDetail",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TSalesInvoiceDetail_UnitId",
                table: "TSalesInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoice_ClientId",
                table: "TSellReturnInvoice",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoice_InvoiceId",
                table: "TSellReturnInvoice",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoice_SafeId",
                table: "TSellReturnInvoice",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoice_StoreId",
                table: "TSellReturnInvoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoiceDetail_ItemId",
                table: "TSellReturnInvoiceDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoiceDetail_ReturnInvoiceId",
                table: "TSellReturnInvoiceDetail",
                column: "ReturnInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TSellReturnInvoiceDetail_UnitId",
                table: "TSellReturnInvoiceDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TSupplier_AccountId",
                table: "TSupplier",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TSupplyPermit_StoreId",
                table: "TSupplyPermit",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TSupplyPermit_SupplierId",
                table: "TSupplyPermit",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TSupplyPermitDetail_ItemId",
                table: "TSupplyPermitDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TSupplyPermitDetail_SupplyPermitId",
                table: "TSupplyPermitDetail",
                column: "SupplyPermitId");

            migrationBuilder.CreateIndex(
                name: "IX_TSupplyPermitDetail_UnitId",
                table: "TSupplyPermitDetail",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TUser_RoleGroupId",
                table: "TUser",
                column: "RoleGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TAssets");

            migrationBuilder.DropTable(
                name: "TAttendanceDeparture");

            migrationBuilder.DropTable(
                name: "TBankBranch");

            migrationBuilder.DropTable(
                name: "TCompanyInfo");

            migrationBuilder.DropTable(
                name: "TConfig");

            migrationBuilder.DropTable(
                name: "TCurrency");

            migrationBuilder.DropTable(
                name: "TDamagedInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TDisbursementPermitDetail");

            migrationBuilder.DropTable(
                name: "TDiscountIncrease");

            migrationBuilder.DropTable(
                name: "TDiscountitedItem");

            migrationBuilder.DropTable(
                name: "TEmployeeAssignment");

            migrationBuilder.DropTable(
                name: "TEmployeeSalaryCounting");

            migrationBuilder.DropTable(
                name: "TEntryDetail");

            migrationBuilder.DropTable(
                name: "TExpenseTypes");

            migrationBuilder.DropTable(
                name: "TFactoringInvoiceComposition");

            migrationBuilder.DropTable(
                name: "TFactoringInvoiceCost");

            migrationBuilder.DropTable(
                name: "TFactoringInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TInventoryCost");

            migrationBuilder.DropTable(
                name: "TItemCompositionDetail");

            migrationBuilder.DropTable(
                name: "TItemUnit");

            migrationBuilder.DropTable(
                name: "TMaintenanceInvoiceCost");

            migrationBuilder.DropTable(
                name: "TMaintenanceInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TMaintenanceSector");

            migrationBuilder.DropTable(
                name: "TMarketerClient");

            migrationBuilder.DropTable(
                name: "TMarketerDisbursementPermitDetail");

            migrationBuilder.DropTable(
                name: "TMarketerInventory");

            migrationBuilder.DropTable(
                name: "TMarketerSalesInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TMarketerSharedEntry");

            migrationBuilder.DropTable(
                name: "TPurchaseInvoiceCost");

            migrationBuilder.DropTable(
                name: "TPurchaseInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TPurchaseReturnInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TQuantityCreditingInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TRoleGroupDetail");

            migrationBuilder.DropTable(
                name: "TSalesInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TSellReturnInvoiceDetail");

            migrationBuilder.DropTable(
                name: "TSupplyPermitDetail");

            migrationBuilder.DropTable(
                name: "TBank");

            migrationBuilder.DropTable(
                name: "TDamagedInvoice");

            migrationBuilder.DropTable(
                name: "TDisbursementPermit");

            migrationBuilder.DropTable(
                name: "TEmployeeSalaryCountingPermit");

            migrationBuilder.DropTable(
                name: "TEmployee");

            migrationBuilder.DropTable(
                name: "TFactoringInvoice");

            migrationBuilder.DropTable(
                name: "TInventory");

            migrationBuilder.DropTable(
                name: "TItemComposition");

            migrationBuilder.DropTable(
                name: "TItemVariance");

            migrationBuilder.DropTable(
                name: "TMaintenanceInvoice");

            migrationBuilder.DropTable(
                name: "TMarketerDisbursementPermit");

            migrationBuilder.DropTable(
                name: "TMarketerSalesInvoice");

            migrationBuilder.DropTable(
                name: "TEntry");

            migrationBuilder.DropTable(
                name: "TPurchaseReturnInvoice");

            migrationBuilder.DropTable(
                name: "TQuantityCreditingInvoice");

            migrationBuilder.DropTable(
                name: "TRole");

            migrationBuilder.DropTable(
                name: "TSellReturnInvoice");

            migrationBuilder.DropTable(
                name: "TSupplyPermit");

            migrationBuilder.DropTable(
                name: "TCostCenter");

            migrationBuilder.DropTable(
                name: "TDepartment");

            migrationBuilder.DropTable(
                name: "TManufacturingFacility");

            migrationBuilder.DropTable(
                name: "TItem");

            migrationBuilder.DropTable(
                name: "TUnit");

            migrationBuilder.DropTable(
                name: "TPurchaseInvoice");

            migrationBuilder.DropTable(
                name: "TUser");

            migrationBuilder.DropTable(
                name: "TSalesInvoice");

            migrationBuilder.DropTable(
                name: "TCategory");

            migrationBuilder.DropTable(
                name: "TSupplier");

            migrationBuilder.DropTable(
                name: "TRoleGroup");

            migrationBuilder.DropTable(
                name: "TClient");

            migrationBuilder.DropTable(
                name: "TMarketer");

            migrationBuilder.DropTable(
                name: "TSafe");

            migrationBuilder.DropTable(
                name: "TStore");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
