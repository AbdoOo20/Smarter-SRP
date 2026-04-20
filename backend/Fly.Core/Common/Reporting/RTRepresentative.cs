namespace Fly.Core.Common.Reporting;

public class RTRepresentative : RTModel
{
    public decimal ClientBalance { get; set; }
    public decimal Balance { get; set; }
    public string SafeName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string MarketerName { get; set; } = string.Empty;
    public string ReceiptNumber { get; set; } = string.Empty;
}