namespace ConsoleApp1.Algorithms.NumberToText;

using System;

public class NumberToTextV2
{
    private static readonly string[] units = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
    private static readonly string[] teens = { "", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
    private static readonly string[] tens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
    private static readonly string[] hundreds = { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
    private static readonly string[] thousands = { "", "ألف", "ألفا", "آلاف" };
    private static readonly string[] millions = { "", "مليون", "مليونا", "ملايين" };

    public static string ConvertToArabicWords(decimal number)
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
                words += ConvertGroup(int.Parse(digits)) + " " + hundreds[int.Parse(digits[0].ToString())];
                break;
            case 4:
            case 5:
                words += ConvertGroup(int.Parse(digits.Substring(length - 3))) + " " + thousands[GetPluralForm(int.Parse(digits.Substring(length - 4, 1)))];
                break;
            case 6:
            case 7:
                words += ConvertGroup(int.Parse(digits.Substring(length - 3))) + " " + hundreds[int.Parse(digits.Substring(length - 6, 1))] + " ألف ";
                break;
            case 8:
            case 9:
                words += ConvertGroup(int.Parse(digits.Substring(length - 3))) + " " + thousands[GetPluralForm(int.Parse(digits.Substring(length - 4, 1)))] + " مليون ";
                break;
            case 10:
            case 11:
            case 12:
                words += ConvertGroup(int.Parse(digits.Substring(length - 3))) + " " + hundreds[int.Parse(digits.Substring(length - 6, 1))] + " مليون ";
                break;
            case 13:
            case 14:
            case 15:
                words += ConvertGroup(int.Parse(digits.Substring(length - 3))) + " " + thousands[GetPluralForm(int.Parse(digits.Substring(length - 4, 1)))] + " مليار ";
                break;
            default:
                words = "الرقم كبير جداً";
                break;
        }

        // Handle the decimal part
        int decimalPart = (int)((number - (int)number) * 100);
        if (decimalPart > 0)
        {
            words += " و" + ConvertGroup(decimalPart) + " قرش";
        }

        return words;
    }

    public static string ConvertToArabicWordsx(decimal number)
    {
        if (number == 0) return "صفر";

        string digits = number.ToString().Replace(".", "");
        int length = digits.Length;

        if (length > 15)
            return "الرقم كبير جداً";

        string words = "";

        int i = 0;
        while (i < length)
        {
            int group = int.Parse(digits.Substring(i, Math.Min(3, length - i)));

            if (group > 0)
            {
                if (i == length - 3)
                {
                    // Millions group
                    words += ConvertGroup(group) + " " + millions[GetPluralForm(group)];
                }
                else if (i == length - 6)
                {
                    // Thousands group
                    words += ConvertGroup(group) + " " + thousands[GetPluralForm(group)];
                }
                else
                {
                    words += ConvertGroup(group);
                }

                words += " و";
            }

            i += 3;
        }

        // Remove the trailing "و"
        words = words.TrimEnd('و');

        // Handle the decimal part
        int decimalPart = (int)((number - (int)number) * 100);
        if (decimalPart > 0)
        {
            words += " و" + ConvertGroup(decimalPart) + " قرش";
        }

        return words;
    }

    internal static object GetText(decimal v)
    {
        return ConvertToArabicWords(v);
    }

    private static string ConvertGroup(int group)
    {
        string words = "";

        if (group >= 100)
        {
            words += hundreds[group / 100] + " ";
            group %= 100;
        }

        if (group >= 20)
        {
            words += tens[group / 10] + " ";
            group %= 10;
        }

        if (group >= 11 && group <= 19)
        {
            words += teens[group - 10];
        }
        else if (group >= 1 && group <= 9)
        {
            words += units[group];
        }

        return words;
    }

    private static int GetPluralForm(int number)
    {
        if (number == 1) return 0;
        if (number == 2) return 1;
        return 2;
    }
}