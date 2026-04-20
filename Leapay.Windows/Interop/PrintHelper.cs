using System.Drawing.Printing;
using ZXing;

#if WINDOWS

namespace Leapay.Windows.Interop;

public static class PrintHelper
{
    public static string? SelectPrinter()
    {
        using var printDialog = new PrintDialog
        {
            AllowPrintToFile = false,
            AllowSelection = false,
            AllowSomePages = false,
            UseEXDialog = true
        };

        if (printDialog.ShowDialog() == DialogResult.OK) return printDialog.PrinterSettings.PrinterName;

        return null;
    }

    public static T Cascade<T>(this T printer, params Action<T>[] printerAct)
    {
        foreach (var action in printerAct) action.Invoke(printer);

        return printer;
    }

    public static void PrintBarcodeStickers(
        string barcodeValue,
        int count,
        string itemName,
        decimal price)
    {
        var printDoc = new PrintDocument();

        printDoc.PrintPage += (sender, e) =>
        {
            var stickerWidth = 250;
            var stickerHeight = 120;
            var marginLeft = 20;
            var marginTop = 20;

            var writer = new BarcodeWriter<Image>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 200,
                    Height = 50,
                    Margin = 0
                }
            };

            var barcodeImg = writer.Write(barcodeValue);

            for (var i = 0; i < count; i++)
            {
                var top = marginTop + i * (stickerHeight + 10);

                e.Graphics.DrawRectangle(Pens.Black,
                    marginLeft, top, stickerWidth, stickerHeight);

                e.Graphics.DrawImage(barcodeImg,
                    marginLeft + 10, top + 10, 200, 50);

                e.Graphics.DrawString(
                    itemName,
                    new Font("Arial", 10, FontStyle.Bold),
                    Brushes.Black,
                    marginLeft + 10,
                    top + 70);

                e.Graphics.DrawString(
                    $"{price:F2}$",
                    new Font("Arial", 10),
                    Brushes.Black,
                    marginLeft + 10,
                    top + 90);
            }

            e.HasMorePages = false;
        };
        
        PrintPreviewDialog preview = new PrintPreviewDialog
        {
            Document = printDoc,
            Width = 1200,
            Height = 800
        };
        preview.ShowDialog();
    }
}
#endif