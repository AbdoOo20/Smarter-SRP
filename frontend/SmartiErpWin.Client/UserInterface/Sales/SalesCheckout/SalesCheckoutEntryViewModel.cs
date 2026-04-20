using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fly.Shared.Modules.PurchaseAndSales.Requests;
using Fly.Shared.Modules.Store.Dtos;
using SmartiErpWin.Client.DtoModels.SalesModels;
using Wpf.Ui.Controls;
using Button = Wpf.Ui.Controls.Button;
using TextBlock = Wpf.Ui.Controls.TextBlock;
using TextBox = System.Windows.Controls.TextBox;
// ReSharper disable AsyncVoidLambda

namespace SmartiErpWin.Client.UserInterface.Sales.SalesCheckout;

public sealed partial class SalesCheckoutEntryViewModel : BaseRefreshablePageViewModel<FastSaleInvoiceWrapper>
{
    #region Fields

    private static string BaseEndpoint => "FastSales";
    
    protected override string Endpoint => "";

    [ObservableProperty]
    private FastSaleInvoiceWrapper _data = new();

    [ObservableProperty]
    private string? _currentBarcode;

    [ObservableProperty]
    private decimal? _currentDiscountValue = 0m;

    private TabControl? CategoriesTabControl { get; set; }

    private StackPanel? ActiveDataStackPanel { get; set; }

    private TextBox? BarcodeBox { get; set; }
    
    private IEnumerable<DtoCategory> Categories { get; set; } = [];

    private IEnumerable<DtoItem> Items { get; set; } = [];

    #endregion

    public SalesCheckoutEntryViewModel(
        LocalisationService localisationService,
        HttpClient client,
        Type pageType) : base(client)
    {
        InitBaseModel(localisationService, null, null);

        ApplicationTitle = GetPageName(pageType, EntryType.Add);
    }

    #region State Management

    public async Task InitAsync(
        TabControl categoriesTabControl,
        StackPanel activeDataStackPanel,
        TextBox barcodeBox)
    {
        CategoriesTabControl = categoriesTabControl;
        ActiveDataStackPanel = activeDataStackPanel;
        BarcodeBox = barcodeBox;

        //BarcodeBox.TextChanged += BarcodeBoxOnTextChanged;
        BarcodeBox.PreviewKeyDown += BarcodeBoxOnPreviewKeyDown;
        SetStaticCombo();

        await ResetAndRefreshAsync();
    }

    private async void BarcodeBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter)
        {
            await MatchBarcodeAsync(BarcodeBox.Text);
        }
    }

    public async Task ResetAndRefreshAsync()
    {
        RefreshState(async () =>
        {
            await ResetAsync();
        });
    }
    
    private async Task ResetAsync()
    {
        await SetCategoriesAsync();
        await SetDataAsync();
    }

    public async Task SetDataAsync()
    {
        Data = null;
        Data = new();

        await SoftRefreshAsync();
    }
    
    public async Task SoftRefreshAsync()
    {
        await SetActiveItemsAsync();
        
        OnPropertyChanged(nameof(Data));
    }

    #endregion

    #region Logic

    private async void BarcodeBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BarcodeBox?.Text == string.Empty)
            return;
        
        await MatchBarcodeAsync(BarcodeBox?.Text);
    }

    private void SetStaticCombo()
    {
        ComboModelBase.SetStaticItemsInternal<DiscountType>(LocaleService);
        ComboModelBase.SetStaticComboBoxItemEnum(DiscountType.Value);
        ComboModelBase.OnStaticComboItemChanged += OnDiscountTypeChanged;
    }

    private EventHandler? OnDiscountTypeChanged()
    {
        switch (ComboModelBase.ConvertToEnum<DiscountType>(ComboModelBase.SelectedStaticComboItem))
        {
            case DiscountType.Value:
                Data.ApplyDiscountAsValue(CurrentDiscountValue.GetValueOrDefault());
                break;
            case DiscountType.Percentage:
                Data.ApplyDiscountAsPercentage((double)CurrentDiscountValue.GetValueOrDefault());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _ = SoftRefreshAsync();

        return default;
    }

    partial void OnCurrentDiscountValueChanged(decimal? value)
    {
        OnDiscountTypeChanged();
    }
    
    private async Task MatchBarcodeAsync(string? barcode)
    {
        var item = Items.FirstOrDefault(i => i.Barcode == barcode);

        if (item is null)
        {
            await DialogueServices.ShowMessageAsync("NoBarcodeItemFoundTitle",
                "NoBarcodeItemFoundMessage",
                localeService: LocaleService);

            return;
        }
        
        BarcodeBox?.Clear();
        
        Data.AddOrUpdate(item);
        await SoftRefreshAsync();
        //SoundHelper.Play(Sound.BarcodeScannerBeep);
    }

    [RelayCommand]
    private async Task OnAbortAsync()
    {
        await ResetAndRefreshAsync();
    }

    [RelayCommand]
    private async Task PayAsync()
    {
        await PostSaveInnerAsync(Data, BaseEndpoint);
        DialogueServices.ShowSuccessSnack("Success", "SalesInvoiceCreatedSuccessfully");
        await OnAbortAsync();
    }
    
    public override async Task OnNavigatedToAsync()
    {
        RefreshState(async () =>
        {
            // New 'Items' data must be fetched every time the user enters the page,
            // since any pricing can change at any moment.
            await FetchNewDataAsync();
            await SetCategoriesAsync(false);
            await SoftRefreshAsync();
        });
    }

    private async Task FetchNewDataAsync()
    {
        Categories = (await GetInnerAllSpecificAsync<DtoCategory>("Category", null)).Data;
        Items = (await GetInnerAllSpecificAsync<DtoItem>("Item", null)).Data;
    }
    
    private async Task SetCategoriesAsync(bool resetData = true)
    {
        if (CategoriesTabControl is null)
            return;

        if (resetData)
        {
            await FetchNewDataAsync();
        }

        CategoriesTabControl.Items.Clear();

        AddCategories();
    }
    
    private Task SetActiveItemsAsync()
    {
        ActiveDataStackPanel?.Children.Clear();

        var innerPanel = new StackPanel
        {
            Margin = new Thickness(0, 7, 0, 0),
        };

        // The items might have changed since the last time the user entered the page,
        // so let's synchronize it with the active cart for accurate pricing.
        
        Data.SynchronizeItems(Items.ToList());
        CreateItems(innerPanel);
        var card = CreateItemsPanel(innerPanel);

        ActiveDataStackPanel?.Children.Add(card);
        return Task.CompletedTask;
    }

    #endregion
    
    #region UserInterface

    private Card CreateItemsPanel(StackPanel innerPanel)
    {
        var scrollViewer = new DynamicScrollViewer
        {
            Height = CategoriesTabControl.MinHeight * 0.89,
            MaxHeight = CategoriesTabControl.MinHeight * 0.89,

            Content = innerPanel,
        };

        var card = new Card
        {
            VerticalAlignment = VerticalAlignment.Top,
            Content = new StackPanel
            {
                Children =
                {
                    new TextBlock
                    {
                        Text = LocaleService["Items"],
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                    },
                    scrollViewer
                }
            }
        };
        return card;
    }

    private void CreateItems(StackPanel innerPanel)
    {
        foreach (var item in Data.Items)
        {
            var availableUnits = new ObservableCollection<DtoUnit>();

            foreach (var unit in item.Item?.Units ?? [])
            {
                availableUnits.Add(unit);
            }

            var comboBox = CreateUnitComboWithTemplate(availableUnits, item);
            var quantityBox = CreateQuantityBox(item);
            var button = CreateDeleteButton(item);
            var itemUi = CreateIndividualItemInterface(item, button, quantityBox, comboBox);

            innerPanel.Children.Add(itemUi);
        }
    }

    private Button CreateDeleteButton(ItemUnitSale item)
    {
        var button = new Button
        {
            Content = new SymbolIcon(SymbolRegular.Delete12, filled: false),
            Width = 45,
            Height = 35,
            Command = new RelayCommand(async () =>
            {
                if (!await DialogueServices.ConfirmDeleteAsync(LocaleService))
                    return;

                Data.Delete(item);
                await SoftRefreshAsync();
            })
        };
        return button;
    }

    private Wpf.Ui.Controls.TextBox CreateQuantityBox(ItemUnitSale item)
    {
        var quantityBox = new Wpf.Ui.Controls.TextBox
        {
            ClearButtonEnabled = false,
            Text = item.Quantity.ToString(),
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5, 0, 0, 0),
            MinWidth = 30,
            MaxHeight = 38,
        };

        quantityBox.LostKeyboardFocus += QuantityBoxOnLostKeyboardFocus;
        quantityBox.PreviewKeyDown += (_, args) =>
        {
            if (args.Key == Key.Enter)
                QuantityBoxOnLostKeyboardFocus(null, null);
        };
        return quantityBox;

        async void QuantityBoxOnLostKeyboardFocus(
            object? o,
            KeyboardFocusChangedEventArgs? keyboardFocusChangedEventArgs)
        {
            if (!decimal.TryParse(quantityBox.Text, out var newQuantity))
                return;

            Data.AddOrUpdate(item.Item, newQuantity);
            await SoftRefreshAsync();
        }
    }

    private ComboBox CreateUnitComboWithTemplate(ObservableCollection<DtoUnit> availableUnits, ItemUnitSale item)
    {
        var itemTemplate = new DataTemplate(typeof(DtoUnit));
        var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
        textBlockFactory.SetBinding(System.Windows.Controls.TextBlock.TextProperty, new Binding("Name"));
        itemTemplate.VisualTree = textBlockFactory;

        var comboBox = new ComboBox
        {
            ItemsSource = availableUnits,
            HorizontalAlignment = HorizontalAlignment.Right,
            ItemTemplate = itemTemplate,
            SelectedItem = item.Unit,
            Margin = new Thickness(5, 0, 0, 0)
        };

        comboBox.SelectionChanged += async (_, _) =>
        {
            Data.UpdateUnit(item.Item, comboBox.SelectedItem as DtoUnit);
            await SoftRefreshAsync();
        };
        return comboBox;
    }

    private static Card CreateIndividualItemInterface(ItemUnitSale item,
        Button button,
        Wpf.Ui.Controls.TextBox quantityBox,
        ComboBox comboBox)
    {
        var itemUi = new Card
        {
            Padding = new Thickness(0),
            Content = new DockPanel
            {
                Margin = new Thickness(10, 5, 10, 5),
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Children =
                        {
                            new Border
                            {
                                Width = 50,
                                Height = 50,
                                CornerRadius = new CornerRadius(12),
                                Background = new ImageBrush(new CachedBitmap(new BitmapImage(
                                        new Uri(string.IsNullOrEmpty(item.Item?.Photo)
                                            ? ImagePlaceholder
                                            : item.Item?.Photo ?? ImagePlaceholder)),
                                    BitmapCreateOptions.None, BitmapCacheOption.Default))
                                {
                                    Stretch = Stretch.UniformToFill
                                },
                            },
                            new TextBlock
                            {
                                TextWrapping = TextWrapping.Wrap,
                                Text = $"{item.Item?.Name} - {item.Item?.Price * item.Quantity}",
                                Margin = new Thickness(10, 13, 0, 0),
                                FontSize = 16,
                            }
                        }
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Children =
                        {
                            /*new TextBlock
                                    {
                                        Text = "\u00d7",
                                        FontSize = 24,
                                        Margin = new Thickness(0, 2, 0, 0)
                                    },*/
                            button,
                            quantityBox,
                            comboBox
                        }
                    }
                }
            }
        };
        return itemUi;
    }

    private void AddCategories()
    {
        foreach (var category in Categories)
        {
            var itemsInCategory = Items.Where(i => i.CategoryId == category.Id);

            var itemsUi = CreateCategoryItems(itemsInCategory);
            var tabItem = CreateCategoriesContentWrapper(itemsUi, category);

            CategoriesTabControl.Items.Add(tabItem);
        }
    }

    private TabItem CreateCategoriesContentWrapper(List<UIElement> itemsUi, DtoCategory category)
    {
        var wrapPanel = new VirtualizingItemsControl
        {
            MinHeight = CategoriesTabControl.MinHeight * 0.95,
            Width = CategoriesTabControl.ActualWidth * 0.75,
            HorizontalAlignment = HorizontalAlignment.Left,
        };

        foreach (var itemUi in itemsUi)
        {
            wrapPanel.Items.Add(itemUi);
        }

        var tabItem = new TabItem
        {
            Header = category.Name,
            Content = new Card
            {
                Padding = new Thickness(3),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                MinHeight = CategoriesTabControl.MinHeight * 0.95,
                Content = wrapPanel
            }
        };
        return tabItem;
    }

    private List<UIElement> CreateCategoryItems(IEnumerable<DtoItem> itemsInCategory)
    {
        var itemsUi = itemsInCategory
            .Select(itemInCategory
                => new Button
                {
                    Height = 180,
                    Width = 180,
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Command = new RelayCommand(async delegate
                    {
                        Data.AddOrUpdate(itemInCategory);
                        await SoftRefreshAsync();
                    }),
                    Content = new StackPanel
                    {
                        Margin = new Thickness(1),
                        
                        Children =
                        {
                            new Border
                            {
                                Width = 120,
                                Height = 120,
                                CornerRadius = new CornerRadius(12),
                                Background = new ImageBrush(new CachedBitmap(new BitmapImage(
                                        new Uri(string.IsNullOrEmpty(itemInCategory.Photo)
                                            ? ImagePlaceholder
                                            : itemInCategory.Photo ?? ImagePlaceholder)),
                                    BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default))
                                {
                                    Stretch = Stretch.UniformToFill
                                },
                            },
                            new DockPanel
                            {
                                Margin = new Thickness(0, 5, 0, 0),
                                Children =
                                {
                                    new TextBlock
                                    {
                                        Text = itemInCategory.Name,
                                        FontSize = 14,
                                        MaxWidth = 80,
                                        TextWrapping = TextWrapping.Wrap,
                                        FontWeight = FontWeights.Bold,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                    },
                                    new TextBlock
                                    {
                                        FontSize = 14,
                                        Text = itemInCategory.Price.ToString("N"),
                                        HorizontalAlignment = HorizontalAlignment.Right,
                                    }
                                }
                            }
                        }
                    }
                }).Cast<UIElement>().ToList();
        return itemsUi;
    }

    #endregion
}