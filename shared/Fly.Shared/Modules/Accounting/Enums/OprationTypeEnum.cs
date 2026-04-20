namespace Fly.Shared.Modules.Accounting.Enums;

public enum TagTypeEnum : int
{
    MarketerOnly,
    NonMarketerOnly,
}
public enum OprationTypeEnum
{
    None = 0,
    Normal = 1,
    Purchases = 100,
    PurchasesCost = 101,
    PurchasesCost_Return = 102,
    Purchases_Return = 103,
    Sales = 200,
    Sales_Return = 201,
    Marketer = 300,
    MarketerSell = 301,
    MarketerMobileSell = 302,

    Maintenance = 350,
    Factoring = 351,
    AccountFinancial = 401,
    ClientFinancial = 402,
    EmployeeFinancial = 403,
    ExpenseFinancial = 404,
    SupplierFinancial = 405,
    MarketerFinancial = 406,
    DiscountFinancial = 407,
    RepresentativePaymentReceipt = 415,
    DiscountByItemFinancial = 408,

    DiscountingFinancial = 501,
    DiscountingItemFinancial = 502,


    OpeningBalance = 600,

    CountingTotal = 1001,

    SalaryConting = 1100,
}