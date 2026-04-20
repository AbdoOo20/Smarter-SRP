using SmartErpWin.Core;

namespace Leapay.Client.Polymorphism.Bases;

public partial class ModelDetailsViewModelBase<T>: ObservableObject
    where T : class, new()
{
    [ObservableProperty]
    public partial bool IsNew { get; set; }

    [ObservableProperty]
    public partial T? Model { get; set; }
    
    [ObservableProperty]
    public partial MaterialIconKind? Symbol { get; set; }
    
    /// <summary>
    /// ...
    /// </summary>
    /// <param name="model">Set to null if the component should add instead of update.</param>
    public async void SetState(T? model)
    {
        var add = model is null;

        // extensive assignment is because of INPC concerns
        // do not modify assignment technique
        
        model ??= new();
        
        Model = null;
        
        Model = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model);
        IsNew = add;

        Symbol = IsNew
            ? MaterialIconKind.Add
            : MaterialIconKind.ContentSave;
    }
}
