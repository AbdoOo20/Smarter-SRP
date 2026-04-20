using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;
using SmartErpWin.Core.Interfaces;

namespace SmartiErpWin.Client.Wrappers.INPC;

public partial class ObservableSelectableRole: ObservableObject,
                                               ISelectableRole<ObservableRoleKey>,
                                               ISearchable
{
    public Identifier Id { get; set; }

    [ObservableProperty]
    private string? _windowName;

    [ObservableProperty]
    private ObservableCollection<ObservableRoleKey> _keys = [];

    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private string? _isEnabledText;

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