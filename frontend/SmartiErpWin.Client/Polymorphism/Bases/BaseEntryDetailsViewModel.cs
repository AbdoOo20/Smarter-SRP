using SmartErpWin.Core;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public abstract partial class BaseDetailsEntryViewModel<T>(HttpClient client) :
    BaseEntryViewModel<T>(client)
    where T : class, new()
{
    [ObservableProperty] 
    private bool _isNew;

    [ObservableProperty]
    private T? _receivedModel;
    
    [ObservableProperty]
    private T? _newModel;

    [ObservableProperty]
    private SymbolRegular? _symbol;

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
            ? SymbolRegular.Add12
            : SymbolRegular.Save16;
        
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
