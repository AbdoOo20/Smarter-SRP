using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace Avalonia.VirtualizingWrapPanel;

internal class ItemContainerManagerItemsChangedEventArgs
{
    public NotifyCollectionChangedAction Action { get; }

    public ItemContainerManagerItemsChangedEventArgs(NotifyCollectionChangedAction action)
    {
        Action = action;
    }
}

internal class ItemContainerManager
{
    public event EventHandler<ItemContainerManagerItemsChangedEventArgs>? ItemsChanged;

    public bool IsRecycling { get; set; }

    public ReadOnlyCollection<object> Items => _itemContainerGenerator.Items;

    public IReadOnlyDictionary<object, IControl> RealizedContainers => _realizedContainers;

    private readonly Dictionary<object, IControl> _realizedContainers = new();

    private readonly ItemContainerGenerator _itemContainerGenerator;

    public ItemContainerManager(ItemContainerGenerator itemContainerGenerator)
    {
        _itemContainerGenerator = itemContainerGenerator;
        _itemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
    }

    public void OnClearChildren()
    {
        _realizedContainers.Clear();
    }

    public IControl Realize(int itemIndex)
    {
        var item = Items[itemIndex];

        if (_realizedContainers.TryGetValue(item, out var existingContainer))
        {
            return existingContainer;
        }

        var containerInfo = _itemContainerGenerator.Materialize(itemIndex, 0, 0);
        var container = containerInfo.ContainerControl;

        _realizedContainers.Add(item, container);

        if (!containerInfo.IsNewlyRealized)
        {
            InvalidateMeasureRecursively(container);
        }

        return container;
    }

    public void Virtualize(object item)
    {
        var container = _realizedContainers[item];
        var itemIndex = _itemContainerGenerator.IndexFromContainer(container);

        if (itemIndex != -1)
        {
             _itemContainerGenerator.Dematerialize(itemIndex, 1, 0);
        }

        _realizedContainers.Remove(item);
    }

    private void ItemContainerGenerator_ItemsChanged(object? sender, ItemContainerEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            _realizedContainers.Clear();
        }
            
        ItemsChanged?.Invoke(this, new ItemContainerManagerItemsChangedEventArgs(e.Action));        
    }

    private static void InvalidateMeasureRecursively(IControl element)
    {
        element.InvalidateMeasure();
        foreach (var child in element.GetVisualChildren().OfType<IControl>())
        {
            InvalidateMeasureRecursively(child);
        }
    }
}
