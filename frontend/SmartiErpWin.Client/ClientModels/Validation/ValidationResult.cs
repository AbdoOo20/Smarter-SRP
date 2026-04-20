namespace SmartiErpWin.Client.ClientModels.Validation;

public readonly record struct ValidationResult(string Result,
    bool EnforcementSuccess);