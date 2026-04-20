using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Global.Requests;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Mapster;
using SmartErpWin.Core;

namespace Leapay.Client.UserInterface.Misc.Configuration;

public partial class CompanyInfoPageViewModel : BaseEntryViewModel<DtoConfig>
{
    public CompanyInfoPageViewModel()
    {
        Refresh();
    }

    private async void Refresh()
    {
        Model = (await GetInnerOneAsync("account-settings")).Data;
        CompanyInfo = (await GetSpecificInnerOneAsync<DtoCompanyInfo>("company-settings")).Data;
        Image = string.IsNullOrEmpty(CompanyInfo?.Image)
            ? Config.Get?.ClientConfig.ImagePath
              ?? ImagePlaceholder : CompanyInfo.Image;
    }

    protected override string Endpoint => "Config";

    private readonly IServiceProvider _prov;

    [ObservableProperty]
    public partial DtoCompanyInfo? CompanyInfo { get; set; }

    [ObservableProperty]
    public partial string? Image { get; set; }
    
    [RelayCommand]
    public async Task BrowseImage()
    {
        var data = await DataHelper.PickFileAndReadBytesAsync();
        CompanyInfo.ImageData = data;
    }
    
    protected override async Task SaveAsync()
    {
        if (Model is null || !PreSaveChecks())
            return;

        PreSaveActions();

        if (!PreSaveValidations())
            return;

        //await PostSaveSpecificAsync(CompanyInfo, "Company");
        //await PostSaveAsync(Model);

        SettingsRequest settingsRequest = new()
        {
            CompanyInfo = CompanyInfo.Adapt<CompanyInfoRequest>(),
            Config = Model.Adapt<ConfigRequest>(),
        };
        
        await PostSaveSpecificAsync(settingsRequest);
        await RefreshCompanyDataAsync();
    }

    public async Task RefreshCompanyDataAsync()
    {
        CompanyInfo = (await GetSpecificInnerOneAsync<DtoCompanyInfo>("company-settings")).Data;
        PreferencesService.Set(s => s.CompanyInfo = CompanyInfo);
    }
}