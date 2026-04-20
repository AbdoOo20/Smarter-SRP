namespace Leapay.Client.Options;

public record struct WindowOptions(string Title,
    bool CanMaximize = false,
    bool CanResize = false,
    bool CanMinimize = true,
    bool IsMaximized = false,
    bool IsDialogue = false,
    bool BlockUntilContentCloses = false);