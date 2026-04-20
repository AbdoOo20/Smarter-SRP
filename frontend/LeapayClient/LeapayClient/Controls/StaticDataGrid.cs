using System.Buffers;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using Leapay.Client.Helpers;

namespace Leapay.Client.Controls;

public class StaticDataGrid : DataGridBase
{
    private readonly ItemsControl _itemsPanel = new();
    
    public StaticDataGrid()
    {
        _itemsPanel.ItemsPanel = new FuncTemplate<Panel?>(() => new VirtualizingStackPanel());

        SetItemsHost(_itemsPanel);
        Columns.CollectionChanged += (_, _) => UpdateHeader();
    }

    public void Populate(IEnumerable<object> items, IEnumerable<object> buttons)
    {
        _itemsPanel.Items.Clear();

        var itemList = items.ToList();
        
        if (!itemList.Any()) return;

        for (var rowIndex = 0; rowIndex < itemList.Count; rowIndex++)
        {
            var item = itemList[rowIndex];
            var itemGrid = new Grid();

            foreach (var column in Columns)
            {
                var columnIndex = Columns.IndexOf(column);
                var isLastColumn = columnIndex == Columns.Count - 1;

                var columnWidth = isLastColumn ? new GridLength(1, GridUnitType.Star) : column.Width;
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition(columnWidth)
                {
                    SharedSizeGroup = isLastColumn ? null : $"Col{columnIndex}"
                });

                Control? cellContent = null;

                if (column.IsButtonColumn)
                {
                    if (column.ConcreteControls.Count() > rowIndex)
                    {
                        cellContent = column.ConcreteControls.ElementAtOrDefault(rowIndex);
                    }
                }
                else
                {
                    cellContent = column.CellTemplate?.Build(item);
                }

                var cellWrapper = new Border
                {
                    Padding = new Thickness(column.IsButtonColumn ? 3 : 5),
                    BorderThickness = new Thickness(0.8),
                    [!Border.BorderBrushProperty] = this[!InnerGridLinesBrushProperty]
                };

                if (cellContent is not null)
                {
                    cellWrapper.Child = new ContentControl
                    {
                        Content = cellContent,
                        HorizontalAlignment = isLastColumn ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                
                    if (!column.IsButtonColumn)
                    {
                        PerformBorderStyling(cellContent, cellWrapper);
                    }
                }

                Grid.SetColumn(cellWrapper, columnIndex);
                itemGrid.Children.Add(cellWrapper);
            }
            _itemsPanel.Items.Add(itemGrid);
        }
    }
    
    private void PerformBorderStyling(Control? cellContent, Border border)
    {
        if (cellContent is not TextBlock textBlock) return;
        if (!textBlock.Text.ContainsAny(SearchValues
                .Create(new ReadOnlySpan<char>(['+', '-', '_'])))) return;
        
        textBlock.ContentSelector(t => t.Text?.StartsWith('+') ?? false,
            t =>
            {
                border.Background = ContentHelper.GetGreenGradient();
                t.Foreground = Brushes.White;
            });
                        
        textBlock.ContentSelector(t => (t.Text?.StartsWith('-') ?? false) && (!t.Text?.Contains("--") ?? false),
            t =>
            {
                border.Background = ContentHelper.GetRedGradient();
                t.Foreground = Brushes.White;
            });
        
        textBlock.ContentSelector(t => t.Text?.StartsWith('_') ?? false,
            t =>
            {
                border.Background = ContentHelper.GetZeroGradient();
                t.Text = t.Text?[1..];
            });
    }
}