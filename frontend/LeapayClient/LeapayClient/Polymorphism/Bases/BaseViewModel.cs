using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Avalonia.Media;
using Fly.Framework.Shared.Common;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.ClientModels.Validation;
using Leapay.Client.Features.Theming;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using Leapay.Client.UdpDiscovery;
using Leapay.Client.UI.ViewModels;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.HttpServices;
using SukiUI;

namespace Leapay.Client.Polymorphism.Bases;

public abstract partial class BaseViewModel<T> : FeatureBase<T>,
    ISynchronisationAware,
    IViewModel,
    IDisposable,
    IValidationEnforcer where T: class, new()
{
    [ObservableProperty]
    public partial IBrush? BackgroundTheme { get; set; }
    
    protected BaseViewModel()
    {
        InitBaseModel();
    }

    public event EventHandler<object[]>? SynchronisationRequested;
    
    protected LocalisationService? LocaleService { get; set; }
    
    [ObservableProperty]
    public partial FlowDirection FlowDirection { get; set; }
    
    [ObservableProperty]
    public partial string ApplicationTitle { get; set; } = "";
    
    [ObservableProperty]
    public partial bool IsResizing { get; set; }

    public Window? WindowLock { get; set; }
    
    [ObservableProperty]
    public partial ComboModelBase ComboModelBase { get; set; } = new();

    [ObservableProperty]
    public partial SessionService? Session { get; set; }

    public virtual double SearchableComboWidth => 260;

    [ObservableProperty]
    public partial bool SerialNumberVisibility { get; set; } = PreferencesService.Get(s => s.SerialNumberVisible);

    protected SukiTheme? Theme { get; set; }

    private void InitBaseModel()
    {
        SetClient(App.GetService<HttpClient>(), BackendService.CreatePinnedHandler());
        LocaleService = App.GetService<LocalisationService>();
        Session = App.GetService<SessionService>();
        FlowDirection = LocaleService.DetermineFlowDirection();
        
        As<ISynchronisationAware>().Register(this);
        
        ApplicationTitle = LocaleService["ApplicationTitle"];
        
        Theme = SukiTheme.GetInstance();
        BackgroundTheme = Theme.ActiveColorTheme?.PrimaryBrush;
        
        Theme.OnColorThemeChanged += theme =>
        {
            BackgroundTheme = theme.PrimaryBrush;
        };
    }

    protected virtual async Task RequestApiComboItemsFromInnerEndpointAsync(
        string innerEndpoint,
        int index = 0,
        string opHeader = "",
        string? defaultText = null)
    {
        Information("Preparing to receive combo box data for path: {0}" +
                    " at {1} with op header ({2}), satisfying: {3}",
            innerEndpoint, index, opHeader, GetType().Name);

        // causes second binding to fail
        //ComboModelBase.SetTemporaryApiComboItem(index);

        var request = await RequestComboModelsAsync<ComboModel>(innerEndpoint, opHeader);

        Verbose("Combo items for {0} received with: {1}",
            GetType().Name,
            request.Data.GetType().Name);

        ComboModelBase.SetApiComboItems(request.Data, index, defaultText: defaultText);
    }

    protected async Task PrepareComboBoxAsync(
        string innerEndpoint,
        Identifier? property,
        int index = 0,
        string opHeader = "")
    {
        await RequestApiComboItemsFromInnerEndpointAsync(innerEndpoint, index, opHeader);
        ComboModelBase.SetComboModelOrDefault(property, index);
    }

    public string this[string key]
    {
        get => LocaleService?[key] ?? key;
    }

    public virtual Task OnSynchronisationRequestedAsync(params object[] args)
    {
        SynchronisationRequested?.Invoke(this, args);
        return Task.CompletedTask;
    }

    protected virtual EnforcementResult Enforce()
    {
        throw new NotImplementedException($"Enforce needs to be overriden for {typeof(T)}.");
    }

    protected string GetPageName(Type type, EntryType entryType)
    {
        return LocaleService[$"{type.Name}{entryType.ToString()}"];
    }

    public FrozenDictionary<string, string> GetLocale(out bool isLtr)
    {
        isLtr = FlowDirection == FlowDirection.LeftToRight;
        return LocaleService.GetCurrentLocale();
    }
    
    public void SynchronizeAllBase(params object[] args)
    {
        As<ISynchronisationAware>().SynchronizeAll(args);
    }

    protected void SynchroniseTypeBase(Type? type, params object[] args)
    {
        if (type is null)
            return;

        Debug("{0} is synchronising {1} with optional args... {@FD}",
            GetType().Name,
            type.Name,
            args);

        As<ISynchronisationAware>().SynchronizeType(type, args);
    }

    public BaseViewModel<T> AddValidation<TE>(
        Expression<Func<TE>> property,
        Tuple<Predicate<TE>, string>[]? rules = null,
        bool optional = false)
    {
        As<IValidationEnforcer>().AddValidation(CreateValidation(property, rules, optional));

        return this;
    }

    protected ValidationModel CreateValidation<TE>(
        Expression<Func<TE>> property,
        Tuple<Predicate<TE>, string>[]? rules = null,
        bool optional = false)
    {
        if (property.Body is not MemberExpression { Member.Name: not null } propertyEx)
            throw new ArgumentNullException(nameof(property), "A correct property must be given.");
        
        var ruleModels = rules?
            .Select(rule => new ValidationRule(rule.Item1, rule.Item2))
            .ToList();

        return new ValidationModel(propertyEx.Member.Name,
                ruleModels, optional);
    }
    
    protected virtual bool PreSaveValidations()
    {
        var enforcements = Enforce();
        return VerifyValidations(enforcements);
    }

    protected async Task SetStateAsync(Func<Task> action)
    {
        await action();
        OnPropertyChanged();
    }

    protected void SetState(Action action)
    {
        action();
        OnPropertyChanged();
    }
    
    [ObservableProperty]
    public partial string BusyText { get; set; } = "";
    
    [ObservableProperty]
    public partial string SecondaryBusyText { get; set; } = "";
    
    [ObservableProperty]
    public partial bool IsBusy { get; set; }
    
    [ObservableProperty]
    public partial string CommandBusyText { get; set; } = "";
    
    [ObservableProperty]
    public partial ICommand? CommandBusyAction { get; set; }

    protected bool VerifyValidations(EnforcementResult result)
    {
        if (result.IsSuccess())
            return true;
        
        _ = DialogueServices.ShowMessageAsync("ValidationFailed",
            result.GetDisplayMessage());

        return false;
    }
    
    /// <summary>
    /// Used to cast a base class to an inherited interface for convenient multi-inheritance.
    /// </summary>
    /// <typeparam name="TInterface">The interface to cast to.</typeparam>
    /// <returns>The cast base class.</returns>
    /// <exception cref="InvalidCastException">Thrown when a base class does not inherit <see cref="TInterface"/>.</exception>
    protected TInterface As<TInterface>() where TInterface : class
    {
        if (this is not TInterface @interface)
        {
            throw new InvalidCastException($"The calling base class {GetType().Name} must " +
                                           $"inherit the interface {typeof(TInterface).Name} " +
                                           $"before it can use its multi-inheritance features.");
        }

        return @interface;
    }

    public async Task WrapEndpointAsync(string endpoint, Func<Task> action)
    {
        var tempEndpoint = Endpoint;
        Endpoint = endpoint;
        try
        {
            await action();
        }
        finally
        {
            Endpoint = tempEndpoint;
        }
    }
    
    protected void SynchroniseTypeBase<TSync>(params object[] args)
    {
        SynchroniseTypeBase(typeof(TSync), args);
    }
    
    protected void On(Func<bool> condition, Action action)
    {
        if (condition())
        {
            action();
        }
    }
    
    protected async Task OnAsync(Func<bool> condition, Func<Task> action)
    {
        if (condition())
        {
            await action();
        }
    }
    
    protected void On<TEnum>(TEnum original, TEnum val, Action action)
    {
        if (original.Equals(val))
        {
            action();
        }
    }
    
    protected async Task OnAsync<TEnum>(TEnum original, TEnum val, Func<Task> action)
    {
        if (original.Equals(val))
        {
            await action();
        }
    }

    public List<ValidationModel> Validations { get; set; } = [];

    public virtual void Dispose()
    {
        
    }
    
}