using Fly.Framework.Shared.Common;

namespace SmartiErpWin.Client.ClientModels.Validation;

public readonly record struct ValidationRule(object Condition, string FailureMessage)
{
    public bool EnforceRule(object val) =>
        Condition switch
        {
            Predicate<object> preObj => preObj.Invoke(val),
            Predicate<double> preDou => preDou.Invoke(Convert.ToDouble(val)),
            Predicate<string> preStr => preStr.Invoke(Convert.ToString(val)),
            Predicate<decimal> preDec => preDec.Invoke(Convert.ToDecimal(val)),
            Predicate<Identifier> preId => preId.Invoke((Identifier)val),
            Predicate<List<Identifier>> preListId => preListId.Invoke((List<Identifier>)val),
            Predicate<List<object>> preListObj => preListObj.Invoke((List<object>)val),
            null => true,
            _ => true
        };
}