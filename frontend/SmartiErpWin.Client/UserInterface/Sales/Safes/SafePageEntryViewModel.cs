using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using SmartiErpWin.Client.Helpers;

namespace SmartiErpWin.Client.UserInterface.Sales.Safes;

public partial class SafePageEntryViewModel : BaseEntryViewModel<DtoSafe>
{
    protected override string Endpoint => "Safe";

    [ObservableProperty]
    private Visibility _lossVisibility = Visibility.Collapsed;
    
    public SafePageEntryViewModel(Window window,
        LocalisationService locale,
        DtoSafe? model,
        EntryType entryType, HttpClient client, Type pageType): base(client)
    {        
        EntryType = entryType;
        EntryWindow = window;
        Model = model ?? new();
        PageType = pageType;
        
        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        ComboModelBase.OnStaticComboItemChanged += () =>
        {
            CheckVisibilities();
            return default;
        };
        
        Refresh();
    }

    private async void Refresh()
    {
        ComboModelBase.SetStaticItemsInternal<PeymentMode>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemEnum(Model.PeymentMode);
        
        CheckVisibilities();
    }
    
    private void CheckVisibilities()
    {
        LossVisibility = ComboModelBase.AssertThatStaticIsAt(1).ApplyVisibility(true);
    }
    
    protected override void PreSaveActions()
    {
        Model.PeymentMode = ComboModelBase.ConvertCurrentToEnum<PeymentMode>();
    }
}