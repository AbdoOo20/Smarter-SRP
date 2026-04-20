using Leapay.Client.Helpers;
using SukiUI.Controls;
using SukiUI.Dialogs;

namespace Leapay.Client.Services;

public static class DialogueServices
{
    private static LocalisationService Locale => App.GetService<LocalisationService>();
    private static ISukiDialogManager Manager => GetManager();

    private static ISukiDialogManager DefaultManager => App.GetService<ISukiDialogManager>();

    private static ISukiDialogManager GetManager()
    {
        if (ContentHelper.OpenWindows.LastOrDefault() is { Hosts: { } hosts } &&
            hosts.OfType<SukiDialogHost>().FirstOrDefault() is { } host)
        {
            var manager = new SukiDialogManager();
            host.Manager = manager;

            return manager;
        }

        return DefaultManager;
    }
    
    public static async Task ShowMessageAsync(
        string? title,
        string? message,
        string? button = "", 
        bool shouldCloseApp = false)
    {
        if (string.IsNullOrEmpty(button))
            button = Locale["Ok"];

        var flowDirection = Locale.DetermineFlowDirection();

        var box = Manager.CreateDialog()
            .WithTitle(Locale[title])
            .WithContent(Locale[message]);
        
        var completion = new TaskCompletionSource<bool>();
        box.Completion = completion;
        
        await box.WithActionButton(button, _ => box.Dismiss(), true)
            .TryShowAsync();

        if (shouldCloseApp)
            App.Shutdown();
    }

    public static async Task ShowErrorAsync(
        string? message,
        string? button = "",
        string? title = "Error",
        bool shouldCloseApp = false)
    {
        await ShowMessageAsync(title, message, button, shouldCloseApp);
    }
    
    public static async Task<bool> ConfirmAsync(
        string title,
        string message)
    {
        var confirmed = false;
        
        var box = Manager.CreateDialog()
            .WithTitle(Locale[title])
            .WithContent(Locale[message])
            .WithActionButton(Locale["Yes"], _ => confirmed = true, true)
            .WithActionButton(Locale["No"], _ => { }, true);
        var completion = new TaskCompletionSource<bool>();
        box.Completion = completion;
        
        await box.TryShowAsync();

        return confirmed;
    }

    public static Task ShowContactSupportMessageAsync()
    {
        return ShowMessageAsync("Error", "PleaseContactSupport");
    }
    
    public static bool Stop(
        this bool function,
        string title,
        string message)
    {
        if (!function)
            return false;
        
        _ = ShowMessageAsync(Locale[title], Locale[message]);
        
        return true;
    }

    public static async Task<bool> ConfirmDeleteAsync()
    {
        return await ConfirmAsync("DeleteItemTitle", "DeleteItemMessage");
    }

    // Different approaches will be used
    
    /*
    public static void ShowSuccessSnack(string title, string message, TimeSpan? timeout = null)
    {
        
    }
    */

    /*public static void ShowContentDialogue(string title, string message, string close = "Close")
    {
        var contentDialogue = App.GetService<IContentDialogService>();

        contentDialogue?.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        {
            Title = LocaleService[title],
            Content = LocaleService[message],
            CloseButtonText = LocaleService[close]
        });
    }*/
}