using SkiaSharp;
using ZXing;
using ZXing.Common;

namespace Leapay.Windows.Interop;

public static class BarcodePrinter
{
    public static void PrintBarcodeLabel(
        string barcodeData,
        string text,
        string price,
        int count)
    {
        var printerName = PrintHelper.SelectPrinter();
        
        for (int i = 0; i < count; i++)
        {
            byte[] pngBytes = RenderLabel(barcodeData, text, price);

            if (i == 1)
            {
#if DEBUG
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"LabelPreview_{DateTime.Now:HHmmss}.png"
                );
                File.WriteAllBytes(path, pngBytes);
#endif
            }
            
            RawPrinterHelperBytes.SendBytesToPrinter(printerName, pngBytes);
        }
    }
    
    private static SKBitmap GenerateBarcode(string value, int width, int height)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 2,
                PureBarcode = true
            }
        };

        var pixelData = writer.Write(value);

        var bmp = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul);

        unsafe
        {
            fixed (byte* ptr = pixelData.Pixels)
            {
                bmp.InstallPixels(bmp.Info, (IntPtr)ptr, bmp.Info.RowBytes);
            }   
        }


        return bmp;
    }
    
    private static byte[] RenderLabel(
        string barcode,
        string text,
        string price)
    {
        int width = 420;
        int height = 260;

        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);
        
        var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 32,
            Typeface = SKTypeface.FromFamilyName("Arial"),
            IsAntialias = true
        };

        var pricePaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 40,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold),
            IsAntialias = true
        };

        float CenterX(SKPaint p, string t) => (width - p.MeasureText(t)) / 2;

        float arabicY = 60;
        canvas.DrawText(text, CenterX(textPaint, text), arabicY, textPaint);

        float priceY = arabicY + 50;
        canvas.DrawText(price, CenterX(pricePaint, price), priceY, pricePaint);
                
        var barcodeBmp = GenerateBarcode(barcode, 400, 120);
        using var skImg = SKImage.FromBitmap(barcodeBmp);
        canvas.DrawImage(skImg, new SKPoint(10, priceY + 50));

        using var img = SKImage.FromBitmap(bitmap);
        using var data = img.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }
}