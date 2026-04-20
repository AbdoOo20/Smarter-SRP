using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Financial.Dtos;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Newtonsoft.Json;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.FinancingOperations.ModelBase;

public abstract partial class FinanceOperationViewModelBase<T> : BaseEntryViewModel<T> where T : RSimpleRequest, new()
{
    protected override string Endpoint => "Financial";

    private readonly string _innerEndpoint;

    protected virtual OperationType? FinanceType => null;

    private readonly string? _thirdPartyEndpoint;
    
    [ObservableProperty]
    public partial SuperComboVm<T> Safes { get; set; } = new();

    [ObservableProperty]
    public partial SearchableComboVm<T> Party { get; set; }
    
    [ObservableProperty]
    public partial bool InternalNotesVisibility { get; set; } = PreferencesService.Get(s => s.FinancialInternalNotesVisible);

    public double HorizontalUiWidth => SerialNumberVisibility ? 250 : 520;
    
    public double NotesUiHeight => SerialNumberVisibility ? 70 : 120;
    
    public double NotesUiHeightForInternalNotes => InternalNotesVisibility ? 70 : 110;
    
    public double AnyInternalsVisibleNotesHeight => InternalNotesVisibility || SerialNumberVisibility ? 70 : 110;

    public StaticComboVm<OperationType> FinanceTypes { get; } = new();

    protected FinanceOperationViewModelBase(
        T? model,
        EntryType entryType,
        Type pageType,
        string thirdPartyEndpoint,
        string innerEndpoint,
        string partyName = "")
    {
        SetDetails(model, entryType, GetType(), false);
        
        Model.Date = DateTime.Now;
        _innerEndpoint = innerEndpoint;
        
        _thirdPartyEndpoint = thirdPartyEndpoint;
        Party = new SearchableComboVm<T>(partyName, SearchableComboEntryWidth);

        AddValidation(() => Model.FromAccount).
            AddValidation(() => Model.ToAccount)
            .AddValidation(() => Model.Amount, [ new(r => r != 0, "TypeAnAmount") ]);
        
        Refresh();
    }

    public async void Refresh()
    {
        /*ComboModelBase.SetStaticItemsInternal<OperationType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemEnum(FinanceType ?? OperationType.Deposit)*/;
        FinanceTypes.Set(FinanceType ?? OperationType.Deposit, s => Model.Type = (int)s);

        await PrepareAdditionalAsync();
        OnPropertyChanged(nameof(Model));

        /*await PrepareComboBoxAsync(_thirdPartyEndpoint, new Identifier());
        await PrepareComboBoxAsync("Safes", new Identifier(), 1);*/
        await Party.LoadAsync(this, _thirdPartyEndpoint, new Identifier());
        await Safes.LoadAsync(this, "Safes", new Identifier());
    }

    protected virtual Task PrepareAdditionalAsync()
    {
        return Task.CompletedTask;
    }
    
    protected override void PreSaveActions()
    {
        var isDeposit = FinanceTypes.GetSelectedItem() is OperationType.Deposit;

        if (Party.Combo.SelectedItem is null || Safes.SelectedItem is null || Model is null)
        {
            Warning("Either Safe or Party does not have a value while " +
                    "trying to finish a financial process.");
            // Return here. Let the validation system do the rest.
            return;
        }
        
        Model.FromAccount = isDeposit ? Party.Combo.SelectedItem.Id : Safes.SelectedItem.Id;
        Model.ToAccount = isDeposit ? Safes.SelectedItem.Id : Party.Combo.SelectedItem.Id;
        
        Model.FromName = isDeposit ? Party.Combo.SelectedItem.Display : Safes.SelectedItem.Display;
        Model.ToName = isDeposit ? Safes.SelectedItem.ToString() : Party.Combo.SelectedItem.ToString();
    }

    protected override async Task SaveAsync()
    {
        PreSaveActions();
        
        if (!PreSaveValidations())
            return;

        var isDeposit = FinanceTypes.GetSelectedItem() is OperationType.Deposit;
        
        var from = Model.FromName;
        var to = Model.ToName;
        Model.Type = (int)FinanceTypes.GetSelectedItem() is 0 ? 1 : 0;
        
        var message = string.Format(LocaleService["FinanceOperationConfirmation"], Model?.Amount, from, to);

        if (!await DialogueServices.ConfirmAsync(GetPageName(GetType(), EntryType), message))
            return;
        
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
                Model.RemainText = dtoStatement.Balance  > 0 ? "باقي عليك:" : "باقي لك:";
                Model.RemainAmount = (dtoStatement.Debit - dtoStatement.Credit);
                LoadReport(isDeposit, others, response);
            }
            else
            {
                LoadReport(isDeposit, others);
            }

            WindowClosure();

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