using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;
using Leapay.Client.Services;
using SmartErpWin.Core.Interfaces;

namespace Leapay.Client.Wrappers.INPC;

public partial class ObservableSelectableRole: ObservableObject,
                                               ISelectableRole<ObservableRoleKey>,
                                               ISearchable
{
    public Identifier Id { get; set; }

    [ObservableProperty]
    public partial string? WindowName { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<ObservableRoleKey> Keys { get; set; } = [];
    [ObservableProperty]
    public partial bool IsEnabled { get; set; }

    [ObservableProperty]
    public partial string? IsEnabledText { get; set; }

    public void Refresh()
    {
        IsEnabled = Keys.Any(k => k.IsChecked);
        IsEnabledText = Keys.All(k => k.IsChecked)
            ? "☑️"
            : IsEnabled
                ? "⏹️"
                : string.Empty;
    }

    public string Display
    {
        get => App.GetService<LocalisationService>()?[WindowName]
               ?? WindowName
               ?? string.Empty;
        set { }
    }
}