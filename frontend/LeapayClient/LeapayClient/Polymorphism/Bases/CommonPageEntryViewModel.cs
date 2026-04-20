using SmartErpWin.Core.Enums;

namespace Leapay.Client.Polymorphism.Bases;

public class CommonPageEntryViewModel<T>: BaseEntryViewModel<T> where T: class, new()
{
    protected override string? Endpoint { get; set; }
    
    private Action? _windowClosure = null;
    private Action? _afterSave = null;
    
    public CommonPageEntryViewModel(
        T? model,
        string endpoint,
        EntryType entryType,
        Type pageType,
        Action? windowClosure = null,
        Action? afterSave = null)
    {
        SetDetails(model, entryType, pageType);
        
        Endpoint = endpoint;

        _windowClosure = windowClosure;
        _afterSave = afterSave;
        
        ApplicationTitle = GetPageName(pageType, EntryType);
    }

    protected override void WindowClosure()
    {
        if(_windowClosure is not null)
            _windowClosure.Invoke();
        else
        {
            base.WindowClosure();
        }
    }

    protected override void AfterSave()
    {
        _afterSave?.Invoke();
        base.AfterSave();
    }
}