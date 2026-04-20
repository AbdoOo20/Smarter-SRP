using System.Linq.Expressions;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Requests;
using SmartiErpWin.Client.ClientModels.Validation;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.Polymorphism.Interfaces;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public abstract partial class BaseEntryViewModel<T> : BaseViewModel<T> where T: class, new()
{
    [ObservableProperty]
    private EntryType _entryType;

    [ObservableProperty]
    private T? _model;

    protected BaseEntryViewModel(HttpClient client) : base(client)
    {
        As<IValidationEnforcer>().ClearAll();
    }

    protected Window? EntryWindow { get; set; }
    
    protected Type? PageType { get; set; }
    
    [RelayCommand]
    private Task CancelAsync()
    {
        EntryWindow?.Close();
        
        return Task.CompletedTask;
    }

    [RelayCommand]
    protected virtual async Task SaveAsync()
    {
        if (Model is null || !PreSaveChecks())
            return;
     
        PreSaveActions();

        if (!PreSaveValidations())
            return;
        
        await PostSaveAsync(Model);
        //  todo: checleck if needed
        //ReportRunner.OnSaveComplate(new(Model));
        EntryWindow?.Close();
        
        SynchroniseTypeBase(PageType);
    }

    public BaseEntryViewModel<T> AddValidation<TE>(
        Expression<Func<TE>> property,
        Tuple<Predicate<TE>, string>[]? rules = null,
        bool optional = false)
    {
        if (property.Body is not MemberExpression { Member.Name: not null } propertyEx)
            throw new ArgumentNullException(nameof(property), "A correct property must be given.");
        
        var ruleModels = rules?
            .Select(rule => new ValidationRule(rule.Item1, rule.Item2))
            .ToList();

        As<IValidationEnforcer>().AddValidation(
            new ValidationModel(propertyEx.Member.Name,
            ruleModels, optional));

        return this;
    }

    protected override EnforcementResult Enforce()
    {
        return As<IValidationEnforcer>().Enforce(Model);
    }
    
    [RelayCommand]
    protected virtual Task RefreshAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual bool PreSaveValidations()
    {
        var enforcements = Enforce();

        if (enforcements.IsSuccess())
            return true;
        
        _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"],
            enforcements.GetDisplayMessage(), localeService: LocaleService);

        return false;
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
}