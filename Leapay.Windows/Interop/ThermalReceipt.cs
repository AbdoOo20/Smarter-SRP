using System.Collections.Frozen;
using System.Text;
using ESC_POS_USB_NET.Printer;
using Leapay.Windows.Printing;
using SkiaSharp.QrCode;
using SkiaSharp.QrCode.Image;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using Color = System.Drawing.Color;

namespace Leapay.Windows.Interop;

public class ThermalReceipt
{
    private readonly long _receiptNumber;
    public byte[] CompanyImage { get; }

    public ThermalReceipt(string companyName, string contactInfo, byte[] companyImage, decimal totalBill, int paperWidth = 32)
    {
        CompanyName = companyName;
        ContactInfo = contactInfo;
        PaperWidth = paperWidth;
        CompanyImage = companyImage;
        TotalBill = totalBill;
    }

    public ThermalReceipt(List<ReceiptSaleItem> items,
        string companyName,
        string contactInfo,
        byte[] companyImage,
        decimal totalBill,
        long receiptNumber,
        int paperWidth = 32): this(companyName, contactInfo, companyImage, totalBill, paperWidth)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        _receiptNumber = receiptNumber;
        foreach (var item in items) 
            AddItem(item);
    }
    
    private List<ReceiptSaleItem> Items { get; } = [];

    private string CompanyName { get; }

    private string ContactInfo { get; }
    
    private decimal TotalBill { get; }

    private int PaperWidth { get; }

    public void AddItem(ReceiptSaleItem item)
    {
        Items.Add(item);
    }

    public string GenerateReceipt(string barcodeData = null)
    {
        var sb = new StringBuilder();

        var center = "\x1B\x61\x01";
        var right = "\x1B\x61\x02";
        var left = "\x1B\x61\x00";
        var boldOn = "\x1B\x45\x01";
        var boldOff = "\x1B\x45\x01";
        var cut = "\x1D\x56\x00";

        var totalDiscount = 0m;
        var finalPrice = 0m;

        sb.AppendLine(center + CompanyName);
        sb.AppendLine(center + ContactInfo);
        sb.AppendLine(left + new string('-', PaperWidth));

        sb.AppendLine(left + "Item".PadRight(14) + "Qty  Price Disc  Total");

        foreach (var item in Items)
        {
            var lineTotal = item.SellPrice * item.Quantity;
            var discount = item.Discount * item.Quantity;
            var finalLinePrice = lineTotal - discount;

            totalDiscount += discount;
            finalPrice += finalLinePrice;

            var lines = WrapText(item.ItemName, 14);
            for (var i = 0; i < lines.Count; i++)
                if (i == 0)
                    sb.AppendLine(
                        $"{lines[i].PadRight(14)} {item.Quantity,3} {item.SellPrice,6:0.00} {discount,5:0.00} {finalLinePrice,6:0.00}"
                    );
                else
                    sb.AppendLine(lines[i]);
        }

        sb.AppendLine(new string('-', PaperWidth));

        sb.AppendLine(boldOn + $"TOTAL_DISCOUNT: {totalDiscount:0.00}".PadLeft(PaperWidth) + boldOff);
        sb.AppendLine(boldOn + $"FINAL_PRICE:   {finalPrice:0.00}".PadLeft(PaperWidth) + boldOff);

        if (!string.IsNullOrEmpty(barcodeData))
        {
            sb.AppendLine(center);
            sb.Append("\x1D\x6B\x49" + (char)barcodeData.Length + barcodeData);
        }

        sb.AppendLine(center + "THANK_NOTE");

        sb.Append(cut);

        return sb.ToString();
    }

    public void GenerateReceiptUsingEps(FrozenDictionary<string, string> locale, decimal discount, bool ltr)
    {
        #if WINDOWS
            var printerName = PrintHelper.SelectPrinter();
            if (printerName == null) 
                return;

            var totalPrice = 0m;
            var setArabicCodepage = new byte[] { 0x1B, 0x74, 0x10 };

            var printer = new Printer(printerName, "CP864")
                .Cascade(p =>p.Append(setArabicCodepage), p =>
                    {
                        if (CompanyImage.Length == 0)
                            return;
                        
                        using var ms = new MemoryStream(CompanyImage);
                        using var bitmap = new Bitmap(ms);
                        p.Image(bitmap);
                    },
                    p => Align(p),
                    p => p.Append(CompanyName),
                    p => p.Append(ContactInfo),
                    p => p.AlignCenter(),
                    p => p.Append($"          {locale["SalesInvoice"]}          "),
                    p => p.Separator(),
                    p => Align(p),
                    p => p.Append($"{locale["ItemName"]}         {locale["Price"]}   {locale["Quantity"]}  {locale["Discount"]}  {locale["FinalPrice"]}"),
                    p => p.Separator(),
                    p =>
                    {
                        Align(p);
                        
                        foreach (var item in Items)
                        {
                            var price = (item.SellPrice - item.Discount) * item.Quantity;
                            totalPrice += price;
                            
                            var line = string.Format("{0,-10}{1,6:0.00}{2,5}{3,6:0.00}{4,7:0.00}",
                                item.ItemName?.Length > 10 ? item.ItemName[..10] : item.ItemName,
                                item.SellPrice,
                                item.Quantity,
                                discount,
                                price
                            );
                            
                            p.Append(line);
                        }
                    },
                    p => p.Separator(),
                    p => Align(p),
                    p => p.Append($"{locale["TotalPrice"]}: {totalPrice}"),
                    p => p.Append($"{locale["InvoiceDiscount"]}: {discount}"),
                    p => p.Append($"{locale["FinalPrice"]}: {totalPrice - discount}"),
                    p => p.Separator(),
                    p => p.AlignCenter(), 
                    p => p.Append(locale["ThanksReceiptNote"]),
                    p => p.FullPaperCut());

            printer.PrintDocument();

            void Align(Printer p, bool rev = false)
            {
                if(ltr && !rev)
                    p.AlignLeft();
                else p.AlignRight();
            }

            void Pad(Printer p, int count = 3)
            {
                p.AppendWithoutLf(new string(' ', count));
            }
            
        #endif
    }
    
    private static List<string> WrapText(string? text, int maxLength)
    {
        var lines = new List<string>();
        while (text?.Length > maxLength)
        {
            lines.Add(text.Substring(0, maxLength));
            text = text.Substring(maxLength);
        }
        lines.Add(text ?? "");
        return lines;
    }

    public void PrintReceiptAsImage(FrozenDictionary<string, string> locale, decimal discount, bool ltr)
    {
        var totalPrice = 0m;

        var barcodeContent =
            $"customerId={Guid.NewGuid()};customerAccountNumber={Random.Shared.Next(10000000, 999999999)};receiptTotal={TotalBill}";
        var stringFormat = new StringFormat();

        /*if (!ltr)
        {
            stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
        }*/
        
        using var bmp = RenderReceipt();
        var printerName = PrintHelper.SelectPrinter();
        if (printerName == null)
            return;
        var printer = new Printer(printerName);
        #if DEBUG
        bmp.Save("D:\\test-receipt.png");
        #endif
        printer.Image(bmp);
        printer.FullPaperCut();
        printer.PrintDocument();
        return;
        
        Bitmap RenderReceipt()
        {
            var width = 550; // ~80mm
            var rowHeight = 40;
            var margin = 5;

            // dynamic height: header + items + footer
            var height = 300 + Items.Count * rowHeight + 200 + 150;
            var bmp = new Bitmap(width, height);

            using var g = Graphics.FromImage(bmp);
            
            g.Clear(Color.White);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            var headerFont = new Font("Tahoma", 16, FontStyle.Bold);
            var normalFont = new Font("Tahoma", 12);
            var boldFont = new Font("Tahoma", 13, FontStyle.Bold);
            var boldTitleFont = new Font("Tahoma", 15, FontStyle.Bold);

            float y = margin;

            // --- Company logo ---
            if (CompanyImage?.Length > 0)
            {
                using var ms = new MemoryStream(CompanyImage);
                using var logo = new Bitmap(ms);
                logo.SetResolution(100, 100);
                var x = (width / 2) - 50;
                g.DrawImage(logo, x, (int)y, 100, 100);
                y += 70;
            }
            var totalsMargin = margin + 330;

            g.DrawString(CompanyName, headerFont, Brushes.Black, new PointF(totalsMargin + 40, y), stringFormat);
            y += 50;

            g.DrawString(ContactInfo, normalFont, Brushes.Black, new PointF(totalsMargin + 30, y), stringFormat);
            y += 40;

            var titleSize = g.MeasureString(locale["SalesInvoice"], boldFont);
            g.DrawString(locale["SalesInvoice"], boldTitleFont, Brushes.Black,
                new PointF((width - titleSize.Width) / 2, y), stringFormat);
            y += 55;

            DrawLine(g, ref y, width, margin);

            int[] colWidths = [220, 80, 80, 100, 120]; // sum <= width - 2*margin
            colWidths = colWidths.Reverse().ToArray();
            string[] headers =
            {
                " " + locale["Item"], locale["Price"], locale["Quantity"],
                locale["Discount"], locale["Total"]
            };

            DrawRow(g, headers.Reverse().ToArray(), colWidths, margin, ref y, boldFont, Brushes.Black);

            DrawLine(g, ref y, width, margin);

            var totalPrice = 0m;
            
            foreach (var item in Items)
            {
                var price = (item.SellPrice - item.Discount) * item.Quantity;
                totalPrice += price;

                string[] row =
                [
                    item.ItemName?.Length > 25 ? item.ItemName[..25] : item.ItemName,
                    item.SellPrice.ToString("0.00"),
                    item.Quantity.ToString(),
                    item.Discount.ToString("0.00"),
                    price.ToString("0.00")
                ];

                DrawRow(g, row.Reverse().ToArray(), colWidths, margin, ref y, normalFont, Brushes.Black);
            }

            DrawLine(g, ref y, width, margin);
            var totalsFormat = new StringFormat {  };

            g.DrawString($"{locale["TotalPrice"]}: {totalPrice:0.00}", normalFont, Brushes.Black, new PointF(totalsMargin + 45, y), totalsFormat);
            y += 40;
            g.DrawString($"{locale["InvoiceDiscount"]}: {discount:0.00}", normalFont, Brushes.Black,
                new PointF(totalsMargin + 45, y), totalsFormat);
            y += 40;
            g.DrawString($"{locale["FinalPrice"]}: {totalPrice - discount:0.00}", normalFont, Brushes.Black,
                new PointF(totalsMargin + 15, y), totalsFormat);
            y += 50;

            DrawLine(g, ref y, width, margin);

            var thanksSize = g.MeasureString(locale["ThanksReceiptNote"], boldFont);
            g.DrawString(locale["ThanksReceiptNote"], boldFont, Brushes.Black,
                new PointF((width - thanksSize.Width) / 2, y), stringFormat);
            y += thanksSize.Height + 10;
            const int qrCodeSize = 140;

            var qrCode = new QRCodeImageBuilder(barcodeContent)
                .WithSize(qrCodeSize, qrCodeSize)
                .WithErrorCorrection(ECCLevel.H)
                .ToByteArray();
            using (var qr = System.Drawing.Image.FromStream(new MemoryStream(qrCode)))
            {
                var x = (width - qrCodeSize) / 2;
                g.DrawImage(qr, x, y, qrCodeSize, qrCodeSize);
            }

            var date = DateTime.Now.ToString("F");
            var dateSize = g.MeasureString(date, boldFont);
            g.DrawString(date, boldFont, Brushes.Black,
                new PointF((width - dateSize.Width) / 2, y), stringFormat);
            
            return bmp;
        }

        void DrawRow(Graphics g, string[] cols, int[] colWidths, int margin, ref float y, Font font, Brush brush)
        {
            var x = margin;
            for (var i = 0; i < cols.Length; i++)
            {
                g.DrawString(cols[i], font, brush, new RectangleF(x, y, colWidths[i], 40), stringFormat);
                x += colWidths[i];
            }

            y += 40;
        }

        void DrawLine(Graphics g, ref float y, int width, int margin)
        {
            g.DrawLine(Pens.Black, margin, y, width - margin, y);
            y += 10;
        }
    }

    public void PrintUsb(string printerName, string? barcode = null)
    {
        var data = GenerateReceipt(barcode);
        RawPrinterHelper.SendStringToPrinter(printerName, data);
    }
    
    public void PrintToSelectedPrinter(string? barcode = null)
    {
        #if WINDOWS
        
        var printerName = PrintHelper.SelectPrinter();
        if (printerName == null)
            return;

        PrintUsb(printerName, barcode);
        #endif
    }
    
    public void PrintReceiptAsImageQuest(FrozenDictionary<string, string> locale, decimal discount, bool ltr)
    {
        var barcodeContent =
            $"customerId={Guid.NewGuid()};customerAccountNumber={Random.Shared.Next(10000000, 999999999)};receiptTotal={TotalBill}";

        var doc = new ReceiptDocument(
            locale,
            Items,
            discount,
            CompanyImage,
            CompanyName,
            ContactInfo,
            barcodeContent,
            ltr,
            _receiptNumber
        );

        var dir = Path.Combine(Path.GetTempPath(), $"{Random.Shared.Next(10000000, 999999999)}", "receipt");
        Directory.CreateDirectory(dir);

        doc.GenerateImages(i => Path.Combine(dir, $"p{i}.png"),
            new ImageGenerationSettings { RasterDpi = 203 });

        using var bmp = StitchImages(Directory.GetFiles(dir, "p*.png"));

#if DEBUG
        bmp.Save("D:\\test-receipt.png");
#endif

        var printerName = PrintHelper.SelectPrinter();
        if (printerName == null)
            return;

        var printer = new Printer(printerName);
        printer.Image(bmp);
        printer.FullPaperCut();
        printer.PrintDocument();
    }
    
    static Bitmap StitchImages(IEnumerable<string> files)
    {
        var bitmaps = files.Select(f =>
        {
            using var s = File.OpenRead(f);
            return SKBitmap.Decode(s);
        }).ToList();

        var width = bitmaps.Max(b => b.Width);
        var height = bitmaps.Sum(b => b.Height);

        var final = new SKBitmap(width, height);
        using var canvas = new SKCanvas(final);
        canvas.Clear(SKColors.White);

        int y = 0;
        foreach (var bmp in bitmaps)
        {
            canvas.DrawBitmap(bmp, 0, y);
            y += bmp.Height;
        }

        using var image = SKImage.FromBitmap(final);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var ms = new MemoryStream();
        data.SaveTo(ms);

        return new Bitmap(ms);
    }
}

public sealed class ReceiptDocument : IDocument
{
    private readonly FrozenDictionary<string, string> _locale;
    private readonly IReadOnlyList<ReceiptSaleItem> _items;
    private readonly decimal _discount;
    private readonly byte[]? _companyImage;
    private readonly string _companyName;
    private readonly string _contactInfo;
    private readonly string _barcodeContent;
    private readonly bool _ltr;
    private readonly long _number;

    public ReceiptDocument(
        FrozenDictionary<string, string> locale,
        IReadOnlyList<ReceiptSaleItem> items,
        decimal discount,
        byte[]? companyImage,
        string companyName,
        string contactInfo,
        string barcodeContent,
        bool ltr,
        long number)
    {
        _locale = locale;
        _items = items;
        _discount = discount;
        _companyImage = companyImage;
        _companyName = companyName;
        _contactInfo = contactInfo;
        _barcodeContent = barcodeContent;
        _ltr = ltr;
        _number = number;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(576, 200);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(25));

            (_ltr ? page.Content().ContentFromLeftToRight() : page.Content().ContentFromRightToLeft()).Column(col =>
            {
                // Logo
                if (_companyImage?.Length > 0)
                {
                    col.Item().AlignCenter().Width(100).Height(100)
                        .Image(_companyImage);
                }

                col.Item().AlignCenter().Text(_companyName).Bold().FontSize(27);
                col.Item().AlignCenter().Text(_contactInfo);

                col.Item().PaddingVertical(10)
                    .AlignCenter()
                    .Text(_locale["SalesInvoice"] + $" #{_number}")
                    .Bold()
                    .FontSize(28);

                col.Item().PaddingVertical(2).LineHorizontal(1);

                // Table
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(3);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(1);
                        cols.RelativeColumn(1);
                    });

                    void Header(string t) =>
                        table.Cell().PaddingBottom(5).Element(CellStyle).Text(t).Bold();

                    Header(_locale["Item"]);
                    Header(_locale["Price"]);
                    Header(_locale["Quantity"]);
                    Header(_locale["Discount"]);
                    Header(_locale["Total"]);

                    decimal total = 0;

                    foreach (var item in _items)
                    {
                        var price = (item.SellPrice - item.Discount) * item.Quantity;
                        total += price;

                        table.Cell().Text(item.ItemName.Length > 25
                            ? item.ItemName[..25]
                            : item.ItemName);

                        table.Cell().AlignRight().Text(item.SellPrice.ToString("0.00"));
                        table.Cell().AlignCenter().Text(item.Quantity.ToString());
                        table.Cell().AlignRight().Text(item.Discount.ToString("0.00"));
                        table.Cell().AlignRight().Text(price.ToString("0.00"));
                    }

                    static IContainer CellStyle(IContainer c) =>
                        c.PaddingVertical(5).BorderBottom(1);
                });

                col.Item().PaddingVertical(10).LineHorizontal(1);

                var totalPrice = _items.Sum(i => (i.SellPrice - i.Discount) * i.Quantity);

                col.Item().AlignRight().Text($"{_locale["TotalPrice"]}: {totalPrice:0.00}");
                col.Item().AlignRight().Text($"{_locale["InvoiceDiscount"]}: {_discount:0.00}");
                col.Item().AlignRight().Text($"{_locale["FinalPrice"]}: {totalPrice - _discount:0.00}")
                    .Bold();

                col.Item().PaddingVertical(10).LineHorizontal(1);

                col.Item().AlignCenter().Text(_locale["ThanksReceiptNote"]).Bold();

                col.Item().AlignCenter().Width(140).Height(140)
                    .Background(Colors.White)
                    .Svg(size =>
                    {
                        var writer = new QRCodeWriter();
                        var qrCode = writer.encode(_barcodeContent, BarcodeFormat.QR_CODE, (int)size.Width, (int)size.Height);
                        var renderer = new SvgRenderer { FontName = "Lato" };
                        return renderer.Render(qrCode, BarcodeFormat.EAN_13, null).Content;
                    });

                col.Item().AlignCenter()
                    .Text(DateTime.Now.ToString("G"))
                    .FontSize(18);

                col.Item().AlignCenter().PaddingVertical(5)
                    .Text("LeaPay ERP Solutions. MmTech. Copyright 2026 (C). All rights reserved.").FontSize(16);
            });
        });
    }
}
