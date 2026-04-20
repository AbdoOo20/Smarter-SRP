using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Sales.Safes;

public partial class SafeEntryViewModel : BaseEntryViewModel<DtoSafe>
{
    protected override string Endpoint => "Safe";

    public StaticComboVm<PeymentMode> PaymentType { get; } = new();

    [ObservableProperty]
    public partial bool LossVisibility { get; set; } = false;

    public SafeEntryViewModel(
        DtoSafe? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        AddValidation(() => Model.Name, optional: false);

        Refresh();
    }
    
    private async void Refresh()
    {
        PaymentType.Set(Model.PeymentMode, m =>
        {
            LossVisibility = m is PeymentMode.Services;

            Model.PeymentMode = m;
        });
    }
}