using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Requests;
using SmartErpWin.Core;
using Fly.Shared.Modules.Financial.Dtos;
using Newtonsoft.Json;
using SmartiErpWin.Client.Helpers;

namespace SmartiErpWin.Client.UserInterface.Financial.FinancingOperations.ModelBase;

public abstract partial class FinanceOperationViewModelBase<T> : BaseEntryViewModel<T> where T : RSimpleRequest, new()
{
    protected override string Endpoint => "Financial";

    private readonly string _innerEndpoint;

    protected virtual OperationType? FinanceType => null;

    private string? _thirdPartyEndpoint;

    [ObservableProperty]
    private Visibility _internalNotesVisibility = Config.Get.FinancialInternalNotesVisible.ApplyVisibility(true);
    
    protected FinanceOperationViewModelBase(Window window,
        LocalisationService locale,
        T? model,
        EntryType entryType,
        HttpClient client,
        Type pageType,
        string thirdPartyEndpoint,
        string innerEndpoint) : base(client)
    {
        EntryWindow = window;
        PageType = pageType;
        EntryType = entryType;

        Model = model ?? new();
        Model.Date = DateTime.Now;
        _innerEndpoint = innerEndpoint;

        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(GetType(), EntryType);

        _thirdPartyEndpoint = thirdPartyEndpoint;
        
        Refresh();
    }

    public async void Refresh()
    {
        ComboModelBase.SetStaticItemsInternal<OperationType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemEnum(FinanceType ?? OperationType.Deposit);
        
        await PrepareAdditionalAsync();
        OnPropertyChanged(nameof(Model));

        await PrepareComboBoxAsync(_thirdPartyEndpoint, new Identifier());
        await PrepareComboBoxAsync("Safes", new Identifier(), 1);
    }

    protected virtual Task PrepareAdditionalAsync()
    {
        return Task.CompletedTask;
    }

    protected override void PreSaveActions()
    {
        var isDeposit = ComboModelBase.AssertThatStaticIsAt();
        
        Model.FromAccount = ComboModelBase.GetSelectedItemId(isDeposit ? 0 : 1);
        Model.ToAccount = ComboModelBase.GetSelectedItemId(isDeposit ? 1 : 0);
    }

    protected override async Task SaveAsync()
    {
        if (!PreSaveValidations())
            return;

        PreSaveActions();
        var isDeposit = ComboModelBase.AssertThatStaticIsAt();

        var from = ComboModelBase.SelectedApiComboItems[isDeposit ? 0 : 1]?.Display;
        var to = ComboModelBase.SelectedApiComboItems[isDeposit ? 1 : 0]?.Display;

        var message = string.Format(LocaleService["FinanceOperationConfirmation"], Model?.Amount, from, to);

        if (!await DialogueServices.ConfirmAsync(LocaleService, GetPageName(GetType(), EntryType), message))
            return;

        Model.Type = ComboModelBase.AssertThatStaticIsAt() ? (int)OperationType.Withdraw : (int)OperationType.Deposit;
        var result = await SendFinancialRequestAsync(_innerEndpoint, Model);

        var others = isDeposit ? from : to;
        if (result.Status)
        {
            // dosn't work
            //var aaa = result.Data.Adapt<DtoStatment>(); 

            // dosn't work
            //var sss = JsonConvert.SerializeObject(result.Data);
            //var st = JsonConvert.DeserializeObject<DtoStatment>(sss);

            if (result.Data is not null)
            {
                // yeeeeeeeeeeees it's works
                var response = result.Data?.ToString() ?? string.Empty;
                var dtoStatement = JsonConvert.DeserializeObject<DtoStatment>(response);
                Model.ApiResponse = dtoStatement;
                Model.Number = dtoStatement.Number; // As
                Model.RemainText = dtoStatement.Balance  > 0 ? "باقي عليك" : "باقي لك";
                Model.RemainAmount = (dtoStatement.Debit - dtoStatement.Credit);
                LoadReport(isDeposit, others, response);
            }
            else
            {
                LoadReport(isDeposit, others);
            }

            EntryWindow?.Close();

            //As<ISynchronisationAware>().Synchronise<DailyEntriesPageViewModel, EntryPageViewModel>();
        }
    }

    protected virtual void LoadReport(bool isDeposit, string? others, string? date = null)
    {
        //EventHelper.InvokeFinancial(Model);
        EventHelper.InvokeFinancial2(this, date);
        EventHelper.InvokeFinancial(new(Model));
    }
}