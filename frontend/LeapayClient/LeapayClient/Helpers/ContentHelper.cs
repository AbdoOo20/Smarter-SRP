using AsyncImageLoader;
using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Threading;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Sell;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Options;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Leapay.Client.UI.ViewModels;
using Leapay.Client.UserInterface.Common;
using Leapay.Client.UserInterface.Common.Items;
using Leapay.Client.UserInterface.Common.Items.Models;
using Leapay.Client.UserInterface.Misc.Configuration;
using Leapay.Client.Wrappers;
using Leapay.Windows.Printing;
using Material.Icons.Avalonia;
using SkiaSharp;
using SmartErpWin.Core;
using SmartErpWin.Core.Enums;
using SmartErpWin.Core.Extensions;
using SmartErpWin.Core.Globals;
using SukiUI.Controls;

namespace Leapay.Client.Helpers;

public static class ContentHelper
{
    public static AvaloniaList<SukiWindow> OpenWindows { get; private set; } = [];

    private const double OperatingXRes = 1980;
    private const double OperatingYRes = 980;

    private static Dictionary<string, NavigationViewItemWrapper> _views = new();

    public static event EventHandler? MenuItemsLoaded;
    
    public static async Task<SukiWindow?> ShowCrossPlatformContentAsync(UserControlBase control, WindowOptions options)
    {
        /*var vm = App.GetService<TabbedNavigationalPage>();
        var tabbedViewCardModel = new TabbedViewCardModel
        {
            View = control,
            ViewName = control.GetType().Name,
            CardName = options.Title,
        };
        await vm.ViewModel.OnSynchronisationRequestedAsync(tabbedViewCardModel);

        if (control.SynchronisationModel is BaseEntryViewModel<DtoBase> @base)
        {
            await @base.WaitUntilUserSavesAsync();
            await vm.ViewModel.OnSynchronisationRequestedAsync(tabbedViewCardModel);
        }

        return null;*/
        
        // No support for other desktop platforms is required yet
        if (OperatingSystem.IsWindows() && App.TryGetDesktopLifetime(out var desktop))
        {
            var localeService = App.GetService<LocalisationService>();
            ApplyButtonBackgroundStyling(control, localeService);
            
            control.Margin = new Thickness(control.Margin.Left,
                control.Margin.Top,
                control.Margin.Right,
                30);

            var busyArea = new BusyArea
            {
                Content = new Grid
                {
                    Name = "MainGrid",
                    Children =
                    {
                        control
                    }
                }
            };

            busyArea.Bind(BusyArea.SecondaryBusyTextProperty, new Binding("ViewModel.SecondaryBusyText"));
            busyArea.Bind(BusyArea.CommandTextProperty, new Binding("ViewModel.CommandBusyText"));
            busyArea.Bind(BusyArea.CommandActionProperty, new Binding("ViewModel.CommandBusyAction"));
            busyArea.Bind(BusyArea.IsBusyProperty, new Binding("ViewModel.IsBusy"));
            busyArea.Bind(BusyArea.BusyTextProperty, new Binding("ViewModel.BusyText"));
            
            var window = new SukiWindow
            {
                FlowDirection = localeService.DetermineFlowDirection(),
                Content = new Viewbox
                {
                    Stretch = Stretch.Uniform,
                    Child = busyArea
                },
                Width = control.Width + 30,
                Height = control.Height + 20,
                Title = options.Title,
                CanMaximize = options.CanMaximize,
                CanMinimize = options.CanMinimize,
                CanResize = options.CanResize,
                WindowState = options.IsMaximized ? WindowState.Maximized : WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Hosts.Add(new SukiDialogHost());
            Diag.Assert(desktop.MainWindow is not null);
            
            window.Closed += (_, _) => { 
                OpenWindows.Remove(window);
                window.Dispose();
            };

            window.DataContext = control;
            window.CustomContent = control;
            window.BackgroundStyle = PreferencesService.Get(b => b.BackgroundStyle);
            
            OpenWindows.Add(window);

            if (options.IsMaximized)
                AddFavouritesToToolbar(window);

            if (control.SynchronisationModel is IViewModel vm)
            {
                vm.WindowLock = window;
            }
            
            window.Loaded += (_, _) =>
            {
                SetGridResolution(window, window.Content.As<Viewbox>().Child?.As<BusyArea>().Content?.As<Grid>());
                window.BackgroundAnimationEnabled = PreferencesService.Get(b => b.BackgroundAnimations);
            };

            window.Deactivated += WindowOnDeactivate;
            if (options.IsDialogue)
            {
                await window.ShowDialog(desktop.MainWindow);
            }
            else
            {
                window.Show();

                if (options.BlockUntilContentCloses)
                {
                    var completionSource = new TaskCompletionSource();

                    window.Closed += (_, _) =>
                    {
                        completionSource.TrySetResult();
                    };

                    await completionSource.Task;
                }
            }

            return window;
        }
        else if(OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            // TODO: Use navigation system pushing for phone OSes
        }

        return null;
    }

    public static void RefreshFavouritesInMainMenu()
    {
        AddFavouritesToToolbar((App.GetMainWindow() as SukiWindow)!);
    }
    
    private static void AddFavouritesToToolbar(SukiWindow window)
    {
        var favourites = GetFavouritedViews();
        var locale = App.GetService<LocalisationService>();
        
        window.RightWindowTitleBarControls = new Avalonia.Controls.Controls(favourites.Select(GetTitleBarButton)
            .ToList().Prepend(GetTitleBarButton(GetView("HomeTab"))));
        return;

        Button GetTitleBarButton(NavigationViewItemWrapper c)
        {
            var button = new Button
            {
                Padding = new(5),
                Margin = new Thickness(3, 3, 3, 0),
                Content = new MaterialIcon
                {
                    Kind = c.Symbol
                },
                IsVisible = c.Item.IsVisible
            };
            button.Classes.Add("Rounded");
            button.Command = new RelayCommand(() =>
            {
                App.FocusMainWindow();
                App.GetService<PageNavigationService>().RequestNavigation(c.PageType);
            });
            
            ToolTip.SetTip(button, locale[c.Item.Name]);
            
            return button;
        }
    }
    
    private static void WindowOnDeactivate(object? sender, EventArgs eventArgs)
    {
        var window = (sender as SukiWindow)!;
        
        window.Activated += OnActivate;
    }

    private static void OnActivate(object? sender, EventArgs eventArgs)
    {
        var window = (sender as SukiWindow)!;

        if (window.CustomContent is UserControlBase { SynchronisationModel: { } syncAware })
        {
            syncAware.OnSynchronisationRequestedAsync(CodeBook.ResState);
        }
        
        // Important to prevent multiple refreshes on consequent activations
        window.Activated -= OnActivate;
    }

    public static void CloseWindow<T>() where T: UserControl
    {
        // As for phone OSes, we must the navigation system to pop the current view.
        
        if (OperatingSystem.IsWindows())
        {
            App.CloseDialogWindowOfContentType<T>();
        }
        else if(OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            // TODO: Use navigation system popping for phone OSes
        }
    }

    public static void InvokeMenuItemsLoaded()
    {
        MenuItemsLoaded?.Invoke(null, EventArgs.Empty);
    }

    public static byte[] GetDefaultProductImage()
    {
        try
        {
            var data = File.ReadAllText("Assets/def.data");

            if (string.IsNullOrEmpty(data))
                return [];
            
            return Convert.FromBase64String(data);
        }
        catch
        {
            return [];
        }
    }
    
    public static async Task<(bool result, List<ComboModel> item)> OpenItemSelectionWindowAsync(
        string previousItemId,
        string? endpoint = "Sales",
        string? itemEndpoint = null,
        string windowTitleLocale = "SelectItem")
    {
        var window = new ItemSelectionWindow(new ItemSelectionWindowViewModel
            (null, EntryType.Add, previousItemId, endpoint, itemEndpoint));
        
        var entry = await ShowCrossPlatformContentAsync(window, new(App.GetService<LocalisationService>()[windowTitleLocale]));
        var selection = window.ViewModel.WaitForUserToSelectAsync();
        
        var maxWait = Task.Delay(6000000);
        
        // Wait for the selection to happen, or the max wait time.
        await Task.WhenAny(selection, maxWait);
        
        if (selection.Status != TaskStatus.RanToCompletion)
        {
            entry?.Close();
            return (false, null);
        }

        var task = await selection;
        
        return (task.result, task.model);
    }

    public static async Task<(bool result, List<ItemsSelectionModel> items)> OpenItemsSelectionWindowAsync(
        Identifier? customStorageId,
        bool hidePrice = false)
    {
        var window = new ItemsSelectionWindow(new ItemsSelectionWindowViewModel
            (customStorageId, hidePrice));
        
        var entry = await ShowCrossPlatformContentAsync(window, new(App.GetService<LocalisationService>()["SelectItems"]));
        var selection = window.ViewModel.WaitForUserToSelectAsync();
        
        var maxWait = Task.Delay(6000000);
        
        await Task.WhenAny(selection, maxWait);
        
        if (selection.Status != TaskStatus.RanToCompletion)
        {
            entry?.Close();
            return (false, null);
        }

        var task = await selection;
        
        return (task.result, task.model);
    }
    
    public static AdvancedImage GetButtonIcon(string name)
    {
        var uri = new Uri($"avares://Leapay.Client/Assets/{name}.png");
        return new AdvancedImage(uri);
    }
    
    public static string GetTooltipForIcon(MaterialIconKind icon)
    {
        var loc = App.GetService<LocalisationService>();
        
        return icon switch
        {
            MaterialIconKind.ContentCopy => loc["Copy"],
            MaterialIconKind.Barcode => loc["PrintBarcode"],
            MaterialIconKind.PrinterPointOfSaleCheck => loc["PrintReceipt"],
            MaterialIconKind.Whatsapp => loc["SendInvoiceToWhatsapp"],
            MaterialIconKind.Printer => loc["Print"],
            MaterialIconKind.Delete => loc["Delete"],
            MaterialIconKind.Edit => loc["Edit"],
            MaterialIconKind.Add => loc["Add"],
            _ => string.Empty
        };
    }

    public static void ApplyButtonBackgroundStyling(UserControl control, LocalisationService locale)
    {
        ApplyGlobalStylingTo<Button>(control, a =>
        {
            ContentSelector(a, b => b.Content?.ToString() == locale["SaveButton"],
                c => c.Background = GetGreenGradient());
            ContentSelector(a, b => b.Content?.ToString() == locale["CancelButton"],
                c => c.Background = GetRedGradient());
            ContentSelector(a, b => b.Content?.ToString() == locale["ResetButton"], 
                c => c.Background = GetZeroGradient());
            
            AddClassesTo("Flat Rounded", a);
        });
        
        ApplyGlobalStylingTo<ScrollViewer>(control, a =>
        {
            a.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        });
    }
    
    public static void AddClassesTo(string className, params Control[] controls)
    {
        foreach (var control in controls)
        {
            if (control.Classes.Contains(className))
                continue;

            var classes = className.Split(' ');

            if (classes.Length != 0)
            {
                control.Classes.AddRange(classes);
            }
        }
    }
    
    public static void SetGridResolution(Window control, Grid? grid)
    {
        if (grid is null)
        {
            Warning("Pass a valid Grid to set resolution.");
            return;
        }
        
        var resolution = control.Screens.Primary?.Bounds;

        if (resolution.TryGet(out var val))
        {
            if (control.WindowState == WindowState.Maximized)
            {
                /*grid.Width = val.Width * 0.8;
                grid.Height = val.Height * 0.7;*/

                if (val.Height >= 1080)
                {
                    grid.Width = OperatingXRes;
                    grid.Height = OperatingYRes;
                }
                else
                {
                    // For non-standard non-1080p screens, don't make the text too small.
                    grid.Width = OperatingXRes - 400;
                    grid.Height = OperatingYRes - 200;
                }
            }
            else
            {
                grid.Width = control.Width * 0.9;
                grid.Height = control.Height * 0.9;
            }
        }
        else
        {
            Warning("Could not scale Grid due to no resolution being found.");
        }
    }
    
    public static TContent? GetWindowContent<TContent>(Window window) where TContent : UserControl
        => window.Content?.As<Viewbox>().Child?
            .As<BusyArea>().Content?.As<Grid>().Children.FirstOrDefault() as TContent;

    public static bool DoesWindowHaveContentType<TContent>(Window window) where TContent: UserControl
        => GetWindowContent<TContent>(window) is not null;

    public static void ExecuteGlobal<TWindowContent>(Action<Window> action) where TWindowContent: UserControl
    {
        var window = App.GetOpenWindowOfType<TWindowContent>();
        
        if (window is null)
        {
            Warning("No open window of type {0} found to execute action.", typeof(TWindowContent).Name);
            return;
        }
        
        action.Invoke(window);
    }
    
    public static void ExecuteLastOpen(Action<Window> action)
    {
        var window = App.GetOpenWindows().LastOrDefault(o => o is not MainWindow);
        
        if (window is null)
        {
            Warning("No open dialog window found to execute an action.");
            return;
        }
        
        action.Invoke(window);
    }
    
    public static TResult GetGlobal<TWindowContent, TResult>(Func<Window, TResult> selector)
        where TWindowContent: UserControl where TResult: class
    {
        var window = App.GetOpenWindowOfType<TWindowContent>();

        if (window is null)
        {
            Warning("No open window of type {0} found to retrieve a property", typeof(TWindowContent).Name);
            return null;
        }

        return selector.Invoke(window);
    }

    public static void ForceFocus(Window window, Control visual)
    {
        Dispatcher.UIThread.Post(() =>
        {
            window.Activate();
            window.FocusManager.ClearFocus();
            window.Focusable = true;
            window.Focus();
            visual.Focusable = true;

            Information("Visual {0} is visible, focus forcing result: {1}, focusable: {2}", visual.Name, visual.Focus(), visual.Focusable);
        }, DispatcherPriority.Background);
    }

    public static LinearGradientBrush? GetRedGradient() =>
        new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(Color.Parse("#FF1744"), 0), // vivid crimson
                new GradientStop(Color.Parse("#D50000"), 0.5), // deep red
                new GradientStop(Color.Parse("#B71C1C"), 1) // rich blood-red
            ]
        };

    public static LinearGradientBrush GetGreenGradient() =>
        new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(Color.Parse("#00C853"), 0), // vivid green
                new GradientStop(Color.Parse("#2E7D32"), 0.5), // material deep green
                new GradientStop(Color.Parse("#1B5E20"), 1) // dark emerald
            ]
        };
    
    public static LinearGradientBrush GetLightGreenGradient() =>
        new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(Color.Parse("#B9F6CA"), 0),   // light mint green
                new GradientStop(Color.Parse("#69F0AE"), 0.5), // soft fresh green
                new GradientStop(Color.Parse("#00E676"), 1)    // bright spring green
            ]
        };

    public static LinearGradientBrush GetWarningYellowGradient() =>
        new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(Color.Parse("#FFF9C4"), 0),   // light soft yellow
                new GradientStop(Color.Parse("#FFEB3B"), 0.5), // standard warning yellow
                new GradientStop(Color.Parse("#FBC02D"), 1)    // deeper amber
            ]
        };
    
    public static LinearGradientBrush GetZeroGradient()
    {
        return new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
            GradientStops = PreferencesService.Get(s => s.Theme == "dark") ? [ new GradientStop(Colors.Transparent, 0)] :
            [
                new GradientStop(Color.Parse("#F5E7FA"), 0), // subtle light grey
                new GradientStop(Color.Parse("#E9EFF1"), 0.5), // soft mid-grey
                new GradientStop(Color.Parse("#E0F8FC"), 1) // professional dark grey
            ]
        };
    }

    public static IEnumerable<T> FindLogicalChildren<T>(ILogical depObj) where T : class
    {
        if (depObj is Visual visual)
        {
            foreach (var child in visual.GetLogicalChildren())
            {
                if (child is T t)
                    yield return t;

                foreach (var descendant in FindLogicalChildren<T>(child))
                    yield return descendant;
            }
        }
    }

    public static void ApplyGlobalStylingTo<T>(UserControl control, Action<T> action) where T: class
    {
        var controls = FindLogicalChildren<T>(control);

        foreach (var button in controls)
        {
            action.Invoke(button);
        }
    }

    public static void ContentSelector<T>(this T control, Predicate<T> selector, Action<T> action)
    {
        if(selector.Invoke(control))
            action.Invoke(control);
    }

    public static void AddView(string name, NavigationViewItemWrapper type)
    {
        _ = _views.TryAdd(name, type);
    }

    public static NavigationViewItemWrapper GetView(string name)
    {
        _ = _views.TryGetValue(name, out var type);

        return type ?? throw new Exception($"View {name} not found while fetching from cache.");
    }

    public static NavigationViewItemWrapper? GetViewByType(Type type)
    {
        NavigationViewItemWrapper? view = null;

        foreach (var viewB in _views.Values.Where(viewB => viewB.Item.InnerContent.GetType() == type))
        {
            view = viewB;
        }

        return view;
    }
    
    public static bool TryGetView(string? name, out NavigationViewItemWrapper? view)
    {
        if (name is null)
        {
            view = null;
            return false;
        }
            
        var find = _views.TryGetValue(name, out var val);
        view = val;

        return find;
    }
    
    public static List<NavigationViewItemWrapper> GetFavouritedViews()
    {
        var favouritedViewsStr = PreferencesService.Get(s => s.FavouritedViews);
        return favouritedViewsStr.Select(GetView).Where(v 
            => v.Item.IsVisible).ToList();
    }
    
    public static bool IsFavourited(string name)
    {
        return PreferencesService.Get(s => s.FavouritedViews)
            .Any(s => s == name);
    }

    public static string GetViewsOriginalName(string localeName)
    {
        return _views.FirstOrDefault(s 
            => localeName.Contains(App.GetService<LocalisationService>()[s.Key])).Key;
    }
    
    public static SKColor? ToSkColor(this IBrush? brush)
    {
        switch (brush)
        {
            case SolidColorBrush solid:
                return new SKColor(solid.Color.R, solid.Color.G, solid.Color.B, solid.Color.A);

            case LinearGradientBrush { GradientStops.Count: > 0 } linear:
                var firstStop = linear.GradientStops[0];
                var c = firstStop.Color;
                return new SKColor(c.R, c.G, c.B, c.A);

            default:
                return null;
        }
    }

    public static void GenerateReceipt(DtoSalesInvoice args)
    {
        var receiptItems = args.Details.Select(m => new ReceiptSaleItem
        {
            ItemName = m.ItemName,
            SellPrice = m.Price,
            Discount = m.Discount,
            Quantity = m.Quantity
        });

        var home = App.GetService<CompanyInfoPageViewModel>();
        var locale = home.GetLocale(out var isLtr);
        
        var netAmount = args.Count();
        var thermalReceipt = new Windows.Interop.ThermalReceipt(receiptItems.ToList(),
            home.CompanyInfo.Name,
            home.CompanyInfo.PhoneNumber1, 
            home.CompanyInfo.ImageData,
            netAmount.discountedTotal,
            args.Number);
        
        thermalReceipt.PrintReceiptAsImageQuest(locale,
            args.Discount,
            isLtr);
    }
    
    public static string GetPhotoLink(Identifier itemId, double counter)
    {
        return StringGlobals.BackendUrl +
               $"/Item/Image/?id={itemId.ToString()}&counter={counter}";
    }

    public static string GetDirectImageLink(string appendix)
    {
        return StringGlobals.BackendUrl + appendix;
    }
    
    public static Func<Control> GetIconTitle(string title, MaterialIconKind icon, IBrush? foreground = null, double fontSize = 15)
    {
        return () => new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            Children =
            {
                new MaterialIcon
                {
                    Kind = icon,
                    Foreground = foreground ?? new SolidColorBrush(Colors.Transparent)
                },
                new TextBlock
                {
                    Text = title,
                    Margin = new(0, 1, 0, 0),
                    FontWeight = FontWeight.Bold,
                    FontSize = fontSize
                }
            },
        };
    }
}