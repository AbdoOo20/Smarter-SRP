using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Fly.Framework.Shared.Common.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.ClientModels.Validation;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using SmartErpWin.Core.Enums;
using SukiUI.Toasts;

namespace Leapay.Client.Polymorphism.Bases;

public partial class BasePageEntryViewModel<T, TD> : BaseRefreshablePageViewModel<TD>
    where TD: class, new()
    where T: class, new()
{
    [ObservableProperty]
    public partial EntryType EntryType { get; set; }
    
    [ObservableProperty]
    public partial T? Model { get; set; }

    [ObservableProperty]
    public partial Type? PageType { get; set; }

    [ObservableProperty]
    public partial bool IsReadOnly { get; set; }
    
    protected override bool CanSelectInDataGrid => true;

    protected override double DataGridHeight => EntryDataGridDefaultHeight;
    
    protected override bool IsCompact => true;
    
    [ObservableProperty]
    public partial bool ShouldDisplayToastNotifications { get; set; }
    
    protected Window? EntryWindow { get; set; }
    
    protected bool IsNameRequired { get; set; }

    protected bool IsSilent { get; set; }
    
    protected string InnerSaveEndpoint { get; set; } = string.Empty;
    
    private TaskCompletionSource<Identifier> _waitForSave = new();

    private readonly TaskCompletionSource _invoiceNumberInitLock = new();
    
    protected async void SetDetails(T? model,
        EntryType entryType,
        Type pageType,
        bool isNameRequired = true,
        bool silent = false)
    {
        Model = model ?? new();
        EntryType = entryType;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);
        IsNameRequired = isNameRequired;
        IsSilent = silent;
        
        if (IsNameRequired && Model is DtoNamesBase namesBase)
        {
            AddValidation(() => namesBase.Name, optional: false);
        }
    }

    [RelayCommand]
    public virtual async Task<(string, bool)> SaveWithResultAsync()
    {
        if (Model is null || !PreSaveChecks())
            return (this["ValidationFailed"], false);
     
        PreSaveActions();
        
        var save = await PostSaveSpecificAsync(Model, InnerSaveEndpoint);

        return (save.ErrorMessage ?? save.Error ?? this["NoError"], save.Status);
    }
    
    protected async Task SetDetailsByInvoiceNumberOrModel(T? model,
        EntryType entryType,
        Type pageType,
        bool isNameRequired = true,
        long? invoiceNumber = null)
    {
        if (invoiceNumber is not null)
        {
            var modelReq = await GetSpecificOneByStringQueryAsync<T>("ByNumber",
                [new Tuple<string, string?>("number", invoiceNumber.ToString())]);

            try
            {
                if (!modelReq.Status)
                    throw new();
            }
            catch
            {
                true.Stop("InvoiceNotFound", "TheInvoiceNumberWasNotFound");
            }

            model = modelReq.Data;
            IsReadOnly = true;
        }
        
        SetDetails(model, entryType, pageType, isNameRequired);
    }
    
    public override bool ShouldRenderFullPage => false;
    
    
    public BasePageEntryViewModel()
    {
        Limit = 100;
        CurrentPage = 1;
        SearchText = "";
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        WindowClosure();
        _ = _waitForSave.TrySetResult(default);
        
        return Task.CompletedTask;
    }
    
    protected virtual bool PreSaveChecks()
    {
        return true;
    }
    
    public EnforcementResult PubEnforce()
    {
        return As<IValidationEnforcer>().Enforce(Model);
    }
    
    [RelayCommand]
    protected virtual async Task SaveAsync(Action? successCallback = null)
    {
        _ = await SaveAndReturnAsync(successCallback);
    }
    
    [RelayCommand]
    protected async Task<Identifier?> SaveAndReturnAsync(Action? successCallback = null)
    {
        if (Model is null || !PreSaveChecks())
            return null;

        if (IsReadOnly.Stop("SaveNotPossible", "ItemIsReadOnlyReopenFromAnotherWindow"))
            return null;
        
        PreSaveActions();
        
        var req = await PostSaveSpecificAsync(Model, InnerSaveEndpoint);
        var id = (req.Data as DtoBase)?.Id;

        successCallback?.Invoke();
        
        _waitForSave.TrySetResult(id.GetValueOrDefault());

        if (ShouldDisplayToastNotifications)
        {
            App.GetService<ISukiToastManager>().CreateSimpleInfoToast()
                .WithTitle(GetPageName(PageType, EntryType))
                .WithContent(this[$"{(EntryType is EntryType.Add ? "Add" : "Edit")}OperationSuccessful"])
                .Queue();
        }
        
        WindowClosure();
        
        SynchroniseTypeBase(PageType);
        return id;
    }

    public async Task<Identifier> WaitUntilUserSavesAsync()
    {
        return await _waitForSave.Task;
    }
    
    protected override EnforcementResult Enforce()
    {
        return As<IValidationEnforcer>().Enforce(Model);
    }
    
    protected virtual void PreSaveActions() { }
    
    protected async Task RequestApiComboItemsFromInnerEndpointAsync(string innerEndpoint, int index = 0)
    {
        var request = await RequestComboModelsAsync<ComboModel>(innerEndpoint);
        
        ComboModelBase.SetApiComboItems(request.Data, index, Model as DtoBase);
    }

    protected new async Task RequestCombosUsingQueryRequestAsync(
        string innerEndpoint,
        int index = 0,
        QueryRequestBase? reqBase = null)
    {
        var request = await RequestCombosUsingQueryRequestAsync<ComboModel>(
            innerEndpoint,
            reqBase ?? new());
        
        ComboModelBase.SetApiComboItems(request.Data, index, Model as DtoBase);
    }
    
    public static IEnumerable<IDetailsModel> UpdateListGlobal(IDetailsModel model, List<IDetailsModel> list)
    {
        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            list.Add(model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        return list;
    }
    
    protected async Task GetNextNumber()
    {
        var url = "";
    }

    protected virtual void WindowClosure()
    {
        if (WindowLock is not null)
        {
            WindowLock.Close();
        }
        else if (EntryWindow is not null)
        {
            EntryWindow.Close();
        }
    }
    
    protected ComboModel InitNewComboModel(string name, Guid id)
    {
        return new()
        {
            Display = name,
            Value = id.ToString()
        };
    }
}