namespace Leapay.Client.Behaviours;

using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System.Collections;
using System.Collections.Specialized;

public class MultiSelectBehaviour : Behavior<ListBox>
{
    public static readonly AvaloniaProperty SelectedItemsProperty = AvaloniaProperty.Register<MultiSelectBehaviour, IList>(
        nameof(SelectedItems));

    public IList SelectedItems
    {
        get => (IList)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    private bool _isUpdatingTarget;
    private bool _isUpdatingSource;

    protected override void OnLoaded()
    {
        base.OnLoaded();
        
        if (AssociatedObject is null || SelectedItems is null)
            return;

        AssociatedObject.SelectedItems.Clear();
        foreach (var item in SelectedItems)
        {
            AssociatedObject.SelectedItems.Add(item);
        }
        
        AssociatedObject.SelectionChanged += ListBoxSelectionChanged;

        if (SelectedItems is INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged += SourceCollectionChanged;
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is null)
            return;

        AssociatedObject.SelectionChanged -= ListBoxSelectionChanged;

        if (SelectedItems is INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged -= SourceCollectionChanged;
        }
    }

    private void SourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_isUpdatingSource || AssociatedObject == null)
            return;

        try
        {
            _isUpdatingTarget = true;

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                AssociatedObject.SelectedItems.Clear();
                return;
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                    AssociatedObject.SelectedItems.Remove(item);
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    AssociatedObject.SelectedItems.Add(item);
            }
        }
        finally
        {
            _isUpdatingTarget = false;
        }
    }

    private void ListBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingTarget || SelectedItems == null)
            return;

        try
        {
            _isUpdatingSource = true;

            foreach (var item in e.RemovedItems)
                SelectedItems.Remove(item);

            foreach (var item in e.AddedItems)
                SelectedItems.Add(item);
        }
        finally
        {
            _isUpdatingSource = false;
        }
    }
}
