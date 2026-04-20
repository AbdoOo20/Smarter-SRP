using SmartiErpWin.Client.ClientModels.Validation;

namespace SmartiErpWin.Client.Polymorphism.Interfaces;

public interface IValidationEnforcer
{
    private static List<ValidationModel> Validations { get; } = [];

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