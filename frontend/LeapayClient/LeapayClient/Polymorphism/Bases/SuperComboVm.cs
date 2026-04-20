using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Fly.Framework.Shared.Common;
using Leapay.Client.ClientModels.Dto.DtoModels;

namespace Leapay.Client.Polymorphism.Bases;

public partial class SuperComboVm<T>: ObservableObject where T: class, new()
{
    [ObservableProperty]
    public partial ObservableCollection<ComboModel> Items { get; set; } = [];

    [ObservableProperty]
    public partial ComboModel? SelectedItem { get; set; }

    public Action<ComboModel>? OnSelectionChanged { get; set; }
    
    public event EventHandler<ComboModel>? SelectionChanged;
    
    public async Task LoadAsync<TLoader>(TLoader loader,
        string? endpoint = null,
        Identifier? def = null,
        Action<ComboModel>? onSelectionChanged = null,
        string? header = null,
        bool insertDefault = true,
        string? defaultText = null) where TLoader: BaseViewModel<T>
    {
        Information("Fetching combo items for {0}:{1}:{2}",
            typeof(TLoader).Name,
            endpoint ?? "def",
            header ?? "none");
        
        Items.Clear();
        
        var items = await loader.RequestComboModelsAsync<ComboModel>(endpoint ?? "", header ?? "");
        SetItemsInternal(items.Data.ToList(), def, onSelectionChanged, insertDefault, defaultText);
    }

    public void Load(List<ComboModel> models,
        Identifier? def = null,
        Action<ComboModel>? onSelectionChanged = null,
        bool insertDefault = true)
    {
        SetItemsInternal(models, def, onSelectionChanged, insertDefault);
    }
    
    private void SetItemsInternal(List<ComboModel> models, Identifier? def = null,
        Action<ComboModel>? onSelectionChanged = null,
        bool insertDefault = true,
        string? defaultText = null)
    {
        Items.Clear();

        foreach (var item in models)
            Items.Add(item);
        
        OnSelectionChanged = onSelectionChanged;
        
        if (def.HasValue)
        {
            SelectedItem = Items.FirstOrDefault(i => i.Value == def.Value.Value.ToString());
        }

        if (insertDefault)
        {
            Items.Insert(0, GetDefaultComboModel(defaultText));
            
            if (SelectedItem is null)
            {
                SelectedItem = Items.FirstOrDefault();
            }
        }
    }
    
    partial void OnSelectedItemChanged(ComboModel? value)
    {
        OnSelectionChanged?.Invoke(value ?? new());
        SelectionChanged?.Invoke(this, value ?? new());
    }
    
    public static ComboModel GetDefaultComboModel(string? text = null, bool loading = false)
    {
        text ??= "...";
        
        return new ComboModel
        {
            Display = text,
            Loading = false,
            Value = Identifier.Empty.ToString()
        };
    }

    public void Reset()
    {
        Items.Clear();
        SelectedItem = null;
    }
    
    public void SelectAt(int index)
    {
        if (index < 0 || index >= Items.Count)
        {
            Warning("Index {0} is out of range for items count {1}.", index, Items.Count);
            return;
        }
        
        SelectedItem = Items[index];
    }

    public bool IsAnySelected() => SelectedItem != null && SelectedItem.HasValue() && SelectedItem?.Value != "...";
    
    public bool IsSelected(ComboModel model) => SelectedItem?.Value == model.Value;
    
    public bool IsSelected(Identifier id) => SelectedItem?.Value == id.ToString();

    private Expression<Identifier>? _boundIdentifier;
    
    [RequiresDynamicCode("Binding with an expression requires dynamic code generation.")]
    [Obsolete("Can not be used with structs. Use the SelectionChanged event instead.")]
    public void BindWith(Expression<Identifier> property)
    {
        _boundIdentifier = property;
    }
}