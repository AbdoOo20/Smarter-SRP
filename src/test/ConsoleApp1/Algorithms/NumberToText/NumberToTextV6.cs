namespace ConsoleApp1.Algorithms.NumberToText;


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

