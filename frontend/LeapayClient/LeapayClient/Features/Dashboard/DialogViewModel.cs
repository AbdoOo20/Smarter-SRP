using SukiUI.Dialogs;

namespace Leapay.Client.Features.Dashboard;

public partial class DialogViewModel(ISukiDialog dialog) : ObservableObject
{
    [RelayCommand]
    private void CloseDialog()
    {
        dialog.Dismiss();
    }
}