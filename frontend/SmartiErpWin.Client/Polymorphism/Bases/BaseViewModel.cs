using System.Runtime.CompilerServices;
using System.Windows.Media;
using Fly.Framework.Shared.Common;
using SmartErpWin.Core;
using SmartErpWin.Core.HttpServices;
using SmartiErpWin.Client.ClientModels.Validation;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.Helpers;
using SmartiErpWin.Client.Polymorphism.Interfaces;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public abstract partial class BaseViewModel<T>(HttpClient client) : FeatureBase<T>(client),
    ISynchronisationAware, IValidationEnforcer where T: class, new()
{

    public virtual bool IsEnabled { get; set; } = true;

    protected IContentDialogService? DialogService { get; set; }

    protected LocalisationService? LocaleService { get; set; }

    private Dictionary<string, string>? LocalisedDictionary { get; set; }

    [ObservableProperty]
    private FlowDirection _flowDirection;

    [ObservableProperty]
    private string _applicationTitle = "";

    [ObservableProperty]
    private ComboModelBase _comboModelBase = new();

    [ObservableProperty]
    private SessionService? _session;

    [ObservableProperty]
    private Brush _primaryButtonColour = new SolidColorBrush(Colors.CadetBlue);

    [ObservableProperty]
    private Brush _secondaryButtonColour = new SolidColorBrush(Colors.Transparent);

    [ObservableProperty]
    private Visibility _serialNumberVisibility = Config.Get.SerialNumberVisible.ApplyVisibility();

    protected void InitBaseModel(LocalisationService localeService,
        SessionService? sessionService,
        IContentDialogService? contentDialogService)
    {
        LocaleService = localeService;
        Session = sessionService;
        DialogService = contentDialogService;
        LocalisedDictionary = LocaleService.GetCurrentLocale();
        FlowDirection = LocaleService.DetermineFlowDirection();

        As<ISynchronisationAware>().Register(this);

        ApplicationTitle = LocaleService["ApplicationTitle"];
        ApplicationThemeManager.Apply(ApplicationThemeManager.GetAppTheme());
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

        private set
        {
            if (!LocalisedDictionary?.TryAdd(key, value) ?? false)
            {
                if (LocalisedDictionary[key] == value)
                    return;

                LocalisedDictionary[key] = value;
                OnPropertyChanged($"Item[{key}]");
            }
            else
            {
                OnPropertyChanged($"Item[{key}]");
            }
        }
    }

    public virtual Task OnSynchronisationRequestedAsync(params object[] args)
    {
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

    /// <summary>
    /// Used to cast a base class to an inherited interface for convenient multi-inheritance.
    /// </summary>
    /// <typeparam name="TInterface">The interface to cast to.</typeparam>
    /// <returns>The cast base class.</returns>
    /// <exception cref="InvalidCastException">Thrown when a base class does not inherit <see cref="TInterface"/>.</exception>
    protected TInterface As<TInterface>([CallerMemberName] string test = "") where TInterface : class
    {
        if (this is not TInterface @interface)
        {
            throw new InvalidCastException($"The calling base class {GetType().Name} must " +
                                           $"inherit the interface {typeof(TInterface).Name} " +
                                           $"before it can use its multi-inheritance features.");
        }

        return @interface;
    }

    protected void SynchroniseTypeBase<TSync>(params object[] args)
    {
        SynchroniseTypeBase(typeof(TSync), args);
    }
}