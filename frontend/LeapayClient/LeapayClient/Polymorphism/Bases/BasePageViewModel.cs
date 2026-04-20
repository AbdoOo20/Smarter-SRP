using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.PurchaseAndSales.Enums;
using Leapay.Client.ClientModels.Dto;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Controls;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Features;
using Leapay.Client.Helpers;
using Leapay.Client.Options;
using Leapay.Client.Polymorphism.Interfaces;
using Leapay.Client.Services;
using Leapay.Client.UserInterface.Common.ImportDialogue;
using Material.Icons.Avalonia;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Exceptions;
using SukiUI.Controls;
using SukiUI.Toasts;

namespace Leapay.Client.Polymorphism.Bases;

public abstract partial class BasePageViewModel<T> : BaseViewModel<T>, INavigationPage
                                                     where T : class, new()
{
    #region  Fields
    
    [ObservableProperty]
    public partial ObservableCollection<T> Models { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<T> PageModels { get; set; } = [];

    [ObservableProperty]
    public partial int CurrentPage { get; set; } = 1;

    public virtual bool ShouldRenderFullPage { get; set; } = true;
    
    [ObservableProperty]
    public partial int MaxNumberOfPages { get; set; } = 1;

    [ObservableProperty]
    public partial bool AddVisibility { get; set; }
    
    [ObservableProperty]
    public partial string PagesText { get; set; } = "1/";

    [ObservableProperty]
    public partial string ImportText { get; set; } = "Command";
    
    [ObservableProperty]
    public partial string ExtraText { get; set; } = "Command";
    
    [ObservableProperty]
    public partial MaterialIconKind ExtraIcon { get; set; }
    
    [ObservableProperty]
    public partial MaterialIconKind ImportIcon { get; set; }
    
    [ObservableProperty]
    public partial string PrintText { get; set; } = "Print";
    
    [ObservableProperty]
    public partial bool IsFavourited { get; set; }

    [ObservableProperty]
    public partial bool PrimaryPage { get; set; } = true;
    
    [ObservableProperty]
    public partial bool ShowExportButton { get; set; } = true;
    
    [ObservableProperty]
    public partial bool ExtraButtonVisibility { get; set; } = false;
    
    [ObservableProperty]
    public partial bool ImportButtonVisibility { get; set; } = false;
    
    [ObservableProperty]
    public partial string? SearchText { get; set; }

    [ObservableProperty]
    public partial StackPanel? DataGridPanel { get; set; }
    
    public Func<TableImportModel<T>> ImportModel { get; set; }
    
    private double ItemsPerPage { get; init; } = Constants.ItemsPerPage;

    protected delegate void OnPagesChangedHandler();

    protected event OnPagesChangedHandler? OnPagesChanged;

    public EventHandler<DataGridButtonEventArgs<T>>? OnItemEdit;

    public EventHandler<DataGridButtonEventArgs<T>>? OnItemDelete;

    protected string? _pageName;
    private string? _pageNameEng;
    
    protected virtual List<Tuple<object, EventHandler<DataGridButtonEventArgs<T>>>> ExtraButtons { get; } = [];

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }
    
    [ObservableProperty]
    public partial bool ActionsEnabled { get; set; }

    [ObservableProperty]
    public partial int Limit { get; set; } = 15;
    
    [ObservableProperty]
    public partial int[] Limits { get; set; } = [5, 10, 15, 20, 30, 50, 100, 200, 300, 500, 1000, 2000, 3000, 5000, 10000, 50000, 100000, 1000000, 10000000];
    
    [ObservableProperty]
    public partial bool DataVisibility { get; set; }

    [ObservableProperty]
    public partial bool RingVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool PagingVisibility { get; set; }
    
    [ObservableProperty]
    public partial bool PrintVisibility { get; set; }

    protected virtual double DataGridHeight => PageDataGridDefaultHeight;

    protected virtual bool IsEntryPage => false;
    
    protected virtual bool IsCompact => false;

    public ISukiToastManager ToastManager { get; } = App.GetService<ISukiToastManager>();

    /// <inheritdoc/>
    protected BasePageViewModel()
    {
        Session = App.GetService<SessionService>();
        
        if(!PrimaryPage)
            As<ISynchronisationAware>().Unregister(this);
    }

    private async Task JudgeVisibilityPermissions()
    {
        //PrintVisibility = (await Session.JudgeAccessAsync(Endpoint, PrintPermission)).ApplyVisibility();
        AddVisibility = await Session.JudgeAccessAsync(Endpoint, CreatePermission);
    }

    protected void SetEndpoint(string endpoint)
    {
        Endpoint = endpoint;
    }

    protected void RegisterForImport(Func<TableImportModel<T>> importModel)
    {
        if (!Session.JudgeSpecificAccess($"{Endpoint}.Import"))
            return;

        ImportModel = importModel;
        ImportButtonVisibility = true;
        ImportText = this["Import"];
        ImportIcon = MaterialIconKind.ImportExport;
    }

    private async void StartImport()
    {
        var entry = new ImportDialogueWindow
        {
            ViewModel = ImportModel()
        };
        
        await ShowContentAsync(entry, options: new(this["Import"] + " " + _pageName,
            BlockUntilContentCloses: true));

        (entry.ViewModel as IDisposable)?.Dispose();
        entry.ViewModel = null;
    }
    
    [RelayCommand]
    protected virtual Task OnAddAsync()
    {
        return Task.CompletedTask;
    }

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
        if (string.IsNullOrEmpty(Endpoint))
        {
            // Please note that this check not be applied everywhere.
            // It is applied here because RefreshAsync is called automatically on init, and GetAllByQueryAsync
            // does not accept a custom endpoint, so Endpoint must exist otherwise 404 is returned.
            // Some calls may provide an inner endpoint without the parent Endpoint.
            // These calls should not be suppressed with this check.
            return;
        }
        SetState(true);

        await JudgeVisibilityPermissions();
        
        Models.Clear();
        PageModels.Clear();
        RefreshDataGrid();

        HttpResponseWrapperList<T>? request = null;

        await Task.Run(async () =>
        {
            // Add an artificial delay before loading up the data grid,
            // so that the user can navigate to the page without 
            // any UI hiccups.
            try
            {
                await Task.Delay(300);
                
                request = await GetAllByQueryAsync(query: new QueryRequestBase
                {
                    Set = 1,
                    SearchText = SearchText,
                    Page = CurrentPage,
                    Liimit = Limit
                });
            }
            // Unauthorized response should be handled by the global domain dispatcher
            // for automatic logouts in case of an expired auth token
            catch (Exception exception) when (exception is not LoggedException
                                              {
                                                  SourceException: HttpRequestException
                                                  {
                                                      StatusCode: HttpStatusCode.Unauthorized
                                                  }
                                              })
            {
                Program.TryForceSignOut(exception);

                Dispatcher.UIThread.Post(() =>
                {
                    ShowErrorResult(exception);
                    SetState(false);
                });
            }
        }).ContinueWith(_ =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (request is null)
                    return;

                var models = request.Data;

                if (models is null)
                {
                    throw new NullReferenceException($"INTERNAL ERROR: There are no data associated with " +
                                                     $"{typeof(T).Name.Replace("Dto", string.Empty).ToLower()}s");
                }

                MaxNumberOfPages = Math.Clamp(request.Total, 1, int.MaxValue);
                CurrentPage = request.Page;

                SetData(models);
                RefreshDataGrid();
                SetState(false);
            });
        });
    }

    [RelayCommand]
    protected async Task RefreshAsync()
    {
        await RefreshAsync(true);
    }

    [RelayCommand]
    protected virtual async Task PrintAsync()
    {
        var print = await Session.JudgeAccessAsync(Endpoint, "Print");

        if (!print)
        {
            //DialogueServices.ShowContentDialogue("NoPermissions", "YouCannotPrintThisPage");
            return;
        }

        //ExportServices.PrintToDialogue(DataGridServices.FindDataGrid(DataGridPanel));
    }

    private ComboModel? GetDefault(List<ComboModel> models, Func<string> str)
        => models.FirstOrDefault(m => m.Id.ToString() == str());

    protected Identifier GetDefaultStorage(List<ComboModel>? models, Identifier existing, EntryType type)
    {
        var def = GetDefault(models ?? [], () => Session.UserInfo.DefaultStorageId);

        if (type is EntryType.Edit || !existing.IsNullOrEmpty())
            return existing;

        if (def is null)
            return ComboModelBase.GetDefaultComboModel().Id;

        return def.Id;
    }

    protected Identifier GetDefaultBank(List<ComboModel>? models, Identifier existing, EntryType type)
    {
        var def = GetDefault(models ?? [], () => Session.UserInfo.DefaultBankId);

        if (type is EntryType.Edit || !existing.IsNullOrEmpty())
            return existing;

        if (def is null)
            return ComboModelBase.GetDefaultComboModel().Id;

        return def.Id;
    }
    
    protected Identifier GetDefaultSafe(List<ComboModel>? models, Identifier existing, EntryType type)
    {
        var def = GetDefault(models ?? [], () => Session.UserInfo.DefaultSafeId);

        if (type is EntryType.Edit || !existing.IsNullOrEmpty())
            return existing;

        if (def is null)
            return ComboModelBase.GetDefaultComboModel().Id;
        
        return def.Id;
    }
    
    protected List<ComboModel> GetVisibleStorages(List<ComboModel>? models)
        => (models ?? []).Where(s => Session.Verify(Session.UserInfo.VisibleStorages.Contains(s.Value))).ToList();
    
    protected List<ComboModel> GetVisibleSafes(List<ComboModel>? models)
        => (models ?? []).Where(s => Session.Verify(Session.UserInfo.VisibleSafes.Contains(s.Value))).ToList();
    
    protected List<ComboModel> GetVisibleBanks(List<ComboModel>? models)
        => (models ?? []).Where(s => Session.Verify(Session.UserInfo.VisibleBanks.Contains(s.Value))).ToList();
    
    protected PeymentType GetPaymentType(PeymentType existing, EntryType type)
    {
        if (type is EntryType.Edit)
            return existing;
        
        return Session.UserInfo.DefaultPaymentType;
    }
    
    protected StorageType GetStorageType(StorageType existing, EntryType type)
    {
        if (type is  EntryType.Edit)
            return existing;
        
        return Session.UserInfo.DefaultStorageType;
    }

    public void SetData(IEnumerable<T>? models)
    {
        Models.Clear();

        foreach (var model in models ?? [])
            Models.Add(model);

        var pageModels = GetItemsForCurrentPage();

        SetPageModels(pageModels);
    }

    private readonly TaskCompletionSource<bool> _dataGridIsSet = new();
    private TaskCompletionSource<bool> _refreshTaskSource = new();
    
    private bool hasAlreadySetFavourite;

    public virtual async void ShowData(StackPanel panel)
    {
        DataGridPanel = panel;
        
        _dataGridIsSet.TrySetResult(true);
    }

    public async void PublicRefreshDataGrid() => RefreshDataGrid();
    
    protected virtual async void RefreshDataGrid()
    {
        await _dataGridIsSet.Task;
        
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
                ExtraButtons,
                backBrush: BackgroundTheme,
                isCompact: IsCompact);

        dataGrid.MaxHeight = DataGridHeight;
        //dataGrid.SelectionMode = DataGridSelectionMode.Single;
        
        /*if (CanSelectInDataGrid)
        {
            dataGrid.SelectionMode = DataGridSelectionMode.Single;

            dataGrid.SelectionChanged += (_, _) =>
            {
                if (dataGrid.SelectedItem is T item)
                {
                    OnDataGridSelectionChanged?.Invoke(this, item);
                }
            };
        }*/
        
        var minHeightBinding = new Binding(nameof(DataGridHeight))
        {
            Source = this,
            Mode = BindingMode.TwoWay
        };
        
        dataGrid.Bind(Layoutable.MinHeightProperty, minHeightBinding);
        if (ShouldRenderFullPage)
        {
            RenderFullPage(dataGrid);
        }
        else
        {
            DataGridPanel.Children.Add(new GlassCard { Content = dataGrid,
                Padding = new Thickness(2),
                MaxHeight = DataGridHeight,
                Height = DataGridHeight
            });
        }
        
        dataGrid.UpdateLayout();
    }

    public virtual Func<T, T, bool>? Finder => null;

    protected void ScrollTo(int index)
    {

    }

    public static DataGridRow? GetRow(DataGrid dataGrid, object item)
        => dataGrid.GetVisualDescendants().OfType<DataGridRow>()
            .FirstOrDefault(row => Equals(row.DataContext, item));

    protected void ShowErrorResult(Exception x)
    {
        if (CanSelectInDataGrid)
            return;
        
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

    private BindingDataGrid? DataGrid => DataGridServices.FindDataGrid(DataGridPanel);
    
    public void SelectInDataGrid(T type)
    {
        var dataGrid = DataGridServices.FindDataGrid(DataGridPanel);
        SelectInSpecificDataGrid(dataGrid, type);
    }

    public void ScrollToItem(T type)
    {
        var dataGrid = DataGridServices.FindDataGrid(DataGridPanel);
        ScrollInSpecificDataGrid(dataGrid, type);
    }
    
    protected void SelectInSpecificDataGrid<TSpecific>(BindingDataGrid? dataGrid, TSpecific type)
    {
        dataGrid?.HighlightItem(type as DtoBase);
    }

    protected void ScrollInSpecificDataGrid<TSpecific>(BindingDataGrid? dataGrid, TSpecific type)
    {
        dataGrid?.ScrollToItem(type as DtoBase);
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

        DataVisibility = !IsRefreshing;
        RingVisibility = IsRefreshing;
        PagingVisibility = Models.Any();
        
        if (isRefreshing)
        {
            _refreshTaskSource = new();
        }
        else
        {
            _refreshTaskSource.TrySetResult(true);
        }
    }

    protected virtual async void OnDelete(object? sender, DataGridButtonEventArgs<T> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;
        
        if (args.Model is not DtoBase modelBase)
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

        PagesText = $"{MaxNumberOfPages}/";
    }

    #endregion

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        await base.OnSynchronisationRequestedAsync();

        if (args.ElementAtOrDefault(0) is string pageTitle)
        {
            _pageName = this[pageTitle];
            _pageNameEng = pageTitle;
            IsFavourited = ContentHelper.IsFavourited(pageTitle);
            hasAlreadySetFavourite = true;
        }

        if (args.FirstOrDefault() is CodeBook.AddNew)
        {
            await _refreshTaskSource.Task;
            await AddCommand.ExecuteAsync(null);
            return;
        }

        if (args.FirstOrDefault() is CodeBook.IsNew)
        {
            SearchText = string.Empty;
            Limit = Limits.LastOrDefault();
        }

        if (args.FirstOrDefault() is not CodeBook.Missing && args.FirstOrDefault() is not CodeBook.Available)
        {
            await RefreshAsync(args.FirstOrDefault() is CodeBook.ResState);
        }
    }
    
    private async Task<(EventHandler<DataGridButtonEventArgs<T>>? edit, EventHandler<DataGridButtonEventArgs<T>>? delete)> JudgePermissionsAsync()
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

    protected virtual void OnEdit(object? sender, DataGridButtonEventArgs<T> args)
    {
        
    }
    
    public virtual Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }

    public void SetProperties(string displayName, MaterialIconKind icon)
    {
        DisplayName = this[DisplayName];
        Icon = new MaterialIcon
        {
            Kind = icon
        };
    }
    
    protected async Task ShowContentAsync(UserControlBase content,
        EntryType type = EntryType.Add,
        string? name = null,
        WindowOptions? options = null)
    {
        await ContentHelper.ShowCrossPlatformContentAsync(content,
            options ?? new(name is null ? GetPageName(GetType(), type) : this[name]));
    }
    
    [ObservableProperty]
    public partial string DisplayName { get; set; }
    
    [ObservableProperty]
    public partial MaterialIcon Icon { get; set; }
    
    [ObservableProperty]
    public partial int Index { get; set; }

    public async void RenderFullPage(BindingDataGrid dataGrid)
    {
        var root = new DockPanel();

        // ----- Top Toolbar -----
        var topBar = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        
        DockPanel.SetDock(topBar, Avalonia.Controls.Dock.Top);
        
        var leftToolbar = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        var searchBox = new TextBox
        {
            MinWidth = 400,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(10, 0, 0, 0),
            Watermark = this["Search"]
        };

        var searchIcon = new Border
        {
            ClipToBounds = true,
            CornerRadius = new CornerRadius(10),
            Child = new GlassCard
            {
                Width = 40,
                Height = 40,
                Content = new MaterialIcon
                {
                    Width = 20,
                    Height = 20,
                    Kind = MaterialIconKind.Search,
                    Foreground = BackgroundTheme
                }
            }
        };
        
        searchBox.Bind(TextBox.WatermarkProperty, new Binding(VmBind("SearchBar")));
        searchBox.Bind(TextBox.TextProperty, new Binding(VmBind("SearchText")) { Mode = BindingMode.TwoWay });
            
        var searchButton = new Button
        {
            Height = 40,
            MinWidth = 80,
            Margin = new Thickness(25, 0, 0, 0),
            Content = BuildButtonIcon("RefreshButton", MaterialIconKind.Update)
        };
        
        searchButton.Bind(Button.CommandProperty, new Binding(VmBind("RefreshCommand")));
        searchBox.KeyDown += (_, k) =>
        {
            if (k.Key is Key.Enter)
                searchButton.Command?.Execute(null);
        };
        
        leftToolbar.Children.Add(searchIcon);
        leftToolbar.Children.Add(searchBox);
        leftToolbar.Children.Add(searchButton);

        var rightToolbar = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right
        };

        var actionsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        
        actionsPanel.Bind(InputElement.IsEnabledProperty, new Binding(VmBind("ActionsEnabled")));

        var extraButton = new Button
        {
            Margin = new Thickness(10, 0, 0, 0),
            Height = 40,
            MinWidth = 80,
            Content = ExtraIcon is 0 ? ExtraText : BuildButtonIcon(ExtraText, ExtraIcon)
        };

        var importButton = new Button
        {
            Margin = new Thickness(10, 0, 0, 0),
            Height = 40,
            MinWidth = 80,
            Content = ImportIcon is 0 ? ExtraText : BuildButtonIcon(ImportText, ImportIcon),
            Command = new RelayCommand(StartImport)
        };
        
        importButton.Bind(Visual.IsVisibleProperty, new Binding(VmBind("ImportButtonVisibility")));

        extraButton.Bind(Visual.IsVisibleProperty, new Binding(VmBind("ExtraButtonVisibility")));
        extraButton.Bind(Button.CommandProperty, new Binding(VmBind("ExtraButtonCommand")));
        
        var exportButton = new Button
        {
            Margin = new Thickness(10, 0, 0, 0),
            Height = 40,
            MinWidth = 80,
            Content = BuildButtonIcon("Export", MaterialIconKind.MicrosoftExcel)
        };

        ShowExportButton = ShowExportButton && OperatingSystem.IsWindows() && await Session.JudgeAccessAsync(Endpoint, ExportPermission);
        exportButton.Bind(Visual.IsVisibleProperty, new Binding(VmBind("ShowExportButton")));
        exportButton.Bind(Button.CommandProperty, new Binding(VmBind("ExportButtonCommand")));

        var printButton = new Button
        {
            Margin = new Thickness(10, 0, 0, 0),
            MinWidth = 80,
            Height = 40,
            Content = BuildButtonIcon(PrintText, MaterialIconKind.Printer3d)
        };

        printButton.Bind(Visual.IsVisibleProperty, new Binding(VmBind("PrintVisibility")));
        printButton.Bind(Button.CommandProperty, new Binding(VmBind("PrintCommand")));

        var addButton = new Button
        {
            Margin = new Thickness(10, 0, 0, 0),
            MinWidth = 80,
            Height = 40,
            Content = BuildButtonIcon("NewButton", MaterialIconKind.Plus),
            Background = ContentHelper.GetGreenGradient()
        };

        //addButton.Bind(Button.ContentProperty, new Binding(VmBind("NewButton")));
        addButton.Bind(Visual.IsVisibleProperty, new Binding(VmBind("AddVisibility")));
        addButton.Bind(Button.CommandProperty, new Binding(VmBind("AddCommand")));

        actionsPanel.Children.AddRange([exportButton, importButton, printButton, extraButton, addButton]);
        rightToolbar.Children.Add(actionsPanel);

        DockPanel.SetDock(leftToolbar, Avalonia.Controls.Dock.Left);
        DockPanel.SetDock(rightToolbar, Avalonia.Controls.Dock.Right);

        /*dockPanelToolbar.Children.Add(leftToolbar);
        dockPanelToolbar.Children.Add(rightToolbar);*/
        
        var grid = new Grid
        {
            ColumnDefinitions =
            [
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            ]
        };
        
        grid.Children.AddRange([leftToolbar, rightToolbar]);
        Grid.SetColumn(leftToolbar, 0);
        Grid.SetColumn(rightToolbar, 2);
        //var gridCard = new GlassCard { Content = grid };

        /*topBar.Children.Add(dockPanelToolbar);
        topBar.MinWidth = (DataGridPanel?.Parent as Visual)?.Bounds.Width ?? 400;*/

        var upperStack = new StackPanel();
        var dock = new DockPanel();
        
        /*var rightControl = GetStarButton();
        DockPanel.SetDock(rightControl, Avalonia.Controls.Dock.Left);
        dock.Children.Add(rightControl);*/
        
        /*var centerControl = new TextBlock
        {
            RenderTransform = new TranslateTransform { X = -20 },
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10, 0, 0, 0),
            Text = _pageName,
            FontSize = 24,
            FontWeight = FontWeight.Bold
        };
        dock.Children.Add(centerControl);
        
        if (_pageName is not null)
        {
            upperStack.Children.Add(new GlassCard
            {
                Padding = new(6),
                Margin = new Thickness(0, 0, 0, 20),
                MaxWidth = 400,
                Content = dock
            });
        }*/
        
        upperStack.Children.Add(grid);
        DockPanel.SetDock(upperStack, Avalonia.Controls.Dock.Top);

        root.Children.Add(upperStack);

        // ----- Loading Spinner -----
        var loadingSpinner = new Loading
        {
            Width = 65,
            Height = 65,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 150, 0, 0)
        };
        
        DockPanel.SetDock(loadingSpinner, Avalonia.Controls.Dock.Top);
        loadingSpinner.Bind(Visual.IsVisibleProperty, new Binding(VmBind("IsRefreshing")));

        root.Children.Add(loadingSpinner);

        // ----- Data View -----
        var dataStack = new StackPanel
        {
            Margin = new Thickness(0, 25, 0, 0)
        };
        dataStack.Bind(Visual.IsVisibleProperty, new Binding(VmBind("DataVisibility")));

        var scrollViewer = new ScrollViewer
        {
            Padding = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        };

        scrollViewer.Content = new GlassCard
        {
            Content = dataGrid,
            MaxHeight = DataGridHeight,
            Height = DataGridHeight
        };

        dataStack.Children.Add(scrollViewer);

        // ----- Pagination -----
        var pagerPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 15, 0, 0)
        };
        DockPanel.SetDock(pagerPanel, Avalonia.Controls.Dock.Bottom);

        var limitComboBox = new ComboBox { Margin = new Thickness(0, 0, 15, 0) };
        limitComboBox.Bind(ItemsControl.ItemsSourceProperty, new Binding(VmBind("Limits")));
        limitComboBox.Bind(SelectingItemsControl.SelectedItemProperty, new Binding(VmBind("Limit")) { Mode = BindingMode.TwoWay });

        var prevButton = new Button
        {
            Content = BuildButtonIcon("PreviousButton", GetArrow(false))
        };
        
        prevButton.Bind(Button.CommandProperty, new Binding(VmBind("MoveOneDownCommand")));

        var pagesEntry = new TextBox
        {
            Margin = new Thickness(0, 0, 10, 0),
            MaxHeight = 30,
            FontSize = 16,
            MinWidth = 50,
            Text = CurrentPage.ToString()
        };

        pagesEntry.LostFocus += (sender, args) =>
        {
            HandlePageEntrySync();
        };

        pagesEntry.KeyDown += (sender, args) =>
        {
            if (args.Key is Key.Enter)
            {
                HandlePageEntrySync();
            }
        };

        void HandlePageEntrySync()
        {
            if (int.TryParse(pagesEntry.Text, out var pageNumber))
            {
                pageNumber = Math.Clamp(pageNumber, 1, MaxNumberOfPages);

                if (CurrentPage == pageNumber)
                    return;
                
                CurrentPage = pageNumber;
                
                _ = RefreshAsync(false);
            }
            else
            {
                pagesEntry.Text = CurrentPage.ToString();
            }
        }
        
        var pagesText = new TextBlock
        {
            Margin = new Thickness(10, 12, 3, 0),
            FontSize = 16
        };
        pagesText.Bind(TextBlock.TextProperty, new Binding(VmBind("PagesText")));

        var nextButton = new Button
        {
            Content = BuildButtonIcon("NextButton", GetArrow(true))
        };
        
        nextButton.Bind(Button.CommandProperty, new Binding(VmBind("MoveOneUpCommand")));

        ContentHelper.AddClassesTo("Flat Rounded", prevButton, nextButton, extraButton,
            printButton, addButton, searchButton, exportButton, importButton);
        pagerPanel.Children.AddRange([limitComboBox, prevButton, pagesText, pagesEntry, nextButton]);

        dataStack.Children.Add(pagerPanel);
        
        _ = AnimationHelper.AnimatePopUpAsync(scrollViewer);

        root.Children.Add(dataStack);

        DataGridPanel.Margin = new Thickness(25);
        DataGridPanel.Children.Add(root);
    }

    private MaterialIconKind GetArrow(bool forward)
    {
        return FlowDirection is FlowDirection.LeftToRight ?
            forward ? MaterialIconKind.ArrowForward : MaterialIconKind.ArrowBack :
            forward ? MaterialIconKind.ArrowBack : MaterialIconKind.ArrowForward;
    }

    private StackPanel BuildButtonIcon(string text, MaterialIconKind icon)
    {
        return new StackPanel
        {
            Spacing = 5,
            Orientation = Orientation.Horizontal,
            Children =
            {
                new MaterialIcon
                {
                    Kind = icon
                },
                new TextBlock
                {
                    Text = this[text]
                }
            }
        };
    }

    private string VmBind(string property) => $"ViewModel.{property}";

    [RelayCommand]
    protected virtual void OnExtraButton()
    {
        
    }
    
    [RelayCommand]
    protected virtual async void OnExportButton()
    {
        var dg = DataGridServices.FindDataGrid(DataGridPanel);

        if (dg is null)
        {
            Warning("DataGrid was null while trying to export.");
            return;
        }
        
        var provider = App.GetService<MainWindow>().StorageProvider;

        var doc = await provider.TryGetFolderFromPathAsync(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        var options = new FilePickerSaveOptions
        {
            SuggestedStartLocation = doc,
            SuggestedFileName = $"{_pageName}_{DateTime.Now:yyyy-MM-dd-HH-mm}.xlsx"
        };

        var picker = await provider.SaveFilePickerAsync(options);

        var readFile = picker?.TryGetLocalPath();
        if (readFile != null)
        {
            dg.ExportToExcel(readFile, _pageName);
        }
    }
    
    public async Task<T> GetByIdAsync(string id)
    {
        var model = await GetOneAsync(Guid.Parse(id));

        if (!model.Status || model.Data is null)
        {
            Warning("Could not fetch singular ID {0}", id);
            return null;
        }

        return model.Data;
    }
    
    [RelayCommand]
    public async Task<Identifier> OnEditReturnAsync(string id)
    {
        OnEdit(this, new DataGridButtonEventArgs<T>(await GetByIdAsync(id)));
        return Identifier.Parse(id);
    }

    public override void Dispose()
    {
        Models.Clear();
        PageModels.Clear();
        DataGrid?.Dispose();

        base.Dispose();
    }
}