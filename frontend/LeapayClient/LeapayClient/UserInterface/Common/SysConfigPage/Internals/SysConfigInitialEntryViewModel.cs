using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Helpers;

namespace Leapay.Client.UserInterface.Common.SysConfigPage.Internals;

public partial class SysConfigInitialEntryViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    protected override string Endpoint => "";

    [ObservableProperty]
    public partial string? Hash { get; set; }

    public SysConfigInitialEntryViewModel(
        DtoBase? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        
        Refresh();
    }

    private void Refresh()
    {
        SetRefreshState(false);
    }

    [ObservableProperty]
    public partial string? InfoText { get; set; }

    [RelayCommand]
    private async Task OnActionAsync(string parameter)
    {
        SetRefreshState(true);

        try
        {
            SetInfo(LocaleService["Checking"]);
            if (!DataComputeHelper.VerifyHash(Hash, "bit8GYG3OabN3chaEj3rToljxJAhjAq9xqb7nDd+MEyfwYMu3UfJT412AbvuX8hM"))
            {
                SetRefreshState(false);
                SetInfo(LocaleService["Error"]);
                return;
            }

            Success();
        }
        catch (Exception x)
        {
            Unsuccessful(x.Message);
        }

        return;

        void Unsuccessful(string message)
        {
            SetRefreshState(false);
            InfoText = $"{LocaleService["UnsuccessfulLookup"]}\n{message}";
        }
        
        async void Success()
        {
            SetInfo(this["Success"]);
            App.CloseDialogWindowOfContentType<SysConfigInitialEntryWindow>();
            
            await ContentHelper.ShowCrossPlatformContentAsync(new SysConfigPage(new(new(), EntryType.Add)),
                new(this["SysConfig"], false, false, false, false));
        }
    }
}