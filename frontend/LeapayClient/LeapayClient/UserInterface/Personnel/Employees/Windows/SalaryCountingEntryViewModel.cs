using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Global.Dtos;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.Employees.Windows;

public partial class SalaryCountingEntryViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    protected override string Endpoint => "";

    public SuperComboVm<DtoUser> Groups { get; } = new();

    [ObservableProperty]
    public partial string? ActionText { get; set; }

    [ObservableProperty]
    public partial long MounthNumber { get; set; }

    public SalaryCountingEntryViewModel(
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

        if (MounthNumber < 1 || MounthNumber > 12)
        {
            SetRefreshState(false);
            SetInfo(LocaleService["MounthNumberError"]);
            return;
        }

        try
        {
            SetInfo(LocaleService["CountingSalaryWorking"]);
            var request = await PostSpecificOneAsync<TypeOf<bool>>($"Employee/CountingSalary/{MounthNumber}");

            await Task.Delay(1000);

            if (request is { Data: not null, Status: true })
                Success(request.Data);
            else
                Unsuccessful(request?.ErrorMessage ?? "NO ERROR MESSAGE");
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
        
        void Success(TypeOf<bool> sale)
        {
            SetInfo(this["Success"]);
            App.CloseDialogWindowOfContentType<SalaryCountingEntryWindow>();
        }
    }
}