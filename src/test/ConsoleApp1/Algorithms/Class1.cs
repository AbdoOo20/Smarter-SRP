namespace ConsoleApp1.Algorithms;

public class NumberToTextV1
{

    private decimal _amount;

    public NumberToTextV1(decimal amount)
    {
        _amount = amount;
    }


    public string GetText()
    {
        //decimal amount = 1234567890.56m;
        string amountInWords = ConvertToArabicText(_amount);
        return $"{amountInWords}";
    }

    public static string GetText(decimal amount)
    {
        return new NumberToTextV1(amount).GetText();
    }

    string ConvertToArabicText(decimal number)
    {
        string[] units = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };
        string[] teens = { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        string[] tens = { "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        string[] hundreds = { "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };
        string[] thousands = { "", "ألف", "ألفان", "ثلاثة آلاف", "أربعة آلاف", "خمسة آلاف", "ستة آلاف", "سبعة آلاف", "ثمانية آلاف", "تسعة آلاف" };
        string[] millions = { "", "مليون", "مليونان", "ثلاثة ملايين", "أربعة ملايين", "خمسة ملايين", "ستة ملايين", "سبعة ملايين", "ثمانية ملايين", "تسعة ملايين" };
        string[] hundredMillions = { "", "مائة مليون", "مائتا مليون", "ثلاثمائة مليون", "أربعمائة مليون", "خمسمائة مليون", "ستمائة مليون", "سبعمائة مليون", "ثمانمائة مليون", "تسعمائة مليون" };

        int wholeNumber = (int)Math.Floor(number);
        int fractionalPart = (int)((number - wholeNumber) * 100);

        string result = ConvertWholeNumberToArabic(wholeNumber, units, teens, tens, hundreds, thousands, millions, hundredMillions);

        if (fractionalPart > 0)
        {
            result += " و " + ConvertWholeNumberToArabic(fractionalPart, units, teens, tens, hundreds, thousands, millions, hundredMillions) + " من مئة";
        }

        return result;
    }

    string ConvertWholeNumberToArabic(int number, string[] units, string[] teens, string[] tens, string[] hundreds, string[] thousands, string[] millions, string[] hundredMillions)
    {
        if (number == 0) return "صفر";

        string words = "فقط "; // Add "فقط" at the beginning

        if (number >= 100000000)
        {
            words += millions[number / 1000000] + " ";
            number %= 1000000;
        }

        if (number >= 1000000)
        {
            words += millions[number / 1000000] + " ";
            number %= 1000000;
        }

        if (number >= 1000)
        {
            words += thousands[number / 1000] + " ";
            number %= 1000;
        }

        if (number >= 100)
        {
            words += hundreds[number / 100] + " ";
            number %= 100;
        }

        if (number >= 20)
        {
            words += tens[number / 10] + " ";
            number %= 10;
        }
        else if (number >= 10)
        {
            words += teens[number - 10] + " ";
            return words.Trim() + " لا غير"; // Add "لا غير" at the end
        }

        if (number > 0)
        {
            words += units[number] + " ";
        }

        return words.Trim() + " لا غير"; // Add "لا غير" at the end
    }

    static string ConvertAmountToWords(decimal amount)
    {
        if (amount == 0)
            return "صفر";

        int integerPart = (int)amount;
        int fractionalPart = (int)((amount - integerPart) * 100);

        string words = NumberToWords(integerPart) + " دينار";

        if (fractionalPart > 0)
        {
            words += " و " + NumberToWords(fractionalPart) + " قرش";
        }

        return words;
    }

    static string NumberToWords(int number)
    {
        if (number == 0)
            return "صفر";

        if (number < 0)
            return "سالب " + NumberToWords(Math.Abs(number));

        string words = "";

        if (number / 1000000000 > 0)
        {
            words += NumberToWords(number / 1000000000) + " مليار ";
            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            words += NumberToWords(number / 1000000) + " مليون ";
            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            words += NumberToWords(number / 1000) + " ألف ";
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            words += NumberToWords(number / 100) + " مئة ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "و ";

            var unitsMap = new[] { "صفر", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
            var tensMap = new[] { "صفر", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if (number % 10 > 0)
                    words += " و " + unitsMap[number % 10];
            }
        }

        return words;
    }

}
