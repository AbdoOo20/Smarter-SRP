using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Global.Requests;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Modules.Financial.Enums;

namespace Fly.Shared.Modules.Financial.Requests;

public class DynamicFieldRequest : QueryRequestBase
{
    public List<DynamicField> DynamicFields { get; set; } = [];
}
public class StatementQueryRequest : DynamicFieldRequest
{
    public int ItemId { get; set; }

    public Identifier? AccountId { get; set; } = null;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool CollectTheQuantitiesBeforeTime { get; set; }

    public List<DynamicField> DynamicFields { get; set; } = [];

    public List<DynamicCombo> DynamicCombos { get; set; } = [];
    
    public StatementAccountType AccountType { get; set; }

    [ClientInvisible]
    public string? Title { get; set; }

    [ClientInvisible]
    public string? ReportName { get; set; }
}
    
public class ItemInfoQueryRequest : StatementQueryRequest
{
    public Identifier ItemIdd { get; set; }
    public Identifier StoreId { get; set; }
    public Identifier UnitId { get; set; }
    public Identifier? CategoryId { get; set; }
}

public class EmployeeFinancialRequest : RSimpleRequest
{
    public bool IsReceipt { get; set; }


}

public class AccountFinancialRequest : RSimpleRequest
{
    public bool IsReceipt { get; set; }

    public override DtoEntry CreateEntry()
    {
        var b = base.CreateEntry();
        b.OprationType = OprationTypeEnum.AccountFinancial;
        return b;
    }
}

public class SupplierFinancialRequest : RSimpleRequest
{
    public bool IsReceipt { get; set; }
}

public class ClientFinancialRequest : RSimpleRequest
{
    public bool IsReceipt { get; set; }

    public override DtoEntry CreateEntry()
    {
        var ent = base.CreateEntry();
        ent.OprationType = OprationTypeEnum.ClientFinancial;

        return ent;
    }
    public CreateEntryRequest CreateEntr2y(Identifier debtionAccount)
    {
        var ent = base.CreateEntry();
        ent.OprationType = OprationTypeEnum.ClientFinancial;

        return new CreateEntryRequest()
        {

            Date = Date,
            Note = Note,
            Details = new List<RcEntryDetail>
            {
                new RcEntryDetail
                {
                    AccountId = FromAccount,
                    Credit = Amount
                },
                new RcEntryDetail
                {
                    AccountId = ToAccount,
                    Debit = Amount
                },
                new RcEntryDetail
                {
                    AccountId = debtionAccount,
                    Credit = TotalAmount
                },
                new RcEntryDetail
                {
                    AccountId = FromAccount,
                    Debit = TotalAmount
                },
            },
        };
    }
}

public class ExpenseFinancialRequest : RSimpleRequest
{
    public bool IsReceipt { get; set; }
}
//  ItemDiscountStatment
public class ItemDiscountFinancialRequest : RSimpleRequest
{
    public bool IsReceipt { get; set; }

    public Identifier UnitId { get; set; }
    public Identifier ItemID { get; set; }
    public decimal Discount { get; set; }
    public decimal Qauntity { get; set; }
    public DateTime ExpireDate { get; set; }
}

public class MarketerPercentRequest : RSimpleRequest
{
    public decimal Percent { get; set; }
}

public class DiscountPercentRequest : RSimpleRequest
{

}

public class DiscountItemBasedRequest : RSimpleRequest
{
    public Identifier MarketerId { get; set; }
    public Identifier AccountId { get; set; }
    public Identifier ClientId { get; set; }
    public Identifier ItemId { get; set; }
    public Identifier UnitId { get; set; }
    public DateTime ExpireDate { get; set; }
    public decimal Quantity { get; set; }
}


public class GetExpensesRequest : GetAccountsRequest
{

}

public class GetEmployeesRequest : GetAccountsRequest
{

}

public class GetClientsRequest : GetAccountsRequest
{

}

public class GetSuppliersRequest : GetAccountsRequest
{

}

public class GetAccountsRequest
{
    public string SearchText { get; set; } = string.Empty;
}
public interface ISimpleRequest
{
    public Identifier Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public Identifier FromAccount { get; set; }
    public Identifier ToAccount { get; set; }
    public string Note { get; set; }

    public OprationTypeEnum OprationTypeEnum { get; set; }
}