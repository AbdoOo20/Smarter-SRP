namespace Fly.Core.Common.Reporting;

public class RTModel
{
    //  1 = receipt, 2 = pay
    public int Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string TextAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Remaining { get; set; }
    public long Number { get; set; }
    public string TypeName
    {
        get => Type == 1 ? "إيصال قبض" : "إيصال صرف";
        set => _ = value;
    }
    public string TypeText
    {
        get => Type == 1 ? "إستلمتُ من" : "إستلمَ";
        set => _ = value;
    }
}