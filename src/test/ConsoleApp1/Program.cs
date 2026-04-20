// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using ConsoleApp1.Algorithms.NumberToText;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        //BasicLicenseValidation.Starter();
        //RDLCDesigner.Start(args);
        ShowNumber();
    }

    static void ShowNumber()
    {

        var text = NumberToTextV6.GetText(1900545985322.50m);

        Console.Read();
    }

}
