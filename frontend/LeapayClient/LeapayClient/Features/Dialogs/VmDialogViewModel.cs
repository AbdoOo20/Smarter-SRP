using SukiUI.Dialogs;

namespace Leapay.Client.Features.Dialogs;

public partial class VmDialogViewModel(ISukiDialog dialog) : ObservableObject
{
    [RelayCommand]
    private void CloseDialog() => dialog.Dismiss();
}