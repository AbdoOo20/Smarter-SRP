using Leapay.Client.Polymorphism.Interfaces;

namespace Leapay.Client.Polymorphism.Concrete;

public class StaticStringDataRow(object? content) : IStaticDataRow
{
    public object? Content { get; set; } = content;
}