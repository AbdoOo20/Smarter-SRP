namespace Leapay.Windows.Printing;

public class ReceiptSaleItem
{
    public string? ItemName { get; set; }
    
    public decimal SellPrice { get; set; }
    
    public decimal Discount { get; set; }
    
    public decimal Quantity { get; set; }
}