using System.Collections.ObjectModel;
using Fly.Framework.Shared.Common.Dtos;
using SmartErpWin.Core.Extensions;
using SmartiErpWin.Client.DtoModels.UiModels;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.OperationWindows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations;

public partial class FinancingOperationsPageViewModel : BaseViewModel<DtoBase>
{
    protected override string Endpoint => "Category";
    
    [ObservableProperty]
    private ObservableCollection<CardModel> _cards = [];
    
    public FinancingOperationsPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService): base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);
        
        LoadInterface();
    }

    private void LoadInterface()
    {
        BulkCreateCards(
            [
                New(() => new AccountEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "AccountPaymentReceipt", "AccountFinanceOperation"),
                New(() => new ClientBillEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "ClientPaymentReceipt", "ClientFinanceOperation"),
                New(() => new SupplierBillEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "SupplierPaymentReceipt", "SupplierFinanceOperation"),
                New(() => new EmployeeFinanceEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "EmployeePaymentReceipt", "EmployeeFinanceOperation"),
                New(() => new GeneralExpenseEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "ExpensePaymentReceipt", "ExpenseFinanceOperation"),
                //New(() => new DiscountDepositFinanceEntryWindow(LocaleService, null,
                //    EntryType.Add, HttpClient, GetType()).ShowDialog(), "DiscountPaymentReceipt", "DiscountFinanceOperation"),
                New(() => new DiscountItemFinanceEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "DiscountItemPaymentReceipt", "DiscountItemFinanceOperation"),
                //New(() => new MarketerFinanceEntryWindow(LocaleService, null,
                //    EntryType.Add, HttpClient, GetType()).ShowDialog(), "MarketerPaymentReceipt", "MarketerFinanceOperation"),
                New(() => new ClientRepresentativeReceiptEntryWindow(LocaleService, null,
                    EntryType.Add, HttpClient, GetType()).ShowDialog(), "ClientPaymentReceipt", "ClientRepresentativeReceipt"),
            ]);
    }

    private CardModel New(Action window, string endpoint, string name, SymbolRegular symbol = SymbolRegular.CardUi24)
    {
        return new CardModel
        {
            CardName = name,
            CardSymbol = symbol,
            IsVisible = (App
                    .GetService<SessionService>()?
                    .JudgeAccessAsync(endpoint, ShowPermission)
                    .GetAwaiter()
                    .GetResult())
                .GetValueOrDefault()
                .ApplyVisibility(),
            OnClick = window.Invoke
        };
    }
    
    private void BulkCreateCards(params CardModel[] cards)
    {
        cards.Observe(Cards);
    }
}