using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels.UiModels;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.UserInterface.FinancingOperations.OperationWindows;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;

namespace Leapay.Client.UserInterface.FinancingOperations;

public partial class FinancingOperationsPageViewModel : BasePageViewModel<DtoBase>
{
    public FinancingOperationsPageViewModel()
    {
        LoadInterface();
    }
    
    [ObservableProperty]
    public partial ObservableCollection<CardModel> Cards { get; set; } = [];
    
    private void LoadInterface()
    {
        Create();
    }

    private void Create()
    {
        BulkCreateCards(
            [
                New(async () => await ShowContentAsync(new AccountEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "AccountFinanceOperation"), "AccountPaymentReceipt", "AccountFinanceOperation"),
                New(async () => await ShowContentAsync(new ClientBillEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "ClientFinanceOperation"), "ClientPaymentReceipt", "ClientFinanceOperation"),
                New(async () => await ShowContentAsync(new SupplierBillEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "SupplierFinanceOperation"), "SupplierPaymentReceipt", "SupplierFinanceOperation"),
                New(async () => await ShowContentAsync(new EmployeeFinanceEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "EmployeeFinanceOperation"), "EmployeePaymentReceipt", "EmployeeFinanceOperation"),
                New(async () => await ShowContentAsync(new GeneralExpenseEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "ExpenseFinanceOperation"), "ExpensePaymentReceipt", "ExpenseFinanceOperation"),
                //New(() => new DiscountDepositFinanceEntryWindow(LocaleService, null,
                //    EntryType.Add, HttpClient, GetType()).ShowDialog(), "DiscountPaymentReceipt", "DiscountFinanceOperation"),
                New(async () => await ShowContentAsync(new DiscountItemFinanceEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "DiscountItemFinanceOperation"), "DiscountItemPaymentReceipt", "DiscountItemFinanceOperation"),
                //New(() => new MarketerFinanceEntryWindow(LocaleService, null,
                //    EntryType.Add, HttpClient, GetType()).ShowDialog(), "MarketerPaymentReceipt", "MarketerFinanceOperation"),
                New(async () => await ShowContentAsync(new ClientRepresentativeReceiptEntryWindow(new(new(),
                    EntryType.Add, GetType())), name: "ClientRepresentativeReceipt"), "ClientPaymentReceipt", "ClientRepresentativeReceipt"),
            ]);
    }
    
    private CardModel New(Action window, string endpoint, string name, MaterialIconKind symbol = MaterialIconKind.Card)
    {
        const string parent = "FinancialOperation.";
        const string action = "-Create";
        var displayName = endpoint;
        if (name is "ClientRepresentativeReceipt")
            displayName = name;
        
        var isVisible = Session.JudgeSpecificAccess(parent + displayName + action);
        return new CardModel
        {
            CardName = name,
            CardSymbol = symbol,
            IsVisible = isVisible,
            OnClick = window.Invoke
        };
    }
    
    private void BulkCreateCards(params CardModel[] cards)
    {
        cards.Observe(Cards);
    }
}