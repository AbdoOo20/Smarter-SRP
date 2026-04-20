namespace Fly.Core.Modules.F.PurchaseAndSales.Controller;

class ProductCostAcounter
{
    public decimal Quantity { get; set; }
    public decimal CostPerUnit { get; set; }

    public ProductCostAcounter(decimal quantity, decimal costPerUnit)
    {
        Quantity = quantity;
        CostPerUnit = costPerUnit;
    }

    public void UpdateCost(decimal purchasedUnits, decimal purchaseCostPerUnit)
    {
        // Calculate total costs before purchase
        decimal totalCostBefore = Quantity * CostPerUnit;

        // Calculate total costs after purchase
        decimal totalCostAfter = totalCostBefore + (purchasedUnits * purchaseCostPerUnit);

        // Update quantity
        Quantity += purchasedUnits;

        if (Quantity is 0)
            Quantity = 1;
        
        // Calculate new average cost
        CostPerUnit = totalCostAfter / Quantity;
    }
}