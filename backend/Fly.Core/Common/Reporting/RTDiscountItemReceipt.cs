namespace Fly.Core.Common.Reporting;

public class RTDiscountItemReceipt : RTModel
{
    public long Number { get; set; }
    public string SafeName { get; set; }
    public string ReceiptNumber { get; set; }
    public decimal Quantity { get; set; }
    public string ItemName { get; set; }
    public string UnitName { get; set; }
    public string ClientName { get; set; }

    public RTDiscountItemReceipt()
    {

    }
    public RTDiscountItemReceipt(DateTime date, decimal amount, string notes, string safeName, string receiptNumber, decimal quantity, string itemName, string clientName, long number)
    {
        Date = date;
        Amount = amount;
        Notes = notes;
        SafeName = safeName;
        ReceiptNumber = receiptNumber;
        Quantity = quantity;
        ItemName = itemName;
        ClientName = clientName;
        Number = number;
    }
}