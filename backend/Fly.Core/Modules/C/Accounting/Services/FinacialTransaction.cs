using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Core.Modules.C.Accounting.Services;

public class FinacialTransaction
{
    private readonly AccountingFinacial accountingFinacial;

    public DateTime Date { get; set; }
    public OprationTypeEnum? OprationType { get; set; } = OprationTypeEnum.None;
    public int? RelatedIndex { get; set; }
    public Identifier? RelatedId { get; set; }
    public string? Note { get; set; }
    public long DisplayNumber { get; set; }
    public TagTypeEnum? TagType { get; set; }


    public List<decimal> Amounts { get; set; }
    public List<Identifier> DebitAccounts { get; set; }
    public List<Identifier> CreditAccounts { get; set; }
    public List<RcEntryDetail> EntryDetails { get; set; }

    public FinacialTransaction(AccountingFinacial accountingFinacial, DateTime date, OprationTypeEnum? oprationType, int? relatedIndex, Identifier? relatedId, long displayNumber, string? note, TagTypeEnum? tagType = null)
    {
        Amounts = [];
        DebitAccounts = [];
        CreditAccounts = [];
        EntryDetails = [];
        this.accountingFinacial = accountingFinacial;
        Date = date;
        OprationType = oprationType;
        RelatedIndex = relatedIndex;
        RelatedId = relatedId;
        Note = note;
        DisplayNumber = displayNumber;
        TagType = tagType;
    }

    public FinacialTransaction AppendEntry(Identifier debit, Identifier credit, decimal ammount)
    {
        EntryDetails.Add(new RcEntryDetail() { AccountId = debit, Debit = ammount });
        EntryDetails.Add(new RcEntryDetail() { AccountId = credit, Credit = ammount });

        Amounts.Add(ammount);
        DebitAccounts.Add(debit);
        CreditAccounts.Add(credit);
        return this;
    }

    public FinacialTransaction AppendDebitEntry(Identifier debit, decimal ammount)
    {
        EntryDetails.Add(new RcEntryDetail() { AccountId = debit, Debit = ammount });
            

        Amounts.Add(ammount);
        DebitAccounts.Add(debit);
        return this;
    }
    public FinacialTransaction AppendCreditEntry(Identifier credit, decimal ammount)
    {
        EntryDetails.Add(new RcEntryDetail() { AccountId = credit, Credit = ammount });

        Amounts.Add(ammount);
        CreditAccounts.Add(credit);
        return this;
    }

    //public FinacialTransaction AppendEntry(Identifier[] debit, Identifier[] credit, decimal[] ammountDep, decimal[] ammountCred)
    //{
    //    EntryDetails.AddRange(
    //        debit.Select((id, indx) => new RcEntryDetail() { AccountId = debit[indx], Debit = ammountDep[indx] })
    //    );
    //    EntryDetails.AddRange(
    //        debit.Select((id, indx) => new RcEntryDetail() { AccountId = credit[indx], Credit = ammountCred[indx] })
    //    );

    //    //Amounts.Add(ammount);
    //    DebitAccounts.AddRange(debit);
    //    CreditAccounts.AddRange(credit);
    //    return this;
    //}

    public async Task<AccountingFinacial> ComplateTransaction()
    {
        await accountingFinacial.RegesterTransaction(this);
        return accountingFinacial;
    }

    public async Task<AccountingFinacial> RevertTransaction(string note, bool reverseEntry = false)
    {
        await accountingFinacial.ReverseTransaction(this, note, reverseEntry);
        return accountingFinacial;
    }

    public CreateEntryRequest CreateEntryRequest()
    {
        CreateEntryRequest request = new CreateEntryRequest()
        {
            Date = Date,
            Note = Note,
            Details = EntryDetails,
            DisplayNumber = DisplayNumber,
        };
        return request;
    }

}