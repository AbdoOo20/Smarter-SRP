namespace Fly.Core.Modules.H.Controller;

public class PeriodCalculationResult
{
    public DateTime EndDate { get; set; } = new();

    public List<string[]> Rows { get; set; } = new();
    
    public decimal Quantity { get; set; }
    
    public decimal OverallQuantity { get; set; }
    
    public decimal Cost { get; set; }
    
    public decimal Sales { get; set; }
    
    public decimal Profit { get; set; }
    
    public decimal TotalProfit { get; set; }
    
    public decimal SalesTimesQuantity { get; set; }
    
    public decimal CostTimesQuantity { get; set; }

    public string GetRepresentation()
    {
        return $"{CostTimesQuantity}_{SalesTimesQuantity}_{TotalProfit}_{EndDate.Ticks}";
    }
}