using CommunityToolkit.Mvvm.ComponentModel;
using Fly.Framework.Shared.Common.Dtos;

namespace Fly.Shared.Modules.Global.Dtos;

public partial class DtoCompanyInfo : DtoBase
{
    public string? Name { get; set; }
    public string? NameEn { get; set; }

    public string? Type { get; set; }
    public string? TypeEn { get; set; }

    public string? Image { get; set; }

    [ObservableProperty]
    private byte[] _imageData = [];

    public string? Address { get; set; }
    
    public string? PhoneNumber1 { get; set; }
    
    public string? PhoneNumber2 { get; set; }

    public string? PhoneNumber3 { get; set; }
    
    public string? Email { get; set; }
    
    public string? InvoiceNotice { get; set; }
    
    public bool AddBarcodeAutomatically { get; set; }
}