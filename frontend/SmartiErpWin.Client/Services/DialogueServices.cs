using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using MessageBoxResult = Wpf.Ui.Controls.MessageBoxResult;

namespace SmartiErpWin.Client.Services;

public static class DialogueServices
{
    private static LocalisationService? LocaleService => App.GetService<LocalisationService>();
    
    private static ISnackbarService? SnackbarService => App.GetService<ISnackbarService>();

    public static async Task ShowMessageAsync(
        string? title,
        string? message,
        string? button = "", 
        bool shouldCloseApp = false,
        LocalisationService? localeService = null)
    {
        localeService ??= App.GetService<LocalisationService>();
        
        if (string.IsNullOrEmpty(button))
            button = localeService?["Ok"] ?? "Ok";

        var flowDirection = localeService?.DetermineFlowDirection();
        
        await new MessageBox
        {
            Title = localeService?[title] ?? title,
            Content = localeService?[message] ?? message,
            MinWidth = MinimumDialogueBoxWidth,
            CloseButtonText = button,
            FlowDirection =  flowDirection ?? FlowDirection.LeftToRight
        }.ShowDialogAsync();

        if (shouldCloseApp)
            Application.Current.Shutdown();
    }

    public static async Task<bool> ConfirmAsync(
        LocalisationService locale,
        string title,
        string message)
    {
        var dialogue = await new MessageBox
        {
            Title = locale[title],
            Content = message,
            SecondaryButtonText = locale["Yes"],
            CloseButtonText = locale["No"],
            CloseButtonAppearance = ControlAppearance.Secondary,
            SecondaryButtonAppearance = ControlAppearance.Primary,
            FlowDirection =  locale.DetermineFlowDirection(),
            MinWidth = MinimumDialogueBoxWidth,
        }.ShowDialogAsync();

        return dialogue == MessageBoxResult.Secondary;
    }
    
    public static bool Stop(
        this bool function,
        string title,
        string message,
        LocalisationService locale)
    {
        if (!function)
            return false;
        
        _ = ShowMessageAsync(title,
            message,
            localeService: locale);
        
        return true;
    }

    public static async Task<bool> ConfirmDeleteAsync(LocalisationService locale)
    {
        var dialogue = await new MessageBox
        {
            Title = locale["DeleteItemTitle"],
            Content = locale["DeleteItemMessage"],
            SecondaryButtonText = locale["Yes"],
            CloseButtonText = locale["No"],
            CloseButtonAppearance = ControlAppearance.Primary,
            FlowDirection =  locale.DetermineFlowDirection(),
            MinWidth = MinimumDialogueBoxWidth,
        }.ShowDialogAsync();

        return dialogue == MessageBoxResult.Secondary;
    }

    public static void ShowSuccessSnack(string title, string message, TimeSpan? timeout = null)
    {
        timeout ??= TimeSpan.FromSeconds(3);
        
        SnackbarService?.Show(LocaleService[title],
            LocaleService[message],
            ControlAppearance.Success,
            new SymbolIcon(SymbolRegular.Checkmark12),
            timeout.GetValueOrDefault());
    }

    public static void ShowContentDialogue(string title, string message, string close = "Close")
    {
        var contentDialogue = App.GetService<IContentDialogService>();

        contentDialogue?.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        {
            Title = LocaleService[title],
            Content = LocaleService[message],
            CloseButtonText = LocaleService[close]
        });
    }
}