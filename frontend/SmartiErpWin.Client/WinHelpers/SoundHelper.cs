using System.Media;
using SmartErpWin.Core.Attributes;
using SmartErpWin.Core.Helpers;

namespace SmartiErpWin.Client.WinHelpers;

public static class SoundHelper
{
    public static void Play(Sound sound)
    {
        var fileName = ReflectionHelper.GetAttributeOfType<FileName>(sound)?.File;

        if (fileName is null)
            return;

        fileName = $"./res/{fileName}.wav";
        
        var player = new SoundPlayer(fileName);
        
        player.Play();
    }
}