using System.Globalization;

namespace SmartErpWin.Core.Helpers;

public class CommonTextHelpers
{
    private readonly decimal _amount;

    public CommonTextHelpers(decimal amount)
    {
        _amount = amount;
    }
    
    public string GetText()
    {
        //decimal amount = 1234567890.56m;
        string amountInWords = NumberToTextV6.GetText(_amount);
        return $"{amountInWords}";
    }
    public static string GetText(decimal amount)
    {
        //decimal amount = 1234567890.56m;
        string amountInWords = NumberToTextV6.GetText(amount);
        return $"{amountInWords}";
    }

    public static string GetPrintTime()
    {
        return "وقت الطباعة: " + DateTime.Now.ToString("G", new CultureInfo("en-GB"));
    }
    
    public static Dictionary<string, string> ParseQuery(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(query))
            return result;

        if (query.StartsWith("?"))
            query = query.Substring(1);

        var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(parts[0]);
            var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : "";
            result[key] = value;
        }

        return result;
    }
    
    public static Dictionary<string, string> ParseArgs(string[] args)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (!arg.StartsWith('-'))
                continue;
            
            var key = arg.TrimStart('-');
            var value = (i + 1 < args.Length && !args[i + 1].StartsWith('-')) ? args[++i] : "true";
            dict[key] = value;
        }

        return dict;
    }
}


class NumberToTextV6
{
    internal static string GetText(decimal amount)
    {
        string amountInArabicWords = ConvertToArabicWords(amount);
        return amountInArabicWords;
    }
    
    static string ConvertToArabicWords(decimal amount)
    {
        if (amount < 0)
        {
            return "سالب " + ConvertToArabicWords(Math.Abs(amount));
        }

        // Separate the integer and fractional parts
        long integerPart = (long)Math.Floor(amount);
        long fractionalPart = (long)((amount - integerPart) * 100);

        // Convert integer part
        string integerPartText = NumberToArabicText(integerPart) + " دينار";

        // Convert fractional part if it exists
        string fractionalPartText = fractionalPart > 0
            ? " و" + NumberToArabicText(fractionalPart) + " قرش"
            : "";

        return integerPartText + fractionalPartText;
    }

    static string NumberToArabicText(long number)
    {
        if (number == 0)
        {
            return "صفر";
        }

        string[] arabicOnes = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
        string[] arabicTens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        string[] arabicTeens = { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        string[] arabicHundreds = { "", "مئة", "مئتان", "ثلاثمئة", "أربعمئة", "خمسمئة", "ستمئة", "سبعمئة", "ثمانمئة", "تسعمئة" };
        string[] arabicGroups = { "", "ألف", "مليون", "مليار" };

        string result = "";
        long group = 0;

        while (number > 0)
        {
            long currentGroup = number % 1000;
            if (currentGroup != 0)
            {
                string groupText = ConvertHundredsToArabic(currentGroup);
                if (group > 0)
                {
                    if (currentGroup == 1 && group == 1)
                        groupText = "ألف";
                    else if (currentGroup == 2 && group == 1)
                        groupText = "ألفان";
                    else if (currentGroup > 2 && currentGroup < 11 && group == 1)
                        groupText += " آلاف";
                    else
                        groupText += " " + arabicGroups[group];
                }
                result = groupText + " " + result;
            }
            number /= 1000;
            group++;
        }

        return result.Trim();
    }

    static string ConvertHundredsToArabic(long number)
    {
        string[] arabicOnes = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
        string[] arabicTens = { "", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        string[] arabicTeens = { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        string[] arabicHundreds = { "", "مئة", "مئتان", "ثلاثمئة", "أربعمئة", "خمسمئة", "ستمئة", "سبعمئة", "ثمانمئة", "تسعمئة" };

        long hundreds = number / 100;
        long tens = (number % 100) / 10;
        long ones = number % 10;

        string result = "";

        if (hundreds > 0)
        {
            result += arabicHundreds[hundreds] + " ";
        }

        if (tens == 1)
        {
            result += arabicTeens[ones] + " ";
        }
        else
        {
            if (ones > 0 && tens > 0)
            {
                result += arabicOnes[ones] + " و" + arabicTens[tens] + " ";
            }
            else if (tens > 0)
            {
                result += arabicTens[tens] + " ";
            }
            else if (ones > 0)
            {
                result += arabicOnes[ones] + " ";
            }
        }

        return result.Trim();
    }
}
