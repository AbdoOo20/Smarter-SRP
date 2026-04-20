using Fly.Framework.Shared.Common;

namespace Leapay.Client.UserInterface.Sales.SalesPoint.SuspendedInvoices.Models;

public partial class SuspendedInvoiceModel: ObservableObject
{
    [ObservableProperty]
    public partial Identifier InvoiceId { get; set; }

    [ObservableProperty]
    public partial string? ClientName { get; set; }
    
    [ObservableProperty]
    public partial string? ClientPhone { get; set; }
    
    [ObservableProperty]
    public partial long InvoiceNumber { get; set; }
    
    [ObservableProperty]
    public partial string? InvoiceDate { get; set; }
}