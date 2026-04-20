using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Sales.SalesInvoice;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.Sales.SalesPoint.SuspendedInvoices;

public sealed partial class SuspendedInvoicesViewModel : BaseRefreshableEntryViewModel<DtoBase>
{
    [ObservableProperty]
    public partial string? SearchText { get; set; }


    protected override string? Endpoint { get; set; } = "Sales";

    [ObservableProperty]
    public partial ObservableCollection<SuspendedInvoiceModel> ModelsInView { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<SuspendedInvoiceModel> Models { get; set; } = [];
    
    [ObservableProperty]
    public partial bool DataExists { get; set; }
    
    public event EventHandler? ItemsFetched;
    
    public void OnSearch(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            Models.Observe(ModelsInView);
            return;
        }
        
        var search = Models.Where(m =>
            m.InvoiceNumber.ToString().Contains(text.Simplify())
                                       || (m.ClientName?.Simplify().Contains(text.Simplify()) ?? false)
                                       || (m.ClientPhone?.Simplify().Contains(text.Simplify()) ?? false)
                                       || (m.InvoiceDate?.Simplify().Contains(text.Simplify()) ?? false));

        search.Observe(ModelsInView);
    }
    
    public SuspendedInvoicesViewModel(
        DtoBase? model,
        EntryType entryType)
    {
        SetDetails(model, entryType, GetType());
        
        SetRefreshState(false);
        _ = PrepareAsync();
    }

    public async Task PrepareAsync()
    {
        DataExists = true;
        SetRefreshState(true);
        
        var models = await RequestComboModelsAsync<SuspendedInvoiceModel>("Suspended");

        if (!models.Status || models.Data is null)
        {
            _ = DialogueServices.ShowContactSupportMessageAsync();
            return;
        }
        
        models.Data?.Observe(Models);
        OnSearch("");
        
        SetRefreshState(false);
        DataExists = ModelsInView.Count > 0;
        UiVisibility = DataExists;
    }
    
    [RelayCommand]
    private async Task OnActionAsync(SuspendedInvoiceModel suspendedInvoiceModel)
    {
        var req = await GetSpecificOneAsync<DtoSalesInvoice>(suspendedInvoiceModel.InvoiceId);

        if (!req.Status)
        {
            _ = DialogueServices.ShowContactSupportMessageAsync();
        }

        var vm = new SalesInvoiceEntryViewModel(req.Data, EntryType.Edit, typeof(SalesInvoicePage));
        vm.ShouldDisplayToastNotifications = false;
        vm.Model.InvoiceStatus = InvoiceStatus.InvoicePaid;
        await vm.SaveCommand.ExecuteAsync(null);

        await PrepareAsync();
    }
}