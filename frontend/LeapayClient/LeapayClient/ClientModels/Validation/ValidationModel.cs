using Fly.Framework.Shared.Common;

namespace Leapay.Client.ClientModels.Validation;

public record ValidationModel(string PropertyName, List<ValidationRule>? Rules, bool Optional = false)
{
    public ValidationResult Enforce(object dto)
    {
        var propertyInfo = dto.GetType()
            .GetProperties()
            .FirstOrDefault(p => p.Name == PropertyName);

        if (propertyInfo is null)
            return new ValidationResult("NoPropertyFound", false);

        var val = propertyInfo.GetValue(dto);
        
        if (!Optional)
        {
            var prefix = $"{propertyInfo.Name}";
            switch (val)
            {
                case string str:
                    if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                        return new($"{prefix}CannotBeEmpty", false);
                    break;
                case int or double or decimal:
                    if (Convert.ToInt32(val) <= 0 || Convert.ToDouble(val) <= 0 || Convert.ToDecimal(val) <= 0)
                        return new($"{prefix}NeedsValue", false);
                    break;
                case Identifier id:
                {
                    if (id == Identifier.Empty || id == default)
                    {
                        return new($"{prefix}SelectOption", false);
                    }

                    break;
                }
            }
        }

        if (Rules != null && val is not null)
        {
            foreach (var rule in Rules.Where(rule => !rule.EnforceRule(val)))
            {
                return new($"{rule.FailureMessage}", false);
            }    
        }

        return new("Success", true);
    }
}