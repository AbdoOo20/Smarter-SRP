namespace Leapay.Client.UI.ViewModels;

public interface IViewModel
{
    public bool IsResizing { get; set; }
    
    public Window? WindowLock { get; set; }
}