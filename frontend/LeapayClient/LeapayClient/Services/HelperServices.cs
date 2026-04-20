using Avalonia.Input;
using Fly.Framework.Shared.Common.Interfaces;
using Leapay.Client.Helpers;

namespace Leapay.Client.Services;

public static class HelperServices
{
    public static IEnumerable<IDetailsModel> RemoveItemFromModelList(IEnumerable<IDetailsModel> list, IDetailsModel model)
    {
        return list.Where(l => l.TemporaryId != model.TemporaryId);
    }
    
    public static void HandlePreviewKey<TWindow>(KeyEventArgs args) where TWindow: UserControl
    {
        if (args.Key != Key.Enter)
            return;

        Verbose("Received tab key index, handling enter key replacement...");

        args.Handled = true;
        
        var manager = ContentHelper.GetGlobal<TWindow, IFocusManager>(x => x.FocusManager);
        var current = manager.GetFocusedElement();

        if (current is null)
        {
            Warning("Called focus on a window that has no actively focus element.");
            return;
        }
        
        // Currently there is no alternative to this deprecated method until Avalonia 12 releases.
        var next = KeyboardNavigationHandler.GetNext(current, NavigationDirection.Next);

        if (next is null)
        {
            Debug("Could not determine the next control to focus.\n" +
                  "This is probably the last input control in the visual tree.");
            return;
        }
        
        next.Focus();
    }
}