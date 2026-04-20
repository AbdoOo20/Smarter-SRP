using System.Windows.Controls;
using System.Windows.Input;
using SmartiErpWin.Client.DtoModels;

namespace SmartiErpWin.Client.Services;

public static class HelperServices
{
    public static IEnumerable<IModel> RemoveItemFromModelList(IEnumerable<IModel> list, IModel model)
    {
        return list.Where(l => l.TemporaryId != model.TemporaryId);
    }

    public static void HandlePreviewKey(KeyEventArgs args)
    {
        if (args.Key != Key.Enter)
            return;
        
        Verbose("Received tab key index, handling enter key replacement...");

        args.Handled = true;

        if (Keyboard.FocusedElement is Button btn)
        {
            btn.Command?.Execute(null);
            return;
        }
        
        MoveFocusToNextControl();
    }

    private static void MoveFocusToNextControl()
    {
        var request = new TraversalRequest(FocusNavigationDirection.Next);

        if (Keyboard.FocusedElement is UIElement elementWithFocus)
        {
            elementWithFocus.MoveFocus(request);
        }
    }
}