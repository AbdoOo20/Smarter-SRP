using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.Polymorphism.Bases;

namespace Leapay.Client.UserInterface.Accounting.Entries;

public partial class EntriesEntryViewModel
{
    [ObservableProperty]
    public partial SearchableComboVm<DtoEntryDetail> Accounts { get; set; } = new("Account", 200);
    
    [ObservableProperty]
    public partial ModelDetailsViewModelBase<DtoEntryDetail> ModelDetails { get; set; } = new();
    
    private void SetDetailsModel(DtoEntryDetail? model)
    {
        Reset();
        ModelDetails.SetState(model);

        _ = LoadComboInformationAsync();
    }

    [RelayCommand]
    private void OnSubmit()
    {
        UpdateListing(ModelDetails.Model);
        Reset(true);
    }

    private void Reset(bool andSet = false)
    {
        ModelDetails = new();
        Accounts.Combo.Reset();

        if (andSet)
        {
            SetDetailsModel(null);
        }
    }

    private async Task LoadComboInformationAsync()
    {
        await Accounts.LoadAsync(this, "accounts",
            ModelDetails.Model.AccountId, s =>
            {
                if (s is null || ModelDetails?.Model is null)
                    return;
                
                ModelDetails.Model.AccountId = s.Id;
                ModelDetails.Model.AccountName = s.Display;
            });
    }
}