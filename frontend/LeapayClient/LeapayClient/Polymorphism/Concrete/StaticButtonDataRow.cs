using Leapay.Client.Polymorphism.Interfaces;

namespace Leapay.Client.Polymorphism.Concrete;

public class StaticButtonDataRow(Action<object>? action, object data, object? content) : IStaticDataRow
{
    public object? Content { get; set; } = content;

    public Action<object>? Action { get; set; } = action;
    
    public object Data { get; set; } = data;
}