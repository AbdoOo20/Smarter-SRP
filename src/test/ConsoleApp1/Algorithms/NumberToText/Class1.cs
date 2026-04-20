namespace ConsoleApp1.Algorithms.NumberToText;

public class NumberToArabicTextV5
{
    private static readonly string[] ones = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
    private static readonly string[] tens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
    private static readonly string[] hundreds = { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
    private static readonly string[] thousands = { "", "ألف", "ألفان", "ثلاثة آلاف", "أربعة آلاف", "خمسة آلاف", "ستة آلاف", "سبعة آلاف", "ثمانية آلاف", "تسعة آلاف" };
    private static readonly string[] millions = { "", "مليون", "مليونان", "ثلاثة ملايين", "أربعة ملايين", "خمسة ملايين", "ستة ملايين", "سبعة ملايين", "ثمانية ملايين", "تسعة ملايين" };
    private static readonly string[] billions = { "", "مليار", "ملياران", "ثلاثة مليارات", "أربعة مليارات", "خمسة مليارات", "ستة مليارات", "سبعة مليارات", "ثمانية مليارات", "تسعة مليارات" };

    public static string ToArabicText(long number)
    {
        if (number == 0)
            return "صفر";

        string text = "";
        if (number < 0)
        {
            text += "سالب ";
            number = -number;
        }

        if (number >= 1000000000)
        {
            text += billions[(int)(number / 1000000000)] + " ";
            number %= 1000000000;
        }

        if (number >= 1000000)
        {
            text += millions[(int)(number / 1000000)] + " ";
            number %= 1000000;
        }

        if (number >= 1000)
        {
            text += thousands[(int)(number / 1000)] + " ";
            number %= 1000;
        }

        if (number >= 100)
        {
            text += hundreds[(int)(number / 100)] + " ";
            number %= 100;
        }

        if (number >= 20)
        {
            text += tens[(int)(number / 10)] + " ";
            number %= 10;
        }
        else if (number >= 11)
        {
            text += "أحد عشر";
            number = 0;
        }
        else if (number >= 10)
        {
            text += "عشرة";
            number = 0;
        }

        if (number > 0)
        {
            text += ones[number];
        }

        return text.Trim();
    }

    internal static string GetText(decimal number)
    {
        return ToArabicText((long)number);
    }
}