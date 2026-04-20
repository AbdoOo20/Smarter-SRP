using System.Globalization;

namespace Leapay.Client.UserInterface.Common.HomePage;

public class PeriodCalculationResult
{
    public DateTime EndDate { get; set; }
    
    public double TotalProfit { get; set; }
    
    public double SalesTimesQuantity { get; set; }
    
    public double CostTimesQuantity { get; set; }

    public static PeriodCalculationResult ParseString(string? representation, PeriodCalculationResult allTime)
    {
        if (string.IsNullOrWhiteSpace(representation))
            return new();

        var parts = representation.Split('_');
        if (parts.Length != 4)
            throw new FormatException("Invalid representation format.");

        return new PeriodCalculationResult
        {
            CostTimesQuantity = double.Parse(parts[0], CultureInfo.InvariantCulture) + allTime.CostTimesQuantity,
            SalesTimesQuantity = double.Parse(parts[1], CultureInfo.InvariantCulture) + allTime.SalesTimesQuantity,
            TotalProfit = double.Parse(parts[2], CultureInfo.InvariantCulture) + allTime.TotalProfit,
            EndDate = new DateTime(long.Parse(parts[3], CultureInfo.InvariantCulture))
        };
    }
}