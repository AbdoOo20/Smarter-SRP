using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;

namespace Fly.Shared.Modules.Accounting.SeededData;

public class AccountSeed
{
    private const string c = "";
    private static readonly DateTime d = new(2024, 7, 2);

    //public static void Prepare()
    //{

    //        // Assets
    //        var assets = Create("Assets", "أصول", 1, DateTime.Now, null, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Current Assets
    //        var currentAssets = Create("CurrentAssets", "أصول متداولة", 11, DateTime.Now, assets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Cash
    //        Create("Cash", "نقد", 111, DateTime.Now, currentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("CashInHand", "نقد في الصندوق", 1111, DateTime.Now, "Cash", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("CashAtBank", "نقد في البنك", 1112, DateTime.Now, "Cash", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("CurrentAccounts", "حسابات جارية", 1113, DateTime.Now, "Cash", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Short-term Investments
    //        Create("ShortTermInvestments", "استثمارات قصيرة الأجل", 112, DateTime.Now, currentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Bonds", "سندات", 1121, DateTime.Now, "ShortTermInvestments", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Stocks", "أسهم", 1122, DateTime.Now, "ShortTermInvestments", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Inventory
    //        Create("Inventory", "مخزون", 113, DateTime.Now, currentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("RawMaterials", "مواد خام", 1131, DateTime.Now, "Inventory", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("FinishedGoods", "بضائع جاهزة للبيع", 1132, DateTime.Now, "Inventory", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("WorkInProgress", "بضائع تحت الإنتاج", 1133, DateTime.Now, "Inventory", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Accounts Receivable
    //        Create("AccountsReceivable", "الديون المستحقة القبض", 114, DateTime.Now, currentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Customers", "زبائن", 1141, DateTime.Now, "AccountsReceivable", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Clients", "عملاء", 1142, DateTime.Now, "AccountsReceivable", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Prepaid Expenses
    //        Create("PrepaidExpenses", "مصروفات مدفوعة مقدما", 115, DateTime.Now, currentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Insurance", "تأمين", 1151, DateTime.Now, "PrepaidExpenses", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Rent", "إيجار", 1152, DateTime.Now, "PrepaidExpenses", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Services", "خدمات", 1153, DateTime.Now, "PrepaidExpenses", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Subscriptions", "اشتراكات", 1154, DateTime.Now, "PrepaidExpenses", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Non-current Assets
    //        var nonCurrentAssets = Create("NonCurrentAssets", "أصول غير متداولة", 12, DateTime.Now, assets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Fixed Assets
    //        Create("FixedAssets", "الأصول الثابتة", 121, DateTime.Now, nonCurrentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Land", "أراضي", 1211, DateTime.Now, "FixedAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Buildings", "مباني", 1212, DateTime.Now, "FixedAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Equipment", "معدات", 1213, DateTime.Now, "FixedAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Vehicles", "مركبات", 1214, DateTime.Now, "FixedAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Furniture", "أثاث", 1215, DateTime.Now, "FixedAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Software", "برامج كمبيوتر", 1216, DateTime.Now, "FixedAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Long-term Investments
    //        Create("LongTermInvestments", "الاستثمارات طويلة الأجل", 122, DateTime.Now, nonCurrentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("LongTermBonds", "سندات", 1221, DateTime.Now, "LongTermInvestments", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("LongTermStocks", "أسهم", 1222, DateTime.Now, "LongTermInvestments", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("RealEstate", "عقارات", 1223, DateTime.Now, "LongTermInvestments", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Long-term Receivables
    //        Create("LongTermReceivables", "الديون المستحقة القبض طويلة الأجل", 123, DateTime.Now, nonCurrentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Intangible Assets
    //        Create("IntangibleAssets", "الأصول غير الملموسة", 124, DateTime.Now, nonCurrentAssets.Id, AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Patents", "براءات الاختراع", 1241, DateTime.Now, "IntangibleAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Trademarks", "علامات تجارية", 1242, DateTime.Now, "IntangibleAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("Copyrights", "حقوق النشر", 1243, DateTime.Now, "IntangibleAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("BrandNames", "أسماء تجارية", 1244, DateTime.Now, "IntangibleAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);
    //        Create("CustomerLists", "قوائم عملاء", 1245, DateTime.Now, "IntangibleAssets", AccountCat.Assets, AccountType.Debit, "SAR", 0, 0);

    //        // Liabilities
    //        var liabilities = Create("Liabilities", "الخصوم", 2, DateTime.Now, null, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Current Liabilities
    //        var currentLiabilities = Create("CurrentLiabilities", "الخصوم المتداولة", 21, DateTime.Now, liabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Accounts Payable
    //        Create("AccountsPayable", "الديون المستحقة الدفع", 211, DateTime.Now, currentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("Suppliers", "موردين", 2111, DateTime.Now, "AccountsPayable", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("Vendors", "موردين", 2112, DateTime.Now, "AccountsPayable", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Salaries Payable
    //        Create("SalariesPayable", "الرواتب المستحقة الدفع", 212, DateTime.Now, currentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Taxes Payable
    //        Create("TaxesPayable", "الضرائب المستحقة الدفع", 213, DateTime.Now, currentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Short-term Loans
    //        Create("ShortTermLoans", "الديون قصيرة الأجل", 214, DateTime.Now, currentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Other Current Liabilities
    //        Create("OtherCurrentLiabilities", "الديون المستحقة الدفع الأخرى", 215, DateTime.Now, currentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("Guarantees", "ضمانات", 2151, DateTime.Now, "OtherCurrentLiabilities", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("AdvancePayments", "دفعات مقدمة", 2152, DateTime.Now, "OtherCurrentLiabilities", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Non-current Liabilities
    //        var nonCurrentLiabilities = Create("NonCurrentLiabilities", "الخصوم غير المتداولة", 22, DateTime.Now, liabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Long-term Loans
    //        Create("LongTermLoans", "الديون طويلة الأجل", 221, DateTime.Now, nonCurrentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Long-term Payables
    //        Create("LongTermPayables", "الديون المستحقة الدفع طويلة الأجل", 222, DateTime.Now, nonCurrentLiabilities.Id, AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("LongTermBonds", "سندات", 2221, DateTime.Now, "LongTermPayables", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("LongTermLoans", "قروض", 2222, DateTime.Now, "LongTermPayables", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);
    //        Create("Leasing", "تأجير تمويلي", 2223, DateTime.Now, "LongTermPayables", AccountCat.Liabilities, AccountType.Credit, "SAR", 0, 0);

    //        // Equity
    //        var equity = Create("Equity", "حقوق الملكية", 3, DateTime.Now, null, AccountCat.Equity, AccountType.Credit, "SAR", 0, 0);

    //        // Capital
    //        Create("Capital", "رأس المال", 31, DateTime.Now, equity.Id, AccountCat.Equity, AccountType.Credit, "SAR", 0, 0);
    //        Create("ShareholdersEquity", "رأس مال المساهمين", 311, DateTime.Now, "Capital", AccountCat.Equity, AccountType.Credit, "SAR", 0, 0);
    //        Create("PartnersEquity", "رأس مال الشركاء", 312, DateTime.Now, "Capital", AccountCat.Equity, AccountType.Credit, "SAR", 0, 0);

    //        // Retained Earnings
    //        Create("RetainedEarnings", "الأرباح المحتجزة", 32, DateTime.Now, equity.Id, AccountCat.Equity, AccountType.Credit, "SAR", 0, 0);

    //        // Dividends Paid
    //        Create("DividendsPaid", "الأرباح المدفوعة", 33, DateTime.Now, equity.Id, AccountCat.Equity, AccountType.Debit, "SAR", 0, 0);

    //        // Profit and Loss
    //        var profitAndLoss = Create("ProfitAndLoss", "حساب الأرباح والخسائر", 4, DateTime.Now, null, AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);

    //        // Revenue
    //        Create("Revenue", "الإيرادات", 41, DateTime.Now, profitAndLoss.Id, AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);
    //        Create("Sales", "مبيعات", 411, DateTime.Now, "Revenue", AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);
    //        Create("Services", "خدمات", 412, DateTime.Now, "Revenue", AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);
    //        Create("OtherRevenue", "إيرادات أخرى", 413, DateTime.Now, "Revenue", AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);
    //        Create("Interest", "فوائد", 4131, DateTime.Now, "OtherRevenue", AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);
    //        Create("Rentals", "إيجارات", 4132, DateTime.Now, "OtherRevenue", AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);
    //        Create("Royalties", "روялتی", 4133, DateTime.Now, "OtherRevenue", AccountCat.ProfitAndLoss, AccountType.Credit, "SAR", 0, 0);

    //        // Expenses
    //        Create("Expenses", "المصروفات", 42, DateTime.Now, profitAndLoss.Id, AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);
    //        Create("CostOfGoodsSold", "تكلفة البضاعة المباعة", 421, DateTime.Now, "Expenses", AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);
    //        Create("OperatingExpenses", "مصروفات التشغيل", 422, DateTime.Now, "Expenses", AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);
    //        Create("Salaries", "رواتب", 4221, DateTime.Now, "OperatingExpenses", AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);
    //        Create("Rent", "إيجار", 4222, DateTime.Now, "OperatingExpenses", AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);
    //        Create("Insurance", "تأمين", 4223, DateTime.Now, "OperatingExpenses", AccountCat.ProfitAndLoss, AccountType.Debit, "SAR", 0, 0);
    //        //Create
    //    }

    public static IEnumerable<DtoAccount>? Accounts = new List<DtoAccount>
    {

        Create("9002EA94-02FF-47F0-907C-00A8360C440B", "أصول", 1,    d, null, AccountCat.Budget_Assets, AccountType.Debit),
        Create("83A0C584-5013-4186-A337-2985D38B6544", "خصوم", 2,    d,  null, AccountCat.Budget_Opponents, AccountType.Credit),
        Create("56844955-106c-4f9f-8f07-b8e33b3c2812", "حقوق الملكية", 3, d,  null, AccountCat.Budget_Opponents, AccountType.Credit),
        Create("1C63E7C0-033D-435B-B55F-4A37CB6AB77C", "إيرادات", 4, d,  null, AccountCat.ProfitLoss_Income, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C4A47C65185F", "مصروفات", 5, d,  null, AccountCat.ProfitLoss_Cost, AccountType.Debit),


        //  9002EA94-02FF-47F0-907C-00A8360C440B    ==  أصول
        Create("ad630d24-f791-4d5a-8104-27595e273112", "أصول ثابتة", 11, d,      "9002EA94-02FF-47F0-907C-00A8360C440B", AccountCat.Budget_Assets, AccountType.Debit),
        Create("18847d06-8ff6-4e55-9b63-8318edbf6197", "أصول متداولة", 12, d,    "9002EA94-02FF-47F0-907C-00A8360C440B", AccountCat.Budget_Assets, AccountType.Debit),

        //  ad630d24-f791-4d5a-8104-27595e273112    ==  أصول    -   أصول ثابتة
        Create("23b638f5-71d4-4256-b99b-5b84b19c91a7", "مباني", 111, d,     "ad630d24-f791-4d5a-8104-27595e273112", AccountCat.Budget_Assets, AccountType.Debit),
        Create("1036cd62-716a-4b8c-a19f-40ee2584484f", "سيارات", 112, d,    "ad630d24-f791-4d5a-8104-27595e273112", AccountCat.Budget_Assets, AccountType.Debit),
        Create("a65d59a6-4291-4bfe-b718-9c676a5b8578", "أثاث", 123, d,      "ad630d24-f791-4d5a-8104-27595e273112", AccountCat.Budget_Assets, AccountType.Debit),

        //  ad630d24-f791-4d5a-8104-27595e273112    ==  أصول    -   أصول متداولة
        Create("adecdcbe-5c66-4069-9f41-ee29aa4a6dc2", "العملاء", 121, d,       "18847d06-8ff6-4e55-9b63-8318edbf6197", AccountCat.Budget_Assets, AccountType.Debit),
        Create("adecdcbe-5c66-4069-9f41-ee29aa4a6da1", "الخزائن", 122, d,       "18847d06-8ff6-4e55-9b63-8318edbf6197", AccountCat.Budget_Assets, AccountType.Debit),
        Create("adecdcbe-5c66-4069-9f41-ee29aa4a6da2", "البنوك", 123, d,       "18847d06-8ff6-4e55-9b63-8318edbf6197", AccountCat.Budget_Assets, AccountType.Debit),
        Create("adecdcbe-5c66-4069-9f41-ee29aa4a6dc3", "عميل عام", 1211, d,       "adecdcbe-5c66-4069-9f41-ee29aa4a6dc2", AccountCat.Budget_Assets, AccountType.Debit),
        //
        //
        //
        //  83A0C584-5013-4186-A337-2985D38B6544    ==  خصوم
        Create("e87b3121-fb01-489a-9081-b50f9e2e3a91", "خصوم ثابتة", 21, d,      "83A0C584-5013-4186-A337-2985D38B6544", AccountCat.Budget_Opponents, AccountType.Credit),
        Create("dbcb6851-213c-4238-a2a9-46f0d359c878", "خصوم متداولة", 22, d,    "83A0C584-5013-4186-A337-2985D38B6544", AccountCat.Budget_Opponents, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C4A47C651107", "أعمال صيانة داخلية", 91, d,      "dbcb6851-213c-4238-a2a9-46f0d359c878", AccountCat.Budget_Opponents, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C4A47C651108", "أوامر صناعة داخلية", 81, d,      "dbcb6851-213c-4238-a2a9-46f0d359c878", AccountCat.Budget_Opponents, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C4A47C651105", "أعمال صيانة خارجية", 61, d,      "dbcb6851-213c-4238-a2a9-46f0d359c878", AccountCat.Budget_Opponents, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C4A47C651106", "أوامر تصنيع خارجية", 71, d,      "dbcb6851-213c-4238-a2a9-46f0d359c878", AccountCat.Budget_Opponents, AccountType.Credit),
        //
        Create("2b5ee402-49f1-444c-9a86-bf76e0ecbf90", "الموردين", 221, d,   "dbcb6851-213c-4238-a2a9-46f0d359c878", AccountCat.Budget_Opponents, AccountType.Credit),
        Create("13440fba-02cd-4f4b-843b-6159611c3fac", "الموظفين", 222, d,     "dbcb6851-213c-4238-a2a9-46f0d359c878", AccountCat.Budget_Assets, AccountType.Credit),
        Create("2b5ee402-49f1-444c-9a86-bf76e0ecbf91", "مورد عام", 2211, d,   "2b5ee402-49f1-444c-9a86-bf76e0ecbf90", AccountCat.Budget_Opponents, AccountType.Credit),
        //
        //  9002EA94-02FF-47F0-907C-00A8360C440B    ==  مصروفات
        Create("0E54960C-8A5B-473D-8022-C4A47C651001", "مصروفات عمومية", 51, d,      "0E54960C-8A5B-473D-8022-C4A47C65185F", AccountCat.Budget_Assets),
        Create("0E54960C-8A5B-473D-8022-C4A47C651002", "مشتريات", 52, d,      "0E54960C-8A5B-473D-8022-C4A47C65185F", AccountCat.Budget_Assets, AccountType.Debit),
        //
        //  9002EA94-02FF-47F0-907C-00A8360C440B    ==  إيرادات
        Create("0E54960C-8A5B-473D-8022-C4A47C651102", "مبيعات", 41, d,      "1C63E7C0-033D-435B-B55F-4A37CB6AB77C", AccountCat.Budget_Assets, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C4A47C651103", "مبيعات المسوقين", 41, d,      "1C63E7C0-033D-435B-B55F-4A37CB6AB77C", AccountCat.Budget_Assets, AccountType.Credit),
        //
        //
        Create("0E54960C-8A5B-473D-8022-C8847C651103", "حساب العمولات العام", 41, d,      "0E54960C-8A5B-473D-8022-C4A47C65185F", AccountCat.Budget_Assets, AccountType.Credit),
        Create("0E54960C-8A5B-473D-8022-C8847C651104", "حساب التخفيضات", 41, d,      "0E54960C-8A5B-473D-8022-C4A47C65185F", AccountCat.Budget_Assets, AccountType.Credit),
        //
        //
    };

    // Natural
    private static DtoAccount Create(string id, string name, long number,
        DateTime date, string? parentId, AccountCat accountCat, AccountType natural = AccountType.Debit, string currenyId = c, decimal opnDebtor = 0, decimal opnCreditor = 0)
    {
        return new DtoAccount
        {
            Id = Identifier.Parse(id),
            Name = name,
            Number = number,
            CurrencyId = Identifier.Parse(currenyId),
            OpeningCreditor = opnCreditor,
            CreateDate = date,
            ParentId = parentId is null ? null : Identifier.Parse(parentId),
            OpeningDebtor = opnDebtor,
            AccountCat = accountCat,
            Natural = natural,
            //Guid = Guid.Parse(id),
        };
    }
}