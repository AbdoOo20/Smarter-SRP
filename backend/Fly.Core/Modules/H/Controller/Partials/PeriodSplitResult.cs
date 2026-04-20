namespace Fly.Core.Modules.H.Controller;

public class PeriodSplitResult
{
    public PeriodCalculationResult AllTimeBeforePeriod { get; set; } = new();
    
    public PeriodCalculationResult FullPeriod { get; set; } = new();
    
    public List<PeriodCalculationResult> Periods { get; set; } = new();
    
    public PeriodCalculationResult FirstQuarter { get; set; } = new();
    
    public PeriodCalculationResult SecondQuarter { get; set; } = new();
    
    public PeriodCalculationResult ThirdQuarter { get; set; } = new();
    
    public PeriodCalculationResult FourthQuarter { get; set; } = new();
}