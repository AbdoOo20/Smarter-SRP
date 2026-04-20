using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Enums;
using Fly.Shared.Modules.Financial.Requests;

namespace Fly.Shared.Modules.Accounting.Requests;

public class RSimpleRequest : ISimpleRequest
{
    public Identifier Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalAmount { get; set; }
    public Identifier FromAccount { get; set; }
    public Identifier ToAccount { get; set; }
    
    public string? FromName { get; set; }
    
    public string? ToName { get; set; }
    
    public string Note { get; set; } = string.Empty;
    public long DisplayNumber { get; set; }
    public long SerialNumber { get; set; }
    public long Number { get; set; }
    public int Type { get; set; }

    public string? InternalNotes { get; set; }
    
    public OprationTypeEnum OprationTypeEnum { get; set; } = OprationTypeEnum.None;

    public static RSimpleRequest Create(RSimpleRequest simpleRequest, OprationTypeEnum oprationTypeEnum)
    {
        simpleRequest.OprationTypeEnum = oprationTypeEnum;

        return new RSimpleRequest()
        {
            Amount = simpleRequest.Amount,
            FromAccount = simpleRequest.FromAccount,
            Date = simpleRequest.Date,
            Note = simpleRequest.Note,
            ToAccount = simpleRequest.ToAccount,
            SerialNumber = simpleRequest.SerialNumber,
        };
    }
    public static RSimpleRequest Create(long serialNumber, decimal amount, Identifier fromAccount, Identifier toAccount, DateTime date, string note, OprationTypeEnum oprationTypeEnum)
    {
        var simpleRequest = new RSimpleRequest()
        {
            Amount = amount,
            FromAccount = fromAccount,
            Date = date,
            Note = note,
            ToAccount = toAccount,
            OprationTypeEnum = oprationTypeEnum,
            SerialNumber = serialNumber
        };
        return simpleRequest;
    }

    public virtual DtoEntry CreateEntry()
    {
        return new DtoEntry()
        {
            Date = Date,
            OprationType = OprationTypeEnum,
            DisplayNumber = DisplayNumber,
            SerialNumber = SerialNumber,
            Note = Note,

            Details = new List<DtoEntryDetail>()
            {
                new DtoEntryDetail()
                {
                    Credit = Amount,
                    AccountId = FromAccount,
                },
                new DtoEntryDetail()
                {
                    Debit = Amount,
                    AccountId = ToAccount,
                }
            }
        };
    }

    public string? Notes { get; set; }
    public string? RemainText { get; set; }
    public decimal? RemainAmount { get; set; }
    public object? ApiResponse { get; set; }
}