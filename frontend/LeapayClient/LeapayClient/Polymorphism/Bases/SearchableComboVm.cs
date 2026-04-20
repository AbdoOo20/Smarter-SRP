using Fly.Framework.Shared.Common;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Helpers;
using Leapay.Client.Services;

namespace Leapay.Client.Polymorphism.Bases;

public partial class SearchableComboVm<T>: ObservableObject where T: class, new()
{
    private readonly object _name;
    
    private readonly double? _width;

    [ObservableProperty]
    public partial SuperComboVm<T> Combo { get; set; } = new();

    [ObservableProperty]
    public partial bool IsLoading { get; set; }
    
    [ObservableProperty]
    public partial object? Content { get; set; }
    
    public SearchableComboVm(object name, double? width = null)
    {
        _name = name;
        _width = width;
        
        // To ensure consistent width before and after the data is loaded,
        // always set a width from within the constructor.
        
        if(_width is not null)
            Refresh<BaseViewModel<T>>(_width.Value, null);
    }
    
    public async Task<SuggestBoxCreationInfo<ComboModel>> LoadAsync<TLoader>(TLoader loader,
        string? endpoint = null,
        Identifier? def = null,
        Action<ComboModel>? onSelectionChanged = null,
        string? header = null,
        Func<Task<string>>? adornerRightClick = null,
        Func<string, Task<string>>? adornerRightSelectedClick = null,
        string? adornerRightClickEndpointJudge = null,
        string? adornerRightSelectedClickEndpointJudge = null,
        AutoSuggestNotFoundCallbackModel? notFoundCallback = null,
        Action<Button>? actionAdorner = null) where TLoader: BaseViewModel<T>
    {
        await Combo.LoadAsync(loader, endpoint, def, onSelectionChanged, header, false);

        if (!loader.Session.JudgeSpecificAccess(adornerRightClickEndpointJudge))
            adornerRightClick = null;
        
        if (!loader.Session.JudgeSpecificAccess(adornerRightSelectedClickEndpointJudge))
            adornerRightSelectedClick = null;
        
        if (notFoundCallback is not null)
        {
            notFoundCallback.EntityCreated += async (_, @base) =>
            {
                await SharedCallback(@base.Id.ToString());
            };
        }
        
        return Refresh(_width ?? loader.SearchableComboWidth, loader, adornerRightClick,
            rightAdornerCallback: SharedCallback,
            rightAdornerSelectedCallback: SharedCallback,
            adornerRightSelectedClick: adornerRightSelectedClick,
            notFoundCallback: notFoundCallback,
            actionAdorner: actionAdorner);

        async Task SharedCallback(string a)
        {
            await LoadAsync(loader, endpoint, def, onSelectionChanged, header, adornerRightClick,
                adornerRightSelectedClick: adornerRightSelectedClick, actionAdorner: actionAdorner,
                notFoundCallback: AutoSuggestNotFoundCallbackModel.CreateClientNotFoundCallback());
            
            TryAutoSelect(a);
        }
    }

    private void TryAutoSelect(string res)
    {
        var item = Combo.Items.FirstOrDefault(n => n.Value == res);

        if (item is null)
            return;
        
        Combo.SelectedItem = item;
        DataGridServices.FindChildOfType<AutoCompleteBox>(Content as Visual).SelectedItem = Combo.SelectedItem;
    }
    
    public void Load<TLoader>(TLoader loader, List<ComboModel> models,
        Identifier? def = null,
        Action<ComboModel>? onSelectionChanged = null,
        Func<Task<string>>? adornerRightClick = null) where TLoader: BaseViewModel<T>
    {
        Combo.Load(models, def, onSelectionChanged, false);
        Refresh(_width ?? loader.SearchableComboWidth, loader, adornerRightClick);
    }
    
    private SuggestBoxCreationInfo<ComboModel> Refresh<TLoader>(double width,
        TLoader? loader,
        Func<Task<string>>? adornerRightClick = null,
        Func<string, Task>? rightAdornerCallback = null,
        Func<string, Task<string>>? adornerRightSelectedClick = null,
        Func<string, Task>? rightAdornerSelectedCallback = null,
        AutoSuggestNotFoundCallbackModel? notFoundCallback = null,
        Action<Button>? actionAdorner = null) where TLoader: BaseViewModel<T>
    {
        var stackPanel = new StackPanel();
        var localeService = App.GetService<LocalisationService>();

        var info = UiHelpers.CreateSuggestionBox(stackPanel,
            _name is string nameStr ? localeService[nameStr] : _name,
            Combo.Items.ToList(),
            onClear: () => Combo.SelectedItem = null,
            width: width, actionAdorner: actionAdorner ?? (but =>
            {
                if (loader is null)
                    return;

                but.Background = loader.BackgroundTheme;
            }), rightAdornerAction: adornerRightClick,
            rightAdornerSelectedAction: adornerRightSelectedClick,
            rightAdornerCallback: rightAdornerCallback,
            rightAdornerSelectedCallback: rightAdornerSelectedCallback,
            notFoundCallback: notFoundCallback);

        info.Box.SelectedItem = Combo.SelectedItem;
        
        info.OnSuggestionChosen += (_, model) => Combo.SelectedItem = model;
        
        Content = new StackPanel();
        Content = stackPanel;

        return info;
    }
}