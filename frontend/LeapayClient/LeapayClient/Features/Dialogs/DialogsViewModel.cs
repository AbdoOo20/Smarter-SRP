using Avalonia.Controls.Notifications;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace Leapay.Client.Features.Dialogs;

public partial class DialogsViewModel : BasePageViewModel<DtoBase>
{
    private readonly ISukiDialogManager _dialogManager;
    private readonly ISukiToastManager _toastManager;

    public DialogsViewModel(ISukiDialogManager dialogManager, ISukiToastManager toastManager)
    {
        _dialogManager = dialogManager;
        _toastManager = toastManager;
        
        SetProperties("Dialogs", MaterialIconKind.Abacus);
    }
    
    public NotificationType[] NotificationTypes { get; } = Enum.GetValues<NotificationType>();
    
    [ObservableProperty] private NotificationType _selectedType;
    
    [RelayCommand]
    private void OpenStandardDialog()
    {
        _dialogManager.CreateDialog()
            .WithTitle("A Standard Dialog")
            .WithContent("This is a standard dialog. Click the button below to dismiss.")
            .WithActionButton("Dismiss", _ => { }, true)
            .TryShow();
    }

    [RelayCommand]
    private void OpenBackgroundCloseDialog()
    {
        _dialogManager.CreateDialog()
            .WithTitle("Background Closing Dialog")
            .WithContent("Dismiss this dialog by clicking anywhere outside of it.")
            .Dismiss().ByClickingBackground()
            .TryShow();
    }

    [RelayCommand]
    private void OpenMultiOptionDialog()
    {
        _dialogManager.CreateDialog()
            .WithTitle("Multi Option Dialog")
            .WithContent("Select any one of the below options:")
            .WithActionButton("Option 1", _ => ShowOptionToast(1), true)
            .WithActionButton("Option 2", _ => ShowOptionToast(2), true)
            .WithActionButton("Option 3", _ => ShowOptionToast(3), true)
            .TryShow();
    }

    private void ShowOptionToast(int option)
    {
        _toastManager.CreateToast()
            .WithTitle("Dialog Option Clicked")
            .WithContent($"You clicked option #{option}")
            .Dismiss().ByClicking()
            .Dismiss().After(TimeSpan.FromSeconds(3))
            .Queue();
    }

    [RelayCommand]
    private void OpenMessageBoxStyleDialog()
    {
        _dialogManager.CreateDialog()
            .OfType(SelectedType)
            .WithTitle("MessageBox style dialog.")
            .WithContent("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.")
            .WithActionButton("Close " + SelectedType.ToString(), _ => { }, true, "Flat", "Accent")
            .Dismiss().ByClickingBackground()
            .TryShow();
    }

    [RelayCommand]
    private void OpenViewModelDialog()
    {
        _dialogManager.CreateDialog()
            .WithViewModel(dialog => new VmDialogViewModel(dialog))
            .TryShow();
    }

    [RelayCommand]
    private void OpenDialogWindowDemo() => new DialogWindowDemo(_dialogManager).Show();
        
}