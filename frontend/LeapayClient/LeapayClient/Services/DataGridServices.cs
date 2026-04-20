using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using ClosedXML;
using Leapay.Client.Controls;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Attributes;
using Fly.Shared.Common;
using Leapay.Client.Converters;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Helpers;
using Material.Icons.Avalonia;
using SmartErpWin.Core;

namespace Leapay.Client.Services;

public record ButtonColumn(string ColumnName, Action<object, object?> Action, Func<Control> Interface, object Data);

public static class DataGridServices
{
    static DataGridServices()
    {
    }

    public static Task<BindingDataGrid> CreateDataGridForDtoModelAsync<TBaseModel>(
        LocalisationService? locale,
        IEnumerable<object>? models,
        EventHandler<DataGridButtonEventArgs<TBaseModel>>? onEdit = null,
        EventHandler<DataGridButtonEventArgs<TBaseModel>>? onDelete = null,
        List<Tuple<object, EventHandler<DataGridButtonEventArgs<TBaseModel>>>>? extraButtons = null,
        bool previewColumns = true,
        IBrush? backBrush = null,
        bool isCompact = false)
        where TBaseModel: class, new()
    {
        var modelsList = models?.OfType<TBaseModel>().ToList();

        var enableBinding = true;
        
        if ((modelsList?.Count == 0 || modelsList is null) && previewColumns)
        {
            // Simulate columns with an empty model if no models are present.
            // This is useful for displaying headers without data.
            // But must so be done with binding disabled.

            return Task.FromResult(new BindingDataGrid());
            onDelete = null;
            onEdit = null;
            extraButtons = [];
            modelsList = [new()];
            enableBinding = false;
        }
        
        var type = modelsList?.FirstOrDefault()?.GetType();

        if (type is null)
            return Task.FromResult(new BindingDataGrid());

        var dataGrid = new BindingDataGrid
        {
            Margin = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            IsReadOnly = true,
            GridLinesBrush = backBrush,
            IsCompact = isCompact
        };

        if (backBrush is not null)
        {
            // StyleDataGridColumnHeaders(dataGrid, backBrush);
        }
        
        var buttons = new List<Tuple<object, EventHandler<DataGridButtonEventArgs<TBaseModel>>>>();
        
        if (onEdit != null)
        {
            // ContentHelper.GetButtonIcon(MaterialIconKind.Edit)
            buttons.Add(new(MaterialIconKind.Edit, onEdit));
        }

        if (onDelete != null)
        {
            buttons.Add(new(MaterialIconKind.Delete, onDelete));
        }

        if (extraButtons is not null)
        {
            buttons.AddRange(extraButtons);
        }

        AddAllButtons();
        AddTypeProperties(type);

        dataGrid.ItemsSource = modelsList ?? [];

        return Task.FromResult(dataGrid);

        void AddAllButtons()
        {
            for (var i = 0; i < buttons.Count; i++)
            {
                var column = buttons[i];
                var isLast = i == buttons.Count - 1;
                var isFirst = i == 0;
                
                dataGrid.Columns.Add(CreateButtonColumn(column.Item1,
                    column.Item2,
                    isLast,
                    isFirst));
            }
        }

        void AddTypeProperties(Type typeToAdd)
        {
            foreach (var property in typeToAdd.GetProperties())
            {
                if ((property.GetCustomAttributes(true).OfType<ClientInvisibleAttribute>().Any()
                     || InvisibleTypes.Any(t => t == property.PropertyType))
                    && !Debugger.IsAttached)
                    continue;

                var binding = new Binding(property.Name)
                {
                    Mode = BindingMode.OneTime,
                    Converter = new DataGridBoundValueConverter(),
                    ConverterParameter = property.Name
                };

                var headerVal = property.GetAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName
                    ?? property.Name;
                
                dataGrid.Columns.Add(new SimpleDataGridColumn
                {
                    Header = CreateHeader(locale[headerVal]),
                    HeaderId = property.Name,
                    CellTemplate = new FuncDataTemplate<object>((data, _) => new TextBlock
                    {
                        [!TextBlock.TextProperty] = binding
                    }, true)
                });
            }
        }
    }
    
    public static Task<StaticDataGrid> CreateDataGridForStringModelAsync(
        LocalisationService? locale,
        IEnumerable<object[]> models,
        List<ButtonColumn[]>? buttons = null,
        IBrush? backBrush = null,
        object? parent = null)
    {
        var modelsList = models?.ToList();
        if (modelsList is null || !modelsList.Any())
        {
            return Task.FromResult(new StaticDataGrid());
        }

        var headers = modelsList.First();
        var dataRows = modelsList.Skip(1).ToList();
        var buttonRows = new List<object[]>();

        var dataGrid = new StaticDataGrid
        {
            Margin = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            GridLinesBrush = backBrush
        };

        if (buttons is not null && buttons.Count != 0)
        {
            for (var j = 0; j < buttons.Count; j++)
            {
                dataGrid.Columns.Add(new SimpleDataGridColumn
                {
                    Header = CreateHeader(locale?[buttons.ElementAtOrDefault(j)?.FirstOrDefault()?.ColumnName] ?? " "),
                    IsButtonColumn = true,
                    ConcreteControls = buttons[j].Select(s => new Button
                    {
                        Content = s.Interface.Invoke(),
                        Classes = { "Rounded" },
                        Height = 30,
                        Command = new RelayCommand(() => s.Action.Invoke(s.Data, parent))
                    }).AsEnumerable()
                });
            }
        }
        
        AddColumns();
        dataGrid.Populate(dataRows, buttons);
    
        return Task.FromResult(dataGrid);

        void AddColumns()
        {
            for (var i = 0; i < headers.Length; i++)
            {
                var colIndex = i;

                var text = headers.ElementAtOrDefault(i)?.ToString();

                if (text?.Contains(SharedConstants.InternalTableColumn) ?? false)
                    continue;
                
                var funcDataTemplate = new FuncDataTemplate<string[]>((data, _) => new TextBlock 
                {
                    Text = data.ElementAtOrDefault(colIndex) ?? string.Empty
                }, true);
                
                dataGrid.Columns.Add(new SimpleDataGridColumn
                {
                    HeaderId = text,
                    Header = CreateHeader(locale?[text] ?? " "),
                    CellTemplate = funcDataTemplate
                });
            }
        }
    }

    private static object CreateHeader(string header)
    {
        return new TextBlock
        {
            Text = header,
            Padding = new Thickness(3),
            HorizontalAlignment = HorizontalAlignment.Center
        };
    }

    private static IBrush? GetForegroundForIcon(MaterialIconKind icon)
    {
        var vm = App.GetService<MainWindowViewModel>();
        return icon switch
        {
            MaterialIconKind.Delete => ContentHelper.GetRedGradient(),
            _ => vm.BackgroundTheme
        };
    }
    
    private static SimpleDataGridColumn CreateButtonColumn<TBaseModel>(
        object header,
        EventHandler<DataGridButtonEventArgs<TBaseModel>> handler,
        bool isLast = false,
        bool isFirst = false) where TBaseModel: class, new()
    {
        var locale = App.GetService<LocalisationService>();
        
        object? buttonData;

        if (header is MaterialIconKind icon)
        {
            buttonData = new Func<Control>(() => new MaterialIcon
            {
                Kind = icon,
                Foreground = GetForegroundForIcon(icon),
                Width = 22,
                Height = 22
            });
        }
        else
        {
            buttonData = header;
        }
        
        var lastText = new Func<Control>(() => new TextBlock
        {
            Text = locale["Tools"],
            // this is a weird combination to fit the text without making the
            // total actual size too big
            FontSize = 2,
            RenderTransform = new TransformGroup
            {
                Children =
                {
                    //new TranslateTransform(2, 2),
                    new ScaleTransform(7.5, 7.5)
                }
            }  
        });
        
        var buttonColumn = new SimpleDataGridColumn
        {
            Header = isFirst ? lastText() : " ",
            CellTemplate = new FuncDataTemplate<TBaseModel>((dataContext, _) =>
            {
                var button = new Button
                {
                    Content = buttonData is Func<Control> factory 
                        ? factory() 
                        : buttonData,
                    FontSize = 16,
                    Padding = new Thickness(1),
                    MinHeight = 30,
                    Width = 50,
                    Margin = new Thickness(isFirst ? 0 : 3, 3, isLast ? 6 : 2, 3)
                };

                button.Command = new RelayCommand(() =>
                {
                    handler.Invoke(button, new(dataContext
                        .DeepCopy()));
                });
                
                if(header is MaterialIconKind buttonIcon)
                {
                    ToolTip.SetTip(button, ContentHelper.GetTooltipForIcon(buttonIcon));
                } 
                
                return button;
            }),
            IsButtonColumn = true
        };

        return buttonColumn;
    }
    
    public static List<Type> InvisibleTypes { get; } = new()
    {
        typeof(ObservableCollection<>),
        typeof(Identifier),
        typeof(Identifier?),
        typeof(IEnumerable<>),
        typeof(ICollection<>),
        typeof(IList<>),
        typeof(List<>),
        typeof(ObservableCollection<Guid>),
        typeof(Guid),
        typeof(Guid?)
    };
    
    public static T? FindParent<T>(Visual child)
        where T : Visual
    {
        var parent = child.GetVisualParent();

        while (parent != null)
        {
            if (parent is T typedParent)
                return typedParent;

            parent = parent.GetVisualParent();
        }

        return null;
    }

    public static T? FindModel<T>(object? sender)
        where T : class
    {
        if (sender is Visual visual)
        {
            var row = FindParent<DataGridRow>(visual);
            if (row?.DataContext is T model)
            {
                return model;
            }
        }

        return null;
    }

    public static BindingDataGrid GetEmptyItemsControl(double minDataGridHeight)
    {
        return new BindingDataGrid
        {
            IsReadOnly = true,
            MinHeight = minDataGridHeight,
            MinWidth = 500,
            Margin = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
        };
    }

    public static BindingDataGrid? FindDataGrid(StackPanel panel)
    {
        return FindChildOfType<BindingDataGrid>(panel);
    }

    public static StackPanel GetErrorItemsControl(string title, string message,
        LocalisationService locale, Action? retryCallback)
    {
        return new StackPanel
        {
            FlowDirection = locale.DetermineFlowDirection(),
            HorizontalAlignment = HorizontalAlignment.Center,
            Children =
            {
                new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        new MaterialIcon
                        {
                            Kind = MaterialIconKind.Error,
                            Margin = new Thickness(0, 5, 0, 0)
                        },
                        new TextBlock
                        {
                            Text = title,
                            Margin = new Thickness(10, 0, 0, 0),
                            FontSize = 25,
                            FontWeight = FontWeight.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                    }
                },
                new TextBlock
                {
                    MaxWidth = 400,
                    Margin = new Thickness(0, 10, 0, 0),
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    FlowDirection = FlowDirection.LeftToRight,
                    FontSize = 15,
                    FontWeight = FontWeight.Regular,
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                retryCallback is not null ?
                new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 0),
                    Content = locale["RetryButton"],
                    Command = new RelayCommand(retryCallback)
                } : new Control()
            }
        };
    }
    
    public static T? FindChildOfType<T>(Visual? parent) where T : class
    {
        if (parent is null)
            return null;
        
        foreach (var child in parent.GetVisualChildren())
        {
            if (child is T match)
                return match;

            var result = FindChildOfType<T>(child);
            if (result is not null)
                return result;
        }

        return null;
    }
}