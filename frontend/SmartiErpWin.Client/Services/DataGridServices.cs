using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Fly.Framework.Shared.Common.Attributes;
using Newtonsoft.Json;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using TextBlock = System.Windows.Controls.TextBlock;

namespace SmartiErpWin.Client.Services;

public static class DataGridServices
{
    public static Task<DataGrid> CreateDataGridForDtoModelAsync(
        LocalisationService? locale,
        IEnumerable<object>? models,
        EventHandler? onEdit = null,
        EventHandler? onDelete = null,
        List<Tuple<string, EventHandler>>? extraButtons = null)
    {
        var type = models?.FirstOrDefault()?.GetType();

        if (type is null)
            return Task.FromResult(new DataGrid());

        var dataGrid = new DataGrid
        {
            IsReadOnly = true,
            Margin = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
        };

        /*
        var columnStyle = new Style(typeof(DataGridColumnHeader), dataGrid.ColumnHeaderStyle);
        columnStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromArgb(128, 100, 80, 100))));
        columnStyle.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(10)));
        */

        //dataGrid.ColumnHeaderStyle = columnStyle;
            
        var style = GetDefaultCellStyle();

        AddTypeProperties(type);

        if (onEdit != null)
        {
            var editButtonColumn = CreateButtonColumn(locale["EditButton"], onEdit);
            dataGrid.Columns.Add(editButtonColumn);
        }

        if (onDelete != null)
        {
            var deleteButtonColumn = CreateButtonColumn(locale["DeleteButton"], onDelete);
            dataGrid.Columns.Add(deleteButtonColumn);
        }

        if (extraButtons is not null)
        {
            foreach (var extra in extraButtons.Select(button => CreateButtonColumn(locale[button.Item1], button.Item2)))
            {
                dataGrid.Columns.Add(extra);
            }
        }

        foreach (var model in models)
        {
            dataGrid.Items.Add(model);
        }

        return Task.FromResult(dataGrid);

        void AddTypeProperties(Type typeToAdd)
        {
            foreach (var property in typeToAdd.GetProperties())
            {
                if (property.GetCustomAttributes(true).OfType<ClientInvisibleAttribute>().Any()
                    /*&& !Debugger.IsAttached*/)
                    continue;

                var binding = new Binding(property.Name)
                {
                    Mode = BindingMode.OneTime
                };

                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Binding = binding,
                    Header = CreateHeader(locale[property.Name]),
                    ElementStyle = style
                });
            }
        }
    }

    public static Task<DataGrid> CreateDataGridForAnunymousModelAsync(
       LocalisationService? locale,
       IEnumerable<string[]> models
       //EventHandler? onEdit = null,
       //EventHandler? onDelete = null,
       /*List<Tuple<string, EventHandler>>? extraButtons = null*/)
    {

        if (models is null)
            return Task.FromResult(new DataGrid());
        
        var Itemsw = new ObservableCollection<string[]>();
        foreach (var row in models)
        {
            Itemsw.Add(row);
        }
        Itemsw.RemoveAt(0);

        var dataGrid = new DataGrid
        {
            IsReadOnly = true,
            Margin = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            ItemsSource = Itemsw,
            AutoGenerateColumns = false,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        
        var style = GetDefaultCellStyle();

        AddTypeProperties();

        //if (onEdit != null)
        //{
        //    var editButtonColumn = CreateButtonColumn(locale["EditButton"], onEdit);
        //    dataGrid.Columns.Add(editButtonColumn);
        //}

        //if (onDelete != null)
        //{
        //    var deleteButtonColumn = CreateButtonColumn(locale["DeleteButton"], onDelete);
        //    dataGrid.Columns.Add(deleteButtonColumn);
        //}

        //if (extraButtons is not null)
        //{
        //    foreach (var extra in extraButtons.Select(button => CreateButtonColumn(locale[button.Item1], button.Item2)))
        //    {
        //        dataGrid.Columns.Add(extra);
        //    }
        //}

        return Task.FromResult(dataGrid);

        void AddTypeProperties()
        {
            for (int i = 0; i < models.ElementAt(0).Length; i++)
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    //CellStyle = style,
                    Binding = new Binding($"[{i}]") { Mode = BindingMode.OneTime },
                    Header = CreateHeader(locale[models.ElementAt(0)[i]]),
                });
            }
        }
    }

    public static Style GetDefaultCellStyle()
    {
        var cellStyle = new Style(typeof(TextBlock));
        cellStyle.Setters.Add(new Setter(TextBlock.PaddingProperty,
            new Thickness(10)));
        cellStyle.Setters.Add(new Setter(
            FrameworkElement.HorizontalAlignmentProperty,
            HorizontalAlignment.Center));
        cellStyle.Setters.Add(new Setter(
            FrameworkElement.VerticalAlignmentProperty,
            VerticalAlignment.Center));

        return cellStyle;
    }

    public static Style GetDataGridStyle<TStyle>() where TStyle : FrameworkElement
    {
        var cellStyle = new Style(typeof(TStyle));
        cellStyle.Setters.Add(new Setter(TextBlock.PaddingProperty,
            new Thickness(10)));
        cellStyle.Setters.Add(new Setter(
            FrameworkElement.VerticalAlignmentProperty,
            VerticalAlignment.Center));

        return cellStyle;
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

    private static DataGridTemplateColumn CreateButtonColumn(
        string header,
        EventHandler handler)
    {
        var buttonColumn = new DataGridTemplateColumn
        {
            Header = " "
        };

        var dataTemplate = new DataTemplate();

        var buttonFactory = new FrameworkElementFactory(typeof(Button));
        buttonFactory.SetValue(ContentControl.ContentProperty, header);
        buttonFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(10));

        var binding = new Binding
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGrid), 1)
        };
        buttonFactory.SetBinding(ButtonBase.CommandParameterProperty, binding);
        buttonFactory.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(handler));

        dataTemplate.VisualTree = buttonFactory;
        buttonColumn.CellTemplate = dataTemplate;

        return buttonColumn;
    }

    public static T? FindParent<T>(DependencyObject child)
        where T : DependencyObject
    {
        while (true)
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            switch (parentObject)
            {
                case null:
                    return null;
                case T parent:
                    return parent;
                default:
                    child = parentObject;
                    break;
            }
        }
    }

    public static T? FindModel<T>(object? sender)
        where T : class
    {
        if (sender is DependencyObject depObj)
        {
            var parentObject = FindParent<DataGridRow>(depObj);
            if (parentObject?.Item is T model)
            {
                // note: when open a window due `edit` then change any field then close
                // due `close` I'll see that change on the gridview,
                // this makes a problem when sending an object to API for other id.
                var sss = JsonConvert.SerializeObject(model);
                var mmm = JsonConvert.DeserializeObject<T>(sss);
                return mmm;
            }
        }

        return null;
    }

    public static UIElement GetEmptyItemsControl(double minDataGridHeight)
    {
        return new DataGrid
        {
            IsReadOnly = true,
            MinHeight = minDataGridHeight,
            MinWidth = 500,
            Margin = new Thickness(0, 5, 0, 5),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
        };
    }

    public static DataGrid FindDataGrid(StackPanel panel)
    {
        return panel.Children.OfType<DataGrid>().FirstOrDefault() ?? new();
    }

    public static UIElement GetErrorItemsControl(string title, string message,
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
                        new SymbolIcon
                        {
                            Symbol = SymbolRegular.ErrorCircle12,
                            Margin = new Thickness(0, 5, 0, 0)
                        },
                        new TextBlock
                        {
                            Text = title,
                            Margin = new Thickness(10, 0, 0, 0),
                            FontSize = 25,
                            FontWeight = FontWeights.Bold,
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
                    FontWeight = FontWeights.Regular,
                    HorizontalAlignment = HorizontalAlignment.Center
                },
                retryCallback is not null ?
                new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 0),
                    Content = locale["RetryButton"],
                    Command = new RelayCommand(retryCallback)
                } : new FrameworkElement()
            }
        };
    }
}