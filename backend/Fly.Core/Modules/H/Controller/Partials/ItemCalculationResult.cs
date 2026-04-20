namespace Fly.Core.Modules.H.Controller;

public class ItemCalculationResult
{
    public string[] Row { get; set; } = [];
    public decimal Quantity { get; set; }
    public decimal OverallQuantity { get; set; }
    public decimal Cost { get; set; }
    public decimal Sales { get; set; }
    public decimal Profit { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal SalesTimesQuantity { get; set; }
    public decimal CostTimesQuantity { get; set; }
}