using Avalonia.Collections;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace Leapay.Client.Controls;

public abstract class DataGridBase : UserControl
{
    protected Grid _mainGrid;
    protected Grid _headerGrid;

    public static readonly StyledProperty<ScrollBarVisibility> HorizontalScrollBarVisibilityProperty =
        AvaloniaProperty.Register<DataGridBase, ScrollBarVisibility>(nameof(HorizontalScrollBarVisibility), ScrollBarVisibility.Auto);

    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        get => GetValue(HorizontalScrollBarVisibilityProperty);
        set => SetValue(HorizontalScrollBarVisibilityProperty, value);
    }

    public static readonly StyledProperty<IBrush?> GridLinesBrushProperty =
        AvaloniaProperty.Register<DataGridBase, IBrush?>(nameof(GridLinesBrush), Brushes.LightGray);

    public static readonly StyledProperty<IBrush?> InnerGridLinesBrushProperty =
        AvaloniaProperty.Register<DataGridBase, IBrush?>(nameof(GridLinesBrush), Brushes.LightGray);

    public IBrush? GridLinesBrush
    {
        get => GetValue(GridLinesBrushProperty);
        set => SetValue(GridLinesBrushProperty, value);
    }

    public IBrush? InnerGridLinesBrush
    {
        get => GetValue(InnerGridLinesBrushProperty);
        set => SetValue(InnerGridLinesBrushProperty, value);
    }
    
    public AvaloniaList<SimpleDataGridColumn> Columns { get; } = new();

    protected Control ItemsHost { get; private set; }
    
    protected void SetItemsHost(Control itemsHost)
    {
        ItemsHost = itemsHost;
        
        _mainGrid = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*"),
        };
        Grid.SetIsSharedSizeScope(_mainGrid, true);

        _headerGrid = new Grid();
        var headerWrapper = new Border
        {
            [!Border.BorderBrushProperty] = this[!GridLinesBrushProperty],
            Child = _headerGrid
        };
        Grid.SetRow(headerWrapper, 0);

        var scrollViewer = new ScrollViewer
        {
            HorizontalScrollBarVisibility = HorizontalScrollBarVisibility,
            Content = _mainGrid
        };
        
        Grid.SetRow(ItemsHost, 1);

        _mainGrid.Children.Add(headerWrapper);
        _mainGrid.Children.Add(ItemsHost);

        var outerBorder = new Border
        {
            [!Border.BorderBrushProperty] = this[!GridLinesBrushProperty],
            Child = scrollViewer
        };
        Content = outerBorder;

        this.GetObservable(HorizontalScrollBarVisibilityProperty).Subscribe(v => scrollViewer.HorizontalScrollBarVisibility = v);
    }
    
    protected void UpdateHeader()
    {
        _headerGrid.ColumnDefinitions.Clear();
        _headerGrid.Children.Clear();

        for (var i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            var last = i == Columns.Count - 1;

            var columnWidth = last ? new GridLength(1, GridUnitType.Star) : column.Width;
            var def = new ColumnDefinition(columnWidth)
            {
                SharedSizeGroup = last ? null : $"Col{i}"
            };
            _headerGrid.ColumnDefinitions.Add(def);

            var headerCellWrapper = new Border
            {
                Padding = new Thickness(8),
                BorderThickness = new Thickness(0, 0, 0, 3),
                [!Border.BorderBrushProperty] = this[!GridLinesBrushProperty],
                Child = new ContentPresenter
                {
                    Content = column.Header,
                    HorizontalAlignment = last ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeight.Bold
                }
            };

            Grid.SetColumn(headerCellWrapper, i);
            _headerGrid.Children.Add(headerCellWrapper);
        }
    }
}