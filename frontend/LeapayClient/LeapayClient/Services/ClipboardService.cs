using Avalonia.Controls.ApplicationLifetimes;

namespace Leapay.Client.Services;

public class ClipboardService(IClassicDesktopStyleApplicationLifetime lifetime)
{
    public void CopyToClipboard(string text) => lifetime.MainWindow?.Clipboard?.SetTextAsync(text);
}