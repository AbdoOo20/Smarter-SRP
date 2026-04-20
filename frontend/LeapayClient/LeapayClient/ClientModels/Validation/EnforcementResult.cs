using System.Text;
using Leapay.Client.Services;

namespace Leapay.Client.ClientModels.Validation;

public readonly struct EnforcementResult(List<ValidationResult> validationResults)
{
    public List<ValidationResult> ValidationResults { get; } = validationResults;

    public string GetDisplayMessage()
    {
        var localeService = App.GetService<LocalisationService>();
        var strBuilder = new StringBuilder();
        
        foreach (var failed in ValidationResults.Where(r => !r.EnforcementSuccess))
        {
            strBuilder.AppendLine($"• {localeService[failed.Result]}");
        }

        return strBuilder.ToString();
    }

    public bool IsSuccess() => ValidationResults.All(r => r.EnforcementSuccess);
}