using Fly.Shared.Modules.Personnel.Dtos;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Personnel.DiscountsAndIncreases;

public partial class DiscountIncreasePageViewModel : BasePageViewModel<DtoDiscountIncrease>
{
    protected override string Endpoint => "DiscountIncrease";
    
    public DiscountIncreasePageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        
        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoDiscountIncrease>(sender);

        if (model is null)
            return;
        
        var entry = new DiscountIncreaseEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());
        
        entry.ShowDialog();
    }
    
    [RelayCommand]
    private void OnAdd()
    {
        var entry = new DiscountIncreaseEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());
        
        entry.ShowDialog();
    }
}