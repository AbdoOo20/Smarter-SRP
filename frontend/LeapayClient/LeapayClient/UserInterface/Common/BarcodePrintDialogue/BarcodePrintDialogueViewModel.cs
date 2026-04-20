using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Ui;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Windows.Interop;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Common.BarcodePrintDialogue;

public sealed partial class BarcodePrintDialogueViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    public BarcodePrintDialogueWindow? Window { get; set; }
    
    [ObservableProperty]
    public partial string? ItemName { get; set; }
    
    [ObservableProperty]
    public partial string? ItemBarcode { get; set; }
    
    [ObservableProperty]
    public partial string ItemPrice { get; set; }
    
    [ObservableProperty]
    public partial int ItemCount { get; set; }
    
    public BarcodePrintDialogueViewModel(
        DtoBase? model,
        EntryType entryType)
    {
        SetDetails(model, entryType, GetType());
    }
    
    [RelayCommand]
    private Task OnActionAsync(string parameter)
    {
        if (string.IsNullOrEmpty(ItemBarcode))
        {
            _ = DialogueServices.ShowErrorAsync(this["BarcodeNeedsValue"]);
            return Task.CompletedTask;
        }
        
        if (string.IsNullOrEmpty(ItemName))
        {
            _ = DialogueServices.ShowErrorAsync(this["NameNeedsValue"]);
            return Task.CompletedTask;
        }
        
        if (string.IsNullOrEmpty(ItemPrice))
        {
            _ = DialogueServices.ShowErrorAsync(this["PriceNeedsValue"]);
            return Task.CompletedTask;
        }
        
        if (ItemCount < 1)
        {
            _ = DialogueServices.ShowErrorAsync(this["CountNeedsValue"]);
            return Task.CompletedTask;
        }

        if (ItemCount > 5000)
        {
            _ = DialogueServices.ShowErrorAsync(this["CountLimitIsThousand"]);
            return Task.CompletedTask;
        }

        BarcodePrinter.PrintBarcodeLabel(ItemBarcode, ItemName, ItemPrice, ItemCount);
        
        // print for pdf, which is later...
        /*BarcodeHelper.ExportPdf(Enumerable.Range(1, ItemCount).Select(_ => new BarcodeItem
        {
            Name = ItemName,
            Price = ItemPrice,
            BarcodeValue = ItemBarcode
        }).ToList());*/
        
        return Task.CompletedTask;
    }
}