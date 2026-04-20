using System.Collections;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.Interfaces;
using Leapay.Client.Helpers;
using ClosedXML.Excel;
using System.Diagnostics;
using Leapay.Client.Converters;

namespace Leapay.Client.Controls;

public class BindingDataGrid : DataGridBase, IDisposable
{
    private ItemsControl _itemsControl = new();
    private Grid? _highlightedRow;

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<BindingDataGrid, IEnumerable?>(nameof(ItemsSource));

    private VirtualizingStackPanel? _stack = new();
    
    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<BindingDataGrid, bool>(nameof(IsReadOnly), true);

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly StyledProperty<bool> IsCompactProperty =
        AvaloniaProperty.Register<BindingDataGrid, bool>(nameof(IsCompact));

    public bool IsCompact
    {
        get => GetValue(IsCompactProperty);
        set => SetValue(IsCompactProperty, value);
    }
    
    public BindingDataGrid()
    {
        SetItemsHost(_itemsControl);
        Columns.CollectionChanged += (_, _) => UpdateColumns();
        this.GetObservable(ItemsSourceProperty).Subscribe(_ => _itemsControl.ItemsSource = ItemsSource);
    }

    private void UpdateColumns()
    {
        UpdateHeader();

        _itemsControl.ItemsPanel = new FuncTemplate<Panel?>(() => _stack);
        _itemsControl.ItemTemplate = new FuncDataTemplate<object>((_, _) =>
        {
            var itemGrid = new Grid();
            for (var i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                var last = i == Columns.Count - 1;
                var columnWidth = last ? new GridLength(1, GridUnitType.Star) : column.Width;
                itemGrid.ColumnDefinitions.Add(new ColumnDefinition(columnWidth)
                {
                    SharedSizeGroup = last ? null : $"Col{i}"
                });

                var cellWrapper = new Border
                {
                    Padding = new Thickness(column.IsButtonColumn ? 3 : 8),
                    BorderThickness = new Thickness(column.IsButtonColumn ? 0 : 0.8, 0.8),
                    [!Border.BorderBrushProperty] = this[!InnerGridLinesBrushProperty],
                    Child = new ContentControl
                    {
                        ContentTemplate = column.CellTemplate,
                        HorizontalAlignment = last ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        [!ContentProperty] = new Binding()
                    }
                };

                Grid.SetColumn(cellWrapper, i);
                itemGrid.Children.Add(cellWrapper);
            }

            return itemGrid;
        }, true);
    }

    public void HighlightItem(object dto)
    {
        if (_highlightedRow is not null)
        {
            _highlightedRow.Background = Brushes.Transparent;
            _highlightedRow = null;
        }

        if (dto is null)
            return;

        ScrollToItem(dto);

        Dispatcher.UIThread.Post(() =>
        {
            if (ItemsSource is null) return;

            foreach (var item in ItemsSource)
            {
                if (CompareItem(item, dto))
                {
                    if (_itemsControl.ContainerFromItem(item) is ContentPresenter { Child: Grid itemGrid })
                    {
                        itemGrid.Background = ContentHelper.GetLightGreenGradient();
                        _highlightedRow = itemGrid;
                    }
                    break;
                }
            }
        }, DispatcherPriority.Background);
    }

    public void ScrollToItem(object dto)
    {
        if (ItemsSource is null) return;

        foreach (var item in ItemsSource)
        {
            if (CompareItem(item, dto))
            {
                var container = _itemsControl.ContainerFromItem(item);
                container?.BringIntoView();
                break;
            }
        }
    }

    private static bool CompareItem(object item, object dto)
    {
        if (dto is IDetailsModel detailsDto && item is IDetailsModel detailsTemp)
        {
            return detailsDto.TemporaryId == detailsTemp.TemporaryId;
        }

        if (dto is DtoBase dtoBase && item is DtoBase itemBase)
        {
            return dtoBase.Id == itemBase.Id;
        }

        return false;
    }
    
    public void ExportToExcel(string filePath, string exportName)
    {
        if (ItemsSource is null)
        {
            return;
        }
        
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(exportName);

        var allowedColumns = Columns.Where(c => !c.IsButtonColumn).ToList();
        
        for (var i = 0; i < allowedColumns.Count; i++)
        {
            var currCol = allowedColumns.ElementAtOrDefault(i);

            if (currCol is null || currCol.IsButtonColumn)
            {
                continue;
            }

            var header = allowedColumns.ElementAtOrDefault(i)?.Header;
            var valueCol = header is TextBlock text ? text.Text ?? "" : header?.ToString() ?? "";

            worksheet.Cell(1, i + 1).Value = valueCol;
        }

        var rowIndex = 2;
        foreach (var item in ItemsSource)
        {
            var i = 0;

            foreach (var col in allowedColumns)
            {
                object? cellValue = "-";

                var property = item.GetType().GetProperty(col.HeaderId);
                if (property != null)
                {
                    cellValue = property.GetValue(item);
                }

                cellValue = new DataGridBoundValueConverter().Convert(cellValue, null, "-", null);
                var cell = worksheet.Cell(rowIndex, i + 1);
                cell.Value = XLCellValue.FromObject(cellValue);

                i += 1;
            }

            rowIndex++;
        }
        
        var range = worksheet.Range(1, 1, rowIndex - 1, allowedColumns.Count());
        var table = range.CreateTable();
        table.Theme = XLTableTheme.TableStyleMedium16;

        worksheet.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(filePath);

        Process.Start(new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = filePath
        });
    }

    public void Dispose()
    {
        ItemsSource = null;
        _itemsControl.Items.Clear();
        _stack?.Children.Clear();
        _stack = null;
        _itemsControl = null;
        
        GC.Collect();
    }
}
