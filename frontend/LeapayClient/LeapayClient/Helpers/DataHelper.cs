using Leapay.Client.Services;
using SmartErpWin.Core;

namespace Leapay.Client.Helpers;

public static class DataHelper
{
    public static async Task<byte[]?> PickFileAndReadBytesAsync(Window? parent = null)
    {
        parent ??= App.GetMainWindow();
        
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Title = App.GetService<LocalisationService>()["BrowseFile"],
            Filters =
            {
                new FileDialogFilter { Name = "Image Files", Extensions = { "png", "jpg", "jpeg", "bmp", "gif" } },
            }
        };

        var result = await dialog.ShowAsync(parent);
        if (result is { Length: > 0 } && File.Exists(result[0]))
        {
            return await File.ReadAllBytesAsync(result[0]);
        }

        return GetDefaultProductImage();
    }

    public static byte[] GetDefaultProductImage()
    {
        try
        {
            var data = File.ReadAllText("Assets/def.data");

            if (string.IsNullOrEmpty(data))
                return [];
            
            return Convert.FromBase64String(data);
        }
        catch
        {
            return [];
        }
    }

    public static List<ClientInfo> GetCompanyInfo()
    {
        var dto = PreferencesService.Get(s => s.CompanyInfo);

        return
        [
            new()
            {
                Address = dto.Address,
                Decription = dto.Type,
                Email = dto.Email,
                NameAr = dto.Name,
                NameEn = dto.NameEn,
                Phone1 = dto.PhoneNumber1,
                Phone2 = dto.PhoneNumber2,
                Phone3 = dto.PhoneNumber3,
                Image = dto.ImageData
            }
        ];
    }
}