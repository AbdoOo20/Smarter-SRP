using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Configuration;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Globals;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.UserInterface.Common.SysConfigPage;

public partial class SysConfigPageViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    [ObservableProperty]
    public partial SysConfigModel? SysConfigModel { get; set; } = new();
    
    [ObservableProperty]
    public partial string? BackendUrl { get; set; }
    
    public SysConfigPageViewModel(
        DtoBase? model,
        EntryType entryType)
    {
        SetDetails(model, entryType, GetType());
        
        Prepare();
    }
    
    private void Prepare()
    {
        SysConfigModel = PreferencesService.Get(s => s.SysConfigModel);
        BackendUrl = Config.Get.BackendUrl;
    }
    
    [RelayCommand]
    private void OnSaveSettings()
    {
        if(!string.IsNullOrEmpty(SysConfigModel.DatabasePasswordU))
            SysConfigModel.DatabasePassword = ApplicationHost.Start(SysConfigModel.DatabasePasswordU);
        
        if(!string.IsNullOrEmpty(SysConfigModel.BackendPath))
        {
            if (!SysConfigModel.VerifyBackendPath(out var settingsPath))
            {
                _ = DialogueServices.ShowMessageAsync("Error",
                    "Backend path is incorrect. No appsettings.json file found.");
                return;
            }
            
            if (!SysConfigModel.VerifyDatabaseAuth())
            {
                _ = DialogueServices.ShowMessageAsync("Error",
                    "Database auth is missing. Cannot update appsettings.json file.");
                return;
            }
            
            SysConfigModel.UpdateConfig(settingsPath.FullName, SysConfigModel.CompileConstr(),
                SysConfigModel.Token,
                SysConfigModel.MinusSellCheck);
        }

        if (SysConfigModel.EnableBackup)
        {
            if(string.IsNullOrEmpty(SysConfigModel.BackupPath))
            {
                _ = DialogueServices.ShowMessageAsync("Error",
                    "Cannot enable backup when backup path is empty.");
                return;
            }
            
            if(!SysConfigModel.VerifyDatabaseAuth())
            {
                _ = DialogueServices.ShowMessageAsync("Error",
                    "Cannot enable backup when database auth is missing.");
                return;
            }
            
            if(SysConfigModel.BackupMinutes < 3)
            {
                _ = DialogueServices.ShowMessageAsync("Error",
                    "Please select a backup interval that is at least 3 minutes.");
                return;
            }
            
            if (SysConfigModel.BackupPath is not null && !Directory.Exists(SysConfigModel.BackupPath))
            {
                Directory.CreateDirectory(SysConfigModel.BackupPath);
            }
        }
        
        //StringGlobals.BackendUrl = BackendUrl;
        SysConfigModel.Token = string.Empty;
        PreferencesService.Set(s => s.SysConfigModel = SysConfigModel);

        var cfg = Config.Get;
        cfg.BackendUrl = BackendUrl;
        Config.SaveSettings(cfg);
        
        ContentHelper.CloseWindow<SysConfigPage>();
    }
    
    [RelayCommand]
    private void OnClose()
    {
        ContentHelper.CloseWindow<SysConfigPage>();
    }
}