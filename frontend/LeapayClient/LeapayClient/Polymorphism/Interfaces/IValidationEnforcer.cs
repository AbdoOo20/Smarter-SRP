using Leapay.Client.ClientModels.Validation;

namespace Leapay.Client.Polymorphism.Interfaces;

public interface IValidationEnforcer
{
    List<ValidationModel> Validations { get; set; }

    public EnforcementResult Enforce(object obj)
    {
        return new EnforcementResult(Validations
            .Select(validation => validation
            .Enforce(obj))
            .ToList());
    }

    public void AddValidation(ValidationModel validation)
    {
        Validations.Add(validation);
    }

    public void ClearAll()
    {
        Validations.Clear();
    }
}

public class ValidationEnforceImpl : IValidationEnforcer
{
    public List<ValidationModel> Validations { get; set; } = [];
}