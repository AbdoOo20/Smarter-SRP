using SmartErpWin.Core;

namespace Leapay.Client.Polymorphism.Bases;

public abstract partial class BaseDetailsEntryViewModel<T> : BaseEntryViewModel<T>
    where T : class, new()
{
    [ObservableProperty]
    public partial bool IsNew { get; set; }
    
    [ObservableProperty]
    public partial T? ReceivedModel { get; set; }

    [ObservableProperty]
    public partial T? NewModel { get; set; }

    [ObservableProperty]
    public partial MaterialIconKind? Symbol { get; set; }

    protected T? WorkingModel => GetWorkingModel();
    
    /// <summary>
    /// ...
    /// </summary>
    /// <param name="model">Set to null if the component should add instead of update.</param>
    protected async void SetState(T? model)
    {
        var add = model is null;

        // extensive assignment is because of INPC concerns
        // do not modify assignment technique
        
        model ??= new();
        
        ComboModelBase = new();
        Model = null;
        
        Model = await CommonHelpers.DeepCopyValueOfReferenceTypeAsync(model);
        IsNew = add;

        ReceivedModel = null;
        NewModel = null;
        
        if (add)
        {
            NewModel = Model;
            ReceivedModel = null;
        }
        else
        {
            ReceivedModel = Model;
            NewModel = null;
        }

        Symbol = IsNew
            ? MaterialIconKind.Add
            : MaterialIconKind.ContentSave;
        
        Refresh();
    }

    protected internal virtual partial void Refresh()
    {
        // Refresh;
    }

    protected T? GetWorkingModel()
    {
        return IsNew ? NewModel : ReceivedModel;
    }
    
    protected internal virtual partial void Refresh();
}
