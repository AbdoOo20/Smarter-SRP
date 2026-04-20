using Avalonia.Markup.Xaml;

namespace Leapay.Client.Features.Theming;

public partial class ThemingView : UserControl
{
    public ThemingViewModel ViewModel { get; set; }
    
    public ThemingView(ThemingViewModel viewModel)
    {
        ViewModel = viewModel;
        
        InitializeComponent();

        DataContext = this;

        Loaded += (_, _) =>
        {
            // All these will be null
            /*AccountHeader.Header = ViewModel["Settings"];
            ThemeHeader.Header = ViewModel["BaseTheme"];
            ColourThemeHeader.Header = ViewModel["ColourTheme"];
            BackgroundHeader.Header = ViewModel["Background"];*/
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}