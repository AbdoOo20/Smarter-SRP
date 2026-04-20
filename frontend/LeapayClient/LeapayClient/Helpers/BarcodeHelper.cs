using Leapay.Client.ClientModels.Ui;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using System.Diagnostics;

namespace Leapay.Client.Helpers;

public static class BarcodeHelper
{
    public static void ExportPdf(List<BarcodeItem> items)
    {
        var tempPath = Path.GetTempPath();
        var fileName = $"Stickers_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        var fullPath = Path.Combine(tempPath, fileName);

        const float a4Width = 595;
        const float a4Height = 842;

        using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        using var document = SKDocument.CreatePdf(stream);

        const int cols = 3;
        const int rows = 8;
        const float margin = 20;
        const float cellWidth = (a4Width - 2 * margin) / cols;
        const float cellHeight = (a4Height - 2 * margin) / rows;
        const int itemsPerPage = cols * rows;

        for (var pageIndex = 0; pageIndex * itemsPerPage < items.Count; pageIndex++)
        {
            using var canvas = document.BeginPage(a4Width, a4Height);
            canvas.Clear(SKColors.White);

            for (var i = 0; i < itemsPerPage; i++)
            {
                var globalIndex = pageIndex * itemsPerPage + i;
                if (globalIndex >= items.Count) break;

                var item = items[globalIndex];
                var row = i / cols; // about 12, depending on width
                var col = i % cols; // about 6?

                var x = margin + col * cellWidth;
                var y = margin + row * cellHeight;

                var rect = new SKRect(x, y, x + cellWidth, y + cellHeight);
                // ReSharper disable once UsingStatementResourceInitialization
                using var borderPaint = new SKPaint { Color = SKColors.LightGray, Style = SKPaintStyle.Stroke };
                canvas.DrawRect(rect, borderPaint);

                DrawCentredText(item.Name, x, cellWidth, canvas, y + 20);
                DrawCentredText(item.Price, x, cellWidth, canvas, y + 33);

                var barcodeBmp = GenerateBarcode(item.BarcodeValue, (int)(cellWidth - 16), 50);
                using var skImg = SKImage.FromBitmap(barcodeBmp);
                var barcodeRect = new SKRect(x + 8, y + 45, x + 8 + (cellWidth - 16), y + 88);
                canvas.DrawImage(skImg, barcodeRect);
            }

            document.EndPage();
        }

        document.Close();

        Process.Start(new ProcessStartInfo
        {
            FileName = fullPath,
            UseShellExecute = true
        });
    }

    private static void DrawCentredText(string text, float x, float cellWidth, SKCanvas canvas, float y)
    {
        using var paint = new SKPaint
        {
            TextSize = 12,
            Typeface = SKTypeface.Default,
            IsAntialias = true
        };

        var textWidthPaint = paint.MeasureText(text);
        var centeredXPaint = x + (cellWidth - textWidthPaint) / 2;

        canvas.DrawText(text, centeredXPaint, y, paint);
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
}