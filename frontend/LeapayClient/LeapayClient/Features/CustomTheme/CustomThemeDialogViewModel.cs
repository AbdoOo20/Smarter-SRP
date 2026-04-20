using Avalonia.Media;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SukiUI;
using SukiUI.Dialogs;
using SukiUI.Models;

namespace Leapay.Client.Features.CustomTheme;

public partial class CustomThemeDialogViewModel(SukiTheme theme, ISukiDialog dialog) : BasePageViewModel<DtoBase>
{
    [ObservableProperty] private string _displayName = "Pink";
    [ObservableProperty] private Color _primaryColor = Colors.DeepPink;
    [ObservableProperty] private Color _accentColor = Colors.Pink;

    [RelayCommand]
    private void TryCreateTheme()
    {
        if (string.IsNullOrEmpty(DisplayName)) return;
        var theme1 = new SukiColorTheme(DisplayName, PrimaryColor, AccentColor);
        Theme.AddColorTheme(theme1);
        Theme.ChangeColorTheme(theme1);
        dialog.Dismiss();
    }
}