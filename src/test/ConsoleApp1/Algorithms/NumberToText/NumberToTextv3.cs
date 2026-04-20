namespace ConsoleApp1.Algorithms.NumberToText;

using System.Collections.Generic;

public class NumberToTextv3
{
    private static readonly Dictionary<int, string> units = new()
    {
        { 0, "" },
        { 1, "واحد" },
        { 2, "اثنان" },
        { 3, "ثلاثة" },
        { 4, "أربعة" },
        { 5, "خمسة" },
        { 6, "ستة" },
        { 7, "سبعة" },
        { 8, "ثمانية" },
        { 9, "تسعة" }
    };

    private static readonly Dictionary<int, string> teens = new()
    {
        { 11, "أحد عشر" },
        { 12, "اثنا عشر" },
        { 13, "ثلاثة عشر" },
        { 14, "أربعة عشر" },
        { 15, "خمسة عشر" },
        { 16, "ستة عشر" },
        { 17, "سبعة عشر" },
        { 18, "ثمانية عشر" },
        { 19, "تسعة عشر" }
    };

    private static readonly Dictionary<int, string> tens = new()
    {
        { 2, "عشرون" },
        { 3, "ثلاثون" },
        { 4, "أربعون" },
        { 5, "خمسون" },
        { 6, "ستون" },
        { 7, "سبعون" },
        { 8, "ثمانون" },
        { 9, "تسعون" }
    };

    private static readonly Dictionary<int, string> hundreds = new()
    {
        { 1, "مائة" },
        { 2, "مئتان" },
        { 3, "ثلاثمائة" },
        { 4, "أربعمائة" },
        { 5, "خمسمائة" },
        { 6, "ستمائة" },
        { 7, "سبعمائة" },
        { 8, "ثمانمائة" },
        { 9, "تسعمائة" }
    };

    private static readonly Dictionary<int, string> thousands = new()
    {
        { 1, "ألف" },
        { 2, "ألفان" },
        { 3, "ثلاثة آلاف" },
        // ... other thousands
    };

    private static readonly Dictionary<int, string> millions = new()
    {
        { 1, "مليون" },
        { 2, "مليونان" },
        { 3, "ثلاثة ملايين" },
        // ... other millions
    };

    private static readonly Dictionary<int, string> billions = new()
    {
        { 1, "مليار" },
        { 2, "ملياران" },
        { 3, "ثلاثة مليارات" },
        // ... other billions
    };

    internal static string GetText(decimal numb)
    {
        return ConvertToArabicWords((double)numb);
    }

    public static string ConvertToArabicWords(double number)
    {
        if (number == 0) return "صفر";

        string digits = number.ToString().Replace(".", "");
        int length = digits.Length;

        if (length > 15)
            return "الرقم كبير جداً";

        string words = "";

        switch (length)
        {
            case 1:
            case 2:
                words += ConvertGroup(int.Parse(digits));
                break;
            case 3:
                words += ConvertGroup(int.Parse(digits.Substring(1))) + " " + hundreds[int.Parse(digits[0].ToString())];
                break;
            case 4:
            case 5:
                words += ConvertGroup(int.Parse(digits.Substring(length - 3))) + " " + thousands[int.Parse(digits.Substring(length - 4, 1))];
                break;
                // ... other cases for millions, billions, etc.
        }

        // Handle the decimal part
        int decimalPart = (int)((number - (int)number) * 100);
        if (decimalPart > 0)
        {
            words += " و" + ConvertGroup(decimalPart) + " قرش";
        }

        return words;
    }

    private static string ConvertGroup(int group)
    {
        string words = "";

        if (group >= 20)
        {
            words += tens[group / 10] + " ";
            group %= 10;
        }

        if (teens.ContainsKey(group))
        {
            words += teens[group];
        }
        else
        {
            words += units[group];
        }

        return words;
    }
}