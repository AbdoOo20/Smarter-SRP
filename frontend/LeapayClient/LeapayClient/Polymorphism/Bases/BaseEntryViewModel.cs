using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Requests;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.ClientModels.Validation;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core.Enums;
using SukiUI.Toasts;

namespace Leapay.Client.Polymorphism.Bases;

public abstract partial class BaseEntryViewModel<T> : BaseViewModel<T> where T: class, new()
{
    [ObservableProperty]
    protected partial EntryType EntryType { get; set; }
    
    [ObservableProperty]
    public partial T? Model { get; set; }

    protected bool IsNameRequired { get; set; }

    public override double SearchableComboWidth => 490;
    
    private readonly TaskCompletionSource<T> _waitForSave = new();

    protected void SetDetails(T? model,
        EntryType entryType,
        Type pageType,
        bool isNameRequired = true)
    {
        Model = model ?? new();
        EntryType = entryType;
        PageType = pageType;
        ApplicationTitle = GetPageName(pageType, EntryType);
        IsNameRequired = isNameRequired;
        
        if (IsNameRequired && Model is DtoNamesBase namesBase)
        {
            AddValidation(() => namesBase.Name, optional: false);
        }
    }
    
    protected BaseEntryViewModel()
    {
        As<IValidationEnforcer>().ClearAll();
    }

    protected Window? EntryWindow { get; set; }
    
    protected Type? PageType { get; set; }
    
    [RelayCommand]
    private Task CancelAsync()
    {
        WindowClosure();
        
        return Task.CompletedTask;
    }

    protected virtual void AfterSave()
    {
        
    }
    
    public async Task<T> WaitUntilUserSavesAsync()
    {
        return await _waitForSave.Task;
    }
    
    [RelayCommand]
    protected virtual async Task SaveAsync()
    {
        await SaveAndReturnAsync();
    }
    
    [RelayCommand]
    public virtual async Task<(string, bool)> SaveWithResultAsync()
    {
        if (Model is null)
            return (this["ValidationFailed"], false);
     
        PreSaveActions();
        
        var save = await PostSaveAsync(Model);

        return (save.ErrorMessage ?? save.Error ?? this["NoError"], save.Status);
    }
    
    [RelayCommand]
    protected virtual async Task<List<(string?, int, string?)>> SaveBatchAsync(List<T> models)
    {
        var errors = new List<(string?, int, string?)>();
        
        for (var index = 0; index < models.Count; index++)
        {
            var model = models[index];
            Model = model;

            PreSaveActions();
            
            var enforce = Enforce();

            if (!enforce.IsSuccess())
            {
                errors.Add(new((Model as DtoNamesBase)?.Name,
                    index,
                    enforce.GetDisplayMessage()));
            }
        }

        if (errors.Count != 0)
        {
            return errors;
        }
        
        await PostSaveBatchAsync(models);

        return [];
    }
    
    [RelayCommand]
    protected async Task<T?> SaveAndReturnAsync()
    {
        if (Model is null || !PreSaveChecks())
            return null;
     
        PreSaveActions();

        if (!PreSaveValidations())
            return null;
        
        var save = await PostSaveAsync(Model);
        
        if (save is null)
            return null;
        
        App.GetService<ISukiToastManager>().CreateSimpleInfoToast()
            .WithTitle(GetPageName(PageType, EntryType))
            .WithContent(this[$"{(EntryType is EntryType.Add ? "Add" : "Edit")}OperationSuccessful"])
            .Queue();
        
        AfterSave();
        WindowClosure();
        
        SynchroniseTypeBase(PageType);

        _waitForSave.TrySetResult(save.Data);
        return save.Data;
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
    
    protected override EnforcementResult Enforce()
    {
        return As<IValidationEnforcer>().Enforce(Model);
    }
    
    public EnforcementResult PubEnforce()
    {
        return As<IValidationEnforcer>().Enforce(Model);
    }
    
    [RelayCommand]
    protected virtual Task RefreshAsync()
    {
        return Task.CompletedTask;
    }
    
    protected virtual async void PreSaveActions()
    {
        
    }

    protected virtual bool PreSaveChecks()
    {
        return true;
    }

    protected override async Task RequestApiComboItemsFromInnerEndpointAsync(
        string innerEndpoint,
        int index = 0,
        string opHeader = "",
        string? defaultText = null)
    {
        Information("Preparing to receive combo box data for path: {0}" +
                    " at {1} with op header ({2}), satisfying: {3}",
            innerEndpoint, index, opHeader, GetType().Name);
        
        //ComboModelBase.SetTemporaryApiComboItem(index);

        var request = await RequestComboModelsAsync<ComboModel>(innerEndpoint, opHeader);
        
        Verbose("Combo items for {0} received with: {1}",
            GetType().Name,
            request.Data.GetType().Name);
        
        ComboModelBase.SetApiComboItems(request.Data, index, defaultText: defaultText);
    }

    protected void SetStaticCombos<TCombo>(TCombo current) where TCombo : Enum
    {
        ComboModelBase.SetStaticItemsInternal<TCombo>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemEnum(current);
    }
    
    protected async Task PrepareManyComboBoxesAsync(List<Tuple<string, Identifier>> combos)
    {
        for (var i = 0; i < combos.Count; i++)
        {
            await PrepareComboBoxAsync(combos[i].Item1, combos[i].Item2, i);
        }
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
    
    protected new async Task<List<ComboModel>> RequestCombosUsingQueryRequestAndReturnAsync(
        string innerEndpoint,
        QueryRequestBase? reqBase = null)
    {
        var request = await RequestCombosUsingQueryRequestAsync<ComboModel>(
            innerEndpoint,
            reqBase ?? new());

        return request.Data.ToList();
    }
}