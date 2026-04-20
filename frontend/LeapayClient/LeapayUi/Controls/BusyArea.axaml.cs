using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SukiUI.Controls;

public partial class BusyArea : UserControl
{
    public BusyArea()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static readonly StyledProperty<bool> IsBusyProperty =
        AvaloniaProperty.Register<BusyArea, bool>(nameof(IsBusy), defaultValue: false);

    public bool IsBusy
    {
        get { return GetValue(IsBusyProperty); }
        set { SetValue(IsBusyProperty, value); }
    }

    public static readonly StyledProperty<string?> BusyTextProperty =
        AvaloniaProperty.Register<BusyArea, string?>(nameof(BusyText), defaultValue: null);

    public string? BusyText
    {
        get => GetValue(BusyTextProperty);
        set => SetValue(BusyTextProperty, value);
    }
    
    public static readonly StyledProperty<string?> SecondaryBusyTextProperty =
        AvaloniaProperty.Register<BusyArea, string?>(nameof(SecondaryBusyText), defaultValue: null);

    public string? SecondaryBusyText
    {
        get => GetValue(SecondaryBusyTextProperty);
        set => SetValue(SecondaryBusyTextProperty, value);
    }
    
    public static readonly StyledProperty<string?> CommandTextProperty =
        AvaloniaProperty.Register<BusyArea, string?>(nameof(CommandText), defaultValue: null);

    public string? CommandText
    {
        get => GetValue(CommandTextProperty);
        set => SetValue(CommandTextProperty, value);
    }
    
    public static readonly StyledProperty<ICommand?> CommandActionProperty =
        AvaloniaProperty.Register<BusyArea, ICommand?>(nameof(CommandAction), defaultValue: null);

    public ICommand? CommandAction
    {
        get => GetValue(CommandActionProperty);
        set => SetValue(CommandActionProperty, value);
    }
}