using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Controls;
using System.Windows.Data;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Requests;
using SmartErpWin.Core;
using SmartErpWin.Core.Exceptions;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.Helpers;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using DataGrid = System.Windows.Controls.DataGrid;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public abstract partial class BasePageViewModel<T> : BaseViewModel<T>,
                                                     INavigationAware
                                                     where T : class, new()
{

    #region  Fields

    [ObservableProperty]
    private SessionService? _session;

    [ObservableProperty]
    private ObservableCollection<T> _models = new();

    [ObservableProperty]
    private ObservableCollection<T> _pageModels = new();

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int _maxNumberOfPages = 1;

    [ObservableProperty]
    private Visibility _addVisibility;

    [ObservableProperty]
    private string _pagesText = "1/1";

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private StackPanel? _dataGridPanel;

    private double ItemsPerPage { get; init; } = Constants.ItemsPerPage;

    protected delegate void OnPagesChangedHandler();

    protected event OnPagesChangedHandler? OnPagesChanged;

    protected EventHandler? OnItemEdit;

    protected EventHandler? OnItemDelete;

    protected virtual List<Tuple<string, EventHandler>> ExtraButtons { get; } = [];

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private bool _actionsEnabled;

    [ObservableProperty]
    private int _limit = 10;

    [ObservableProperty]
    private Visibility _dataVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _ringVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _pagingVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility _printVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private double _minDataGridHeight;

    /// <inheritdoc/>
    protected BasePageViewModel(HttpClient client) : base(client)
    {
        Session = App.GetService<SessionService>();

        JudgeVisibilityPermissions();
    }

    private async void JudgeVisibilityPermissions()
    {
        //PrintVisibility = (await Session.JudgeAccessAsync(Endpoint, PrintPermission)).ApplyVisibility();
        PrintVisibility = false.ApplyVisibility();
        AddVisibility = (await Session.JudgeAccessAsync(Endpoint, CreatePermission)).ApplyVisibility();
    }

    protected virtual double MaxDataGridHeight => 800;

    protected virtual bool CanSelectInDataGrid => false;

    public event EventHandler<T>? OnDataGridSelectionChanged;

    protected virtual bool ShowError => true;

    #endregion

    #region Data

    protected virtual async Task RefreshAsync(bool refreshCombos = true, IEnumerable<T>? data = null)
    {
        if (data is not null)
        {
            SetData(data);
            RefreshDataGrid();
            SetState(false);
            return;
        }

        await RefreshAsync(true);
    }

    protected virtual async Task RefreshAsync(bool refreshCombos = true)
    {
        SetState(true);
        Models.Clear();
        try
        {
            var request = await GetAllByQueryAsync(query: new QueryRequestBase
            {
                SearchText = SearchText,
                Page = CurrentPage,
                Liimit = Limit
            });

            var models = request.Data;

            if (models is null)
            {
                // If this ever happens, it'll most likely be an issue on the API's end
                throw new NullReferenceException($"INTERNAL ERROR: There are no data associated with " +
                                                 $"{typeof(T).Name.Replace("Dto", string.Empty).ToLower()}s");
            }

            MaxNumberOfPages = Math.Clamp(request.Total, 1, int.MaxValue);
            CurrentPage = request.Page;

            SetData(ApplyModifications(models));
            RefreshDataGrid();
        }
        // Unauthorized response should be handled by the global domain dispatcher
        // for automatic logouts
        catch (Exception exception) when (exception is not LoggedException
        {
            SourceException: HttpRequestException
            {
                StatusCode: HttpStatusCode.Unauthorized
            }
        } && ShowError)
        {
            ShowErrorResult(exception);
        }
        finally
        {
            SetState(false);
        }
    }

    protected virtual List<T> ApplyModifications(IEnumerable<T> models)
    {
        return models.ToList();
    }
    
    [RelayCommand]
    protected async Task RefreshAsync()
    {
        await RefreshAsync(true);
    }

    [RelayCommand]
    private async Task PrintAsync()
    {
        //  TODO : Permission check
        var print = await Session.JudgeAccessAsync(Endpoint, "Print");

        if (!print)
        {
            DialogueServices.ShowContentDialogue("NoPermissions", "YouCannotPrintThisPage");
            return;
        }

        ExportServices.PrintToDialogue(DataGridServices.FindDataGrid(DataGridPanel));
    }

    protected void SetData(IEnumerable<T>? models)
    {
        Models.Clear();

        foreach (var model in models ?? [])
            Models.Add(model);

        var pageModels = GetItemsForCurrentPage();

        SetPageModels(pageModels);
    }

    public virtual void ShowData(StackPanel panel)
    {
        DataGridPanel = panel;
        /*var localResources = new ResourceDictionary();

        localResources.MergedDictionaries.Add(new ThemeResources());
        localResources.MergedDictionaries.Add(new XamlControlsResources());

        DataGridPanel.Resources = localResources;*/
        // Disabling this makes it so that only navigating to the page
        // would refresh the logic. This may fix duplicate refresh requests.
        //await RefreshAsync();
    }

    protected virtual async void RefreshDataGrid()
    {
        if (DataGridPanel is null)
            return;

        DataGridPanel.Children.Clear();

        /*if (PageModels.Count < 1)
        {
            var block = DataGridServices.GetEmptyItemsControl(MinDataGridHeight);
            DataGridPanel.Children.Add(block);
            
            return;
        }*/

        var (edit, delete) = await JudgePermissionsAsync();

        var dataGrid = await DataGridServices
            .CreateDataGridForDtoModelAsync(LocaleService,
                PageModels.ToList(),
                edit,
                delete,
                ExtraButtons);

        dataGrid.MaxHeight = MaxDataGridHeight;

        if (CanSelectInDataGrid)
        {
            dataGrid.SelectionMode = DataGridSelectionMode.Single;
            dataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;

            var style = new Style(typeof(DataGridRow));
            var trigger = new Trigger
            {
                Property = DataGridRow.IsSelectedProperty,
                Value = true
            };
            trigger.Setters.Add(new Setter(Control.BackgroundProperty,
                System.Windows.Media.Brushes.LightSlateGray));
            style.Triggers.Add(trigger);
            dataGrid.RowStyle = style;

            dataGrid.SelectionChanged += (_, _) =>
            {
                if (dataGrid is { SelectedItem: T item })
                {
                    OnDataGridSelectionChanged?.Invoke(this, item);
                }
            };
        }

        var minHeightBinding = new Binding(nameof(MinDataGridHeight))
        {
            Source = this,
            Mode = BindingMode.TwoWay
        };

        dataGrid.SetBinding(FrameworkElement.MinHeightProperty, minHeightBinding);

        DataGridPanel.Children.Add(new Card { Content = dataGrid, Padding = new Thickness(2) });
    }

    public virtual Func<T, T, bool>? Finder => null;

    protected void ScrollTo(T obj)
    {
        var dataGrid = DataGridPanel.Children[0] as DataGrid;
        if (dataGrid is null || Finder is null) return;
        var __obj = PageModels.FirstOrDefault(x => Finder(obj, x));
        if (__obj is null) return;
        var __index = PageModels.IndexOf(__obj);
        var row = GetRow(dataGrid, __index);
        //dataGrid.ScrollIntoView(__obj);
        //dataGrid.SelectedItem = null;
        //dataGrid.ScrollIntoView(dataGrid.Items[__index]);
        //dataGrid.SelectedItem = __obj;
        //dataGrid.Items[__index].
    }

    protected void ScrollTo(int index)
    {
        var dataGrid = DataGridPanel.Children[0] as DataGrid;
        if (dataGrid is null) return;
        var __obj = PageModels[index];
        //dataGrid.ScrollIntoView(__obj);
        var row = GetRow(dataGrid, index);
    }

    public DataGridRow GetRow(DataGrid dataGrid, int index)
    {
        var row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
        if (row == null)
        {
            dataGrid.ScrollIntoView(dataGrid.Items[index]);
            row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
        }
        return row;
    }

    protected void ShowErrorResult(Exception x)
    {
        DataGridPanel?.Children.Clear();
        DataGridPanel?.Children.Add(DataGridServices
            .GetErrorItemsControl(LocaleService["AnErrorHasOccurred"],
                x.Message,
                LocaleService, () =>
            {
                _ = RefreshAsync();
            }));
    }

    protected void ShowResult(string title, string text)
    {
        DataGridPanel?.Children.Clear();
        DataGridPanel?.Children.Add(DataGridServices
            .GetErrorItemsControl(title, text, LocaleService, null));
    }

    protected void SelectInDataGrid(T type)
    {
        var dataGrid = DataGridServices.FindDataGrid(DataGridPanel);
        SelectInSpecificDataGrid(dataGrid, type);
    }

    protected void SelectInSpecificDataGrid<TSpecific>(DataGrid dataGrid, TSpecific type)
    {
        if (type is not IModel mType)
            return;

        var index = -1;

        for (var i = 0; i < dataGrid.Items.Count; i++)
        {
            // For some reason, IndexOf doesn't ref equal TemporaryId, so let's
            // do that ourselves. Maybe because Identifier is a struct.

            if (dataGrid.Items[i] is not IModel model)
                continue;

            if (model.TemporaryId == mType.TemporaryId)
                index = i;
        }

        if (index < 0)
            return;

        // The DataGrid doesn't seem to update the visual layout after adding the model instantly?

        dataGrid.ScrollIntoView(type);
        dataGrid.UpdateLayout();

        var row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);

        if (row != null)
        {
            row.IsSelected = true;
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(Limit))
        {
            // reset paging data so that the user is not stuck with a blank screen 
            // when there's no data in a page that no longer exists

            CurrentPage = 1;
            MaxNumberOfPages = 1;

            _ = RefreshAsync();
        }

        base.OnPropertyChanged(e);
    }

    protected void SetState(bool isRefreshing)
    {
        IsRefreshing = isRefreshing;
        ActionsEnabled = !isRefreshing;

        DataVisibility = IsRefreshing ? Visibility.Collapsed : Visibility.Visible;
        RingVisibility = IsRefreshing ? Visibility.Visible : Visibility.Collapsed;
        PagingVisibility = Models.Any() ? Visibility.Visible : Visibility.Collapsed;
    }

    protected virtual async void OnDelete(object? sender, EventArgs args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
            return;

        var model = DataGridServices.FindModel<T>(sender);

        if (model is not DtoBase modelBase)
            return;

        await DeleteAsync(modelBase.Id);
    }

    private async Task DeleteAsync(Guid objectId)
    {
        SetState(true);

        await DeleteRemoveAsync(objectId, RefreshAsync);

        await RefreshAsync();
    }

    #endregion

    #region Paging

    private int GetNumberOfPages()
    {
        return Models.Count / (int)(ItemsPerPage + 1) + 1;
    }

    private ObservableCollection<T> GetItemsForPage(int page)
    {
        return Models;
    }

    private ObservableCollection<T> GetItemsForCurrentPage()
        => GetItemsForPage(CurrentPage);

    private async void MoveAndSetItems(MoveType moveType)
    {
        var validPage = ValidatePage(moveType == MoveType.Up
            ? CurrentPage + 1 : CurrentPage - 1);

        if (validPage == CurrentPage)
            return;

        CurrentPage = validPage;

        await RefreshAsync(false);
    }

    private int ValidatePage(int page)
    {
        if (page is 0)
            return 1;

        var numOfPages = MaxNumberOfPages;

        if (page > numOfPages)
            page = numOfPages;

        return page;
    }

    [RelayCommand]
    private void MoveOneUp()
    {
        MoveAndSetItems(MoveType.Up);
    }

    [RelayCommand]
    private void MoveOneDown()
    {
        MoveAndSetItems(MoveType.Down);
    }

    private void SetPageModels(IEnumerable<T>? models)
    {
        if (models is null)
            return;

        PageModels.Clear();

        foreach (var model in models)
        {
            PageModels.Add(model);
        }

        PagesText = $"{CurrentPage}/{MaxNumberOfPages}";
    }

    #endregion

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        await base.OnSynchronisationRequestedAsync();

        await RefreshAsync();
    }
    
    private async Task<(EventHandler? edit, EventHandler? delete)> JudgePermissionsAsync()
    {
        var edit = await Session.JudgeAccessAsync(Endpoint, UpdatePermission);
        var delete = await Session.JudgeAccessAsync(Endpoint, DeletePermission);

        return (edit ? OnItemEdit : null, delete ? OnItemDelete : null);
    }

    public async virtual Task OnNavigatedToAsync()
    {
        Verbose("Navigated to {0}", GetType().Name);
        await OnSynchronisationRequestedAsync();
    }

    public virtual Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}