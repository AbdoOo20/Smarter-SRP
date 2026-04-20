using AsyncImageLoader;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Fly.Framework.Shared.Common;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Services;
using Material.Icons.Avalonia;
using SmartErpWin.Core;
using SmartErpWin.Core.Interfaces;
using SukiUI.Controls;

namespace Leapay.Client.Helpers;

public static class UiHelpers
{
    public static SuggestBoxCreationInfo<T> CreateSuggestionBox<T>(
        Panel? panel,
        object title,
        List<T> models,
        string placeholder = "Search",
        double width = 250,
        Action? onClear = null,
        Action<AutoCompleteBox>? action = null,
        Action<Button>? actionAdorner = null,
        Func<Task<string>>? rightAdornerAction = null,
        Func<string, Task>? rightAdornerCallback = null,
        Func<string, Task<string>>? rightAdornerSelectedAction = null,
        Func<string, Task>? rightAdornerSelectedCallback = null,
        AutoSuggestNotFoundCallbackModel? notFoundCallback = null) where T : ISearchable
    {
        var autoCompleteBox = new AutoCompleteBox
        {
            Width = width,
            Watermark = App.GetService<LocalisationService>()[placeholder],
            ItemsSource = models,
            FilterMode = AutoCompleteFilterMode.ContainsOrdinal,
            MinimumPrefixLength = 1,
            MaxDropDownHeight = 500,
            Margin = new Thickness(0, 10, 0, 0),
            CornerRadius = GetCornerRadius(),
            IsTextCompletionEnabled = false
        };

        var isShowingFirst = false;

        Control? notFoundControl;

        autoCompleteBox.TextChanged += (_, a) =>
        {
            if (!isShowingFirst)
                if (autoCompleteBox.Text?.StartsWith(' ') ?? false)
                    autoCompleteBox.Text = autoCompleteBox.Text.TrimStart();
            
            if (string.IsNullOrEmpty(autoCompleteBox.Text?.Trim()))
            {
                onClear?.Invoke();

                if (notFoundCallback is null)
                    return;

                notFoundCallback.CurrentEntityName = string.Empty;
                notFoundCallback.OnRemoveUi();
            }
        };

        autoCompleteBox.GotFocus += (_, _) =>
        {
            if (!string.IsNullOrEmpty(autoCompleteBox.Text))
                return;

            isShowingFirst = true;

            autoCompleteBox.Text = " ";

            isShowingFirst = false;

            autoCompleteBox.IsDropDownOpen = true;
        };

        autoCompleteBox.LostFocus += (_, _) =>
        {
            if (string.IsNullOrEmpty(autoCompleteBox.Text?.Trim()))
                return;

            // This ensures that when the selected item is modified to
            // an item that does not exist, we revert back to an empty
            // state for the user to select again, i.e., on focus loss.

            // However, we're going to make an exception when the not found callback model
            // is provided by the overload.

            if (autoCompleteBox.SelectedItem is null && notFoundCallback is null)
            {
                autoCompleteBox.Text = string.Empty;
                onClear?.Invoke();
            }
            else if (!string.IsNullOrEmpty(autoCompleteBox.Text) && autoCompleteBox.SelectedItem is null)
            {
                notFoundCallback?.OnEntityNotFound(autoCompleteBox.Text);
            }
        };

        autoCompleteBox.ItemTemplate = new FuncDataTemplate<ISearchable>((item, _)
            => new TextBlock { Text = item?.Display ?? "", FontWeight = FontWeight.Bold });

        action?.Invoke(autoCompleteBox);

        var returnedModel = new SuggestBoxCreationInfo<T>();

        var textBlock = title is string ? new TextBlock
        {
            Text = title as string,
            VerticalAlignment = VerticalAlignment.Center
        } : title;

        var adorner = new Button
        {
            Content = new MaterialIcon
            {
                Margin = new Thickness(0, 0, -7, 0),
                Width = 18,
                Height = 18,
                Kind = MaterialIconKind.Search
            },
            Height = 36,
            Width = 55,
            CornerRadius = new CornerRadius(0, 10, 10, 0),
            Margin = new Thickness(0, 9, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            IsHitTestVisible = false,
            BorderThickness = new Thickness(0)
        };

        adorner.Click += (_, _) =>
        {
            autoCompleteBox.Text = string.Empty;
            autoCompleteBox.Focus();
        };

        var rightActionAdorner = new Button
        {
            IsVisible = GetIsAdornerVisible(),
            Content = GetRightAdornerIcon(),
            BorderThickness = new Thickness(0),
            Height = 36,
            Width = 50,
            Background = GetRightAdornerBackground(),
            CornerRadius = new CornerRadius(10, 0, 0, 10),
            Margin = new Thickness(0, 9, 0, 0),
            VerticalAlignment = VerticalAlignment.Center
        };

        if (rightActionAdorner.IsVisible)
        {
            rightActionAdorner.Click += async (_, _) =>
            {
                if (autoCompleteBox.SelectedItem is null && rightAdornerAction is not null && rightAdornerCallback is not null)
                {
                    await rightAdornerCallback(await rightAdornerAction()); 
                }
                else if(autoCompleteBox.SelectedItem is not null && rightAdornerSelectedAction is not null && rightAdornerSelectedCallback is not null)
                {
                    await rightAdornerSelectedCallback(await rightAdornerSelectedAction((autoCompleteBox.SelectedItem as ComboModel)?.Value ?? "")); 
                }
            };
        }

        adorner.Click += (_, _) =>
        {
            autoCompleteBox.Text = string.Empty;
            autoCompleteBox.Focus();
        };

        actionAdorner?.Invoke(adorner);

        var cachedTheme = adorner.Background;

        autoCompleteBox.GetObservable(AutoCompleteBox.TextProperty)
            .Subscribe(text =>
            {
                var isSearch = string.IsNullOrEmpty(text);
                adorner.IsHitTestVisible = !isSearch;
                adorner.Content = new MaterialIcon
                {
                    Margin = new Thickness(0, 0, -7, 0),
                    Width = 18,
                    Height = 18,
                    Kind = isSearch ? MaterialIconKind.Search : MaterialIconKind.Close
                };
                adorner.Background = isSearch ? cachedTheme : ContentHelper.GetRedGradient();
            });

        var upperStack = new StackPanel
        {
            Spacing = 10,
            Orientation = Orientation.Horizontal,
            Children =
            {
                textBlock switch
                {
                    Control control => control,
                    Func<Control> funcTextBloc => funcTextBloc(),
                    _ => new TextBlock
                    {
                        Text = textBlock.ToString()
                    }
                }
            }
        };

        var coreStack = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Children =
            {
                rightActionAdorner,
                autoCompleteBox,
                adorner
            }
        };

        var stack = new StackPanel
        {
            Margin = new Thickness(5, 0, 0, 0),
            Children =
            {
                upperStack,
                coreStack
            }
        };

        if (notFoundCallback?.UiToShow is Control show)
        {
            notFoundControl = show;
            notFoundControl.IsVisible = false;

            notFoundCallback.AddUi += (_, _) =>
            {
                // To account for the extra pixels taken by the NotFound core
                coreStack.RenderTransform = new TranslateTransform(0, -6);
                notFoundControl.IsVisible = true;
            };
            notFoundCallback.RemoveUi += (_, _) =>
            {
                upperStack.RenderTransform = new TranslateTransform(0, 0);
                notFoundControl.IsVisible = false;
            };

            upperStack.Children.Add(notFoundControl);
        }

        returnedModel.Box = autoCompleteBox;
        panel?.Children.Add(stack);

        autoCompleteBox.SelectionChanged += (_, _) =>
        {
            if (autoCompleteBox.SelectedItem is T selectedItem)
            {
                notFoundCallback?.OnRemoveUi();
                returnedModel.RaiseSuggestionChosen(selectedItem);
            }
            
            UpdateRightAdorner();
        };
        
        return returnedModel;

        void UpdateRightAdorner()
        {
            if (!rightActionAdorner.IsVisible)
                return;

            rightActionAdorner.Background = GetRightAdornerBackground();
            rightActionAdorner.Content = GetRightAdornerIcon();
        }

        IBrush GetRightAdornerBackground()
        {
            return autoCompleteBox.SelectedItem is null
                ? ContentHelper.GetGreenGradient()
                : ContentHelper.GetLightGreenGradient();
        }

        MaterialIcon GetRightAdornerIcon() =>
            new()
            {
                Margin = new Thickness(0, 0, -7, 0),
                Width = 18,
                Height = 18,
                Kind = GetAdornerIconConditional()
            };

        CornerRadius GetCornerRadius()
        {
            return rightAdornerAction is not null
                ? new CornerRadius(0, 0, 0, 0)
                : new CornerRadius(10, 0, 0, 10);
        }

        MaterialIconKind GetAdornerIconConditional()
        {
            if (rightAdornerAction is not null && autoCompleteBox.SelectedItem is null)
            {
                return MaterialIconKind.Add;
            }

            if (rightAdornerSelectedAction is not null && autoCompleteBox.SelectedItem is not null)
            {
                return MaterialIconKind.Edit;
            }

            return MaterialIconKind.About;
        }

        bool GetIsAdornerVisible()
        {
            if (rightAdornerAction is not null)
                return true;
            
            return rightAdornerSelectedAction is not null;
        }
    }
    
    public static SuggestBoxCreationInfo<ItemSearchModel> CreateItemsSearchBox(
        Panel? panel,
        IEnumerable<ItemSearchModel> models,
        IBrush mainBrush,
        string placeholder = "SearchForItemOrBarcode",
        double width = 250,
        Action<AutoCompleteBox>? action = null,
        Func<Task<string>>? rightAdornerAction = null,
        Func<string, Task>? rightAdornerCallback = null,
        Action<Button>? actionAdorner = null)
    {
        var itemSearchModels = models as ItemSearchModel[] ?? models.ToArray();

        var autoCompleteBox = CreateStandardItemsSearchBox();

        autoCompleteBox.KeyDown += OnAutoComplete;

        var hashedTable = new Dictionary<string, string>();
        var hashedTableItemUnits = new Dictionary<string, Tuple<string, string>>();

        Task.Run(AddItemsToHashMap);
        
        action?.Invoke(autoCompleteBox);

        var returnedModel = new SuggestBoxCreationInfo<ItemSearchModel>();

        autoCompleteBox.SelectionChanged += OnSelectionChanged;
        autoCompleteBox.TextChanged += OnTextChanged;
        var adorner = CreateAdornerButton();
        var rightActionAdorner = CreateRightAdorner();

        if (rightActionAdorner.IsVisible)
        {
            rightActionAdorner.Click += RightAdornerAction;
        }

        adorner.Content = CreateAdornerContent();

        var coreStack = CreateCoreStack();

        var stack = new StackPanel
        {
            Margin = new Thickness(5, 0, 0, 0),
            Children =
            {
                coreStack
            }
        };

        returnedModel.Box = autoCompleteBox;
        panel?.Children.Clear();
        panel?.Children.Add(stack);

        return returnedModel;

        void AddItemsToHashMap()
        {
            foreach (var item in itemSearchModels)
            {
                if (string.IsNullOrEmpty(item.Barcode)) continue;

                foreach (var barcode in item.OtherBarcodes)
                {
                    _ = hashedTable.TryAdd(barcode, item.ItemId.ToString());
                }
                
                foreach (var barcode in item.ItemUnitBarcodes)
                {
                    if (barcode.Item2 is null || barcode.Item1.IsNullOrEmpty())
                        continue;
                    
                    var tuple = new Tuple<string, string>(item.ItemId.ToString(), barcode.Item1.ToString());
                    _ = hashedTableItemUnits.TryAdd(barcode.Item2, tuple);
                }
                
                _ = hashedTable.TryAdd(item.Barcode, item.ItemId.ToString());
            }
        }

        void OnTextChanged(object? o, TextChangedEventArgs textChangedEventArgs)
        {
            if (string.IsNullOrEmpty(autoCompleteBox.Text)) return;

            if (hashedTable.TryGetValue(autoCompleteBox.Text, out var id) && Identifier.TryParse(id, null, out var itemId))
            {
                returnedModel.RaiseSuggestionChosen(itemSearchModels.FirstOrDefault(i => i.ItemId == itemId));
                autoCompleteBox.Text = string.Empty;
                FocusBox();
            }
            else if (hashedTableItemUnits.TryGetValue(autoCompleteBox.Text, out var tuple))
            {
                var item = Identifier.Parse(tuple.Item1, null);
                var unit = Identifier.Parse(tuple.Item2, null);

                var modelChosen = itemSearchModels.FirstOrDefault(i => i.ItemId == item).DeepCopy();
                modelChosen.UnitId = unit;
                
                returnedModel.RaiseSuggestionChosen(modelChosen);
                autoCompleteBox.Text = string.Empty;
                FocusBox();
            }
        }

        async void RightAdornerAction(object? o, RoutedEventArgs routedEventArgs)
        {
            var result = await rightAdornerAction();
            await rightAdornerCallback(result);
        }

        void OnAutoComplete(object? sender, KeyEventArgs args)
        {
            if (args.Key is not (Key.Tab or Key.Enter)) return;

            args.Handled = true;
            if (sender is not AutoCompleteBox { ItemsSource: IEnumerable<ItemSearchModel> currentModels, Text: { } searchText } box || string.IsNullOrWhiteSpace(searchText))
            {
                autoCompleteBox.IsTextCompletionEnabled = true;
                autoCompleteBox.IsTextCompletionEnabled = false;
                return;
            }

            var firstItem = currentModels.FirstOrDefault(m => ItemFilter(searchText, m));

            if (firstItem is not null)
            {
                box.SelectedItem = firstItem;
            }
            else
            {
                box.IsTextCompletionEnabled = true;
                box.IsTextCompletionEnabled = false;
            }
        }

        void OnSelectionChanged(object? o, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            CheckAndSendSelection();
        }
        
        Control? Build(ItemSearchModel item, INameScope _) =>
            new DockPanel
            {
                Children =
                {
                    new StackPanel
                    {
                        Spacing = 5,
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        Children =
                        {
                            new Border
                            {
                                CornerRadius = new CornerRadius(10),
                                ClipToBounds = true,
                                Width = 40,
                                Height = 40,
                                VerticalAlignment = VerticalAlignment.Center,
                                Child = new AdvancedImage(new Uri(item?.PhotoLink ?? ContentHelper.GetPhotoLink(Identifier.Empty, 0)))
                                {
                                    Width = 40, Height = 40,
                                    Source = item?.PhotoLink
                                }
                            },
                            new TextBlock
                            {
                                Text = item?.ItemName ?? "", FontWeight = FontWeight.Bold,
                                VerticalAlignment = VerticalAlignment.Center
                            }
                        }
                    },
                    new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        RenderTransform = new TranslateTransform(25, 0),
                        Spacing = 3,
                        Children =
                        {
                            new GlassCard
                            {
                                MinWidth = 80,
                                IsVisible = !string.IsNullOrEmpty(item?.Barcode),
                                Background = mainBrush,
                                Padding = new(1),
                                Content = new StackPanel
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Spacing = 3, Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new MaterialIcon { Kind = MaterialIconKind.Barcode, },
                                        new TextBlock
                                        {
                                            Text = item?.Barcode ?? "",
                                            FontWeight = FontWeight.Bold,
                                            FontSize = 11
                                        }
                                    }
                                }
                            },
                            new GlassCard
                            {
                                MinWidth = 80, Background = mainBrush,
                                Padding = new(1),
                                Content = new StackPanel
                                {
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Spacing = 3, Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new MaterialIcon { Kind = MaterialIconKind.Tag },
                                        new TextBlock
                                        {
                                            Text = item?.ItemPrice ?? "",
                                            FontSize = 11,
                                            FontWeight = FontWeight.Bold
                                        }
                                    }
                                }
                            },
                        }
                    }
                }
            };
        
        void CheckAndSendSelection()
        {
            // Remember this method is only used for automatic selection. Manual user selection
            // should not re-directed here. Use returnedModel.RaiseSuggestionChosen for that.
            
            // The auto suggest box seems to be automatically and prematurely selecting an item if it matches
            // the user input fully. This results in the user not being able to finish typing
            // his search for other items they potentially want to select.
            // To fix this, cancel if multiple items are still in the list.

            var itemsInList = itemSearchModels.Where(i => ItemFilter(autoCompleteBox.Text, i));
            if (itemsInList.Count() > 1)
                return;
                    
            if (autoCompleteBox.SelectedItem is ItemSearchModel selectedItem)
            {
                returnedModel.RaiseSuggestionChosen(selectedItem);
                
                autoCompleteBox.SelectedItem = null;
                var currText = autoCompleteBox.Text;
                // Text won't clear without delaying for some reason
                Task.Delay(300).ContinueWith(_ =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        // Don't clear if the user modified the text in those 300ms
                        if (currText == autoCompleteBox.Text)
                        {
                            autoCompleteBox.Text = string.Empty;
                            FocusBox();
                        }
                    });
                });
            }
        }

        void FocusBox()
        {
            autoCompleteBox.InputHitTest(new Point(5, 5));
        }

        Button CreateAdornerButton()
        {
            return new Button
            {
                Content = new MaterialIcon
                {
                    Margin = new Thickness(0, 0, -7, 0),
                    Width = 18,
                    Height = 18,
                    Kind = MaterialIconKind.Search
                },
                Background = mainBrush,
                Height = 36,
                Width = 55,
                CornerRadius = new CornerRadius(0, 10, 10, 0),
                Margin = new Thickness(0, 9, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                IsHitTestVisible = false,
                BorderThickness = new Thickness(0)
            };
        }

        Button CreateRightAdorner()
        {
            return new Button
            {
                IsVisible = rightAdornerAction != null,
                Content = new MaterialIcon
                {
                    Margin = new Thickness(0, 0, -7, 0),
                    Width = 18,
                    Height = 18,
                    Kind = MaterialIconKind.Add
                },
                BorderThickness = new Thickness(0),
                Height = 36,
                Width = 50,
                Background = ContentHelper.GetGreenGradient(),
                CornerRadius = new CornerRadius(10, 0, 0, 10),
                Margin = new Thickness(0, 9, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        
        bool ItemFilter(string? s, object? b)
        {
            if (b is ItemSearchModel model)
            {
                return (model.ItemName?.Contains(s ?? "", StringComparison.OrdinalIgnoreCase) ?? false)
                       || (model.Barcode?.Contains(s ?? "", StringComparison.Ordinal) ?? false);
            }

            return false;
        }

        AutoCompleteBox CreateStandardItemsSearchBox()
        {
            return new AutoCompleteBox
            {
                Width = width,
                Watermark = App.GetService<LocalisationService>()[placeholder],
                ItemsSource = itemSearchModels,
                FilterMode = AutoCompleteFilterMode.Custom,
                MinimumPrefixLength = 1,
                MaxDropDownHeight = 400,
                Margin = new Thickness(0, 10, 0, 0),
                CornerRadius = rightAdornerAction is not null
                    ? new CornerRadius(0, 0, 0, 0)
                    : new CornerRadius(10, 0, 0, 10),
                ItemTemplate = new FuncDataTemplate<ItemSearchModel>(Build),
                Focusable = true,
                ItemFilter = ItemFilter
            };
        }

        MaterialIcon CreateAdornerContent()
        {
            return new MaterialIcon
            {
                Margin = new Thickness(0, 0, -7, 0),
                Width = 18,
                Height = 18,
                Kind = MaterialIconKind.Search
            };
        }

        StackPanel CreateCoreStack()
        {
            return new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    rightActionAdorner,
                    autoCompleteBox,
                    adorner
                }
            };
        }
    }
}