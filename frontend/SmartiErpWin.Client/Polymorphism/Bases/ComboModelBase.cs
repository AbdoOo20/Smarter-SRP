using System.Collections.ObjectModel;
using System.ComponentModel;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using SmartErpWin.Core.Extensions;
using SmartiErpWin.Client.DtoModels;

namespace SmartiErpWin.Client.Polymorphism.Bases;

public partial class ComboModelBase : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> _staticComboItems = [];

    [ObservableProperty]
    private string? _selectedStaticComboItem;

    [ObservableProperty]
    private Dictionary<int, ComboModel?> _selectedApiComboItems = new();

    [ObservableProperty]
    private Dictionary<int, ObservableCollection<ComboModel>> _apiComboItems = new();

    public delegate EventHandler OnApiComboItemChangedEventHandler();

    public event OnApiComboItemChangedEventHandler? OnApiComboItemChanged;

    public delegate EventHandler? OnStaticComboItemChangedEventHandler();

    public event OnStaticComboItemChangedEventHandler? OnStaticComboItemChanged;

    public ComboModelBase()
    {
        ApiComboItems = new();
        SelectedApiComboItems = new();

        for (var i = 0; i < 15; i++)
        {
            SelectedApiComboItems.Add(i, null);
        }

        for (var i = 0; i < 15; i++)
        {
            ApiComboItems.Add(i, new());
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(SelectedApiComboItems):
                OnApiComboItemChanged?.Invoke();
                break;
            case nameof(SelectedStaticComboItem):
                OnStaticComboItemChanged?.Invoke();
                break;
        }
    }

    public static ComboModel GetDefaultComboModel(string? text = null)
    {
        text ??= "...";
        
        return new ComboModel
        {
            Display = text,
            Loading = false,
            Value = string.Empty
        };
    }

    public static LookupOf<string, string> GetDefaultComboLookup()
    {
        return new LookupOf<string, string>
        {
            Display = "...",
            Value = string.Empty
        };
    }

    public static void AddDefaultTo(ObservableCollection<ComboModel> collection)
    {
        collection.Insert(0, GetDefaultComboModel());
    }

    public static void AddDefaultTo(ObservableCollection<LookupOf<string, string>> collection)
    {
        collection.Insert(0, GetDefaultComboLookup());
    }

    public void SetApiComboItems(
        IEnumerable<ComboModel> data,
        int index = 0,
        DtoBase? model = null,
        bool insertDefault = true,
        string? defaultText = null)
    {
        var list = data.ToList();
        if (insertDefault)
        {
            list.Insert(0, GetDefaultComboModel(defaultText));
        }

        Verbose("Inserting default combo model for {@FD} at {0}",
            data.Select(d => d.Display),
            index);

        ApiComboItems[index].Clear();

        // the following prevents accounts and categories from selecting parents as themselves
        if (model is not null)
        {
            list.RemoveAll(m => m.Value == model.Id.ToString());
        }

        foreach (var item in list)
        {
            ApiComboItems[index].Add(item);
        }

        ClearAndChangeByClass(index, list);
    }

    public void SetTemporaryApiComboItem(int index)
    {
        ApiComboItems[index].Clear();

        var comboModelTemp = new ComboModel
        {
            Display = string.Empty,
            Value = string.Empty,
            Loading = true
        };

        ApiComboItems[index].Add(comboModelTemp);
        ClearAndChangeByClass(index, [comboModelTemp]);
        ChangeApiSelectedItem(index, comboModelTemp);
    }

    public void Clear(int index, bool empity = false)
    {
        SetApiCollectionByIndex(index, new());
        if (empity)
            ApiComboItems[index].Clear();
    }

    private void ClearAndChangeByClass(int index, List<ComboModel> models)
    {
        var @new = new ObservableCollection<ComboModel>();

        foreach (var item in models)
        {
            @new.Add(item);
        }

        SetApiCollectionByIndex(index, @new);

        Verbose("Combo models updated at {0}, with data IDs: {@FD}", index,
            models.Select(m => m.Value));
    }

    private ComboModel? GetComboModelOf(Identifier? id, int index)
    {
        var model = ApiComboItems[index].FirstOrDefault(a => a.Value == id.ToString());

        return model;
    }

    public void SetComboModelOrDefault(Identifier? id, int index = 0, bool selectFirstIfValueNotFound = false)
    {
        if ((id == Identifier.Empty || id is null) && selectFirstIfValueNotFound)
        {
            SelectAtOnePoint(index);
            return;
        }
        
        var item = id == Identifier.Empty || id is null ? ApiComboItems[index].FirstOrDefault()
            : GetComboModelOf(id, index);
        SetApiByIndex(index, item);
        ChangeApiSelectedItem(index, item);
    }

    public void SetComboModel(Func<ComboModel, bool> where, int index = 0)
    {
        var oll = GetApiCollectionByIndex(index);
        if (oll is not null)
        {
            var sl = oll.FirstOrDefault(where);
            if (sl is not null)
            {
                SetApiByIndex(index, sl);
            }
        }
    }

    public void SelectAtOnePoint(int apiIndex)
    {
        if (ApiComboItems.Count <= apiIndex) return;
        SelectedApiComboItems[apiIndex] = ApiComboItems[apiIndex].ElementAtOrDefault(1);
        OnPropertyChanged(nameof(SelectedApiComboItems));
    }

    public void SelectAtOnePoint(int apiIndex, int selectionIndex)
    {
        if (ApiComboItems.Count <= apiIndex) return;
        SelectedApiComboItems[apiIndex] = ApiComboItems[apiIndex].ElementAtOrDefault(selectionIndex);
        OnPropertyChanged(nameof(SelectedApiComboItems));
    }

    public void ChangeApiSelectedItem(int index, ComboModel? item)
    {
        SelectedApiComboItems[index] = item;
        OnPropertyChanged(nameof(SelectedApiComboItems));
    }

    public void SetStaticComboBoxItem(string item)
    {
        SelectedStaticComboItem = item;
    }

    public void SetStaticComboBoxItemIndexInternal(int item)
    {
        SelectedStaticComboItem = ConversionKit.Values.ElementAtOrDefault(item);
    }

    public void SetStaticComboBoxItemEnum(object obj)
    {
        SelectedStaticComboItem = ConvertToDisplay((int)obj);
    }

    public Dictionary<int, string?> ConversionKit { get; set; } = new();

    public string? ConvertToDisplay(int api)
    {
        var display = ConversionKit.Count == 0
            ? string.Empty
            : ConversionKit[api];
        return display;
    }

    public TCt ConvertToEnum<TCt>(string? display)
        where TCt : struct, Enum
    {
        var cast = ConversionKit.Count == 0 ? 0 : ConversionKit.Keys
            .FirstOrDefault(k => ConversionKit[k] == display);

        if (!Enum.IsDefined(typeof(TCt), cast))
            return default;

        var type = (TCt)(object)cast;
        return type;
    }

    public TCt ConvertCurrentToEnum<TCt>()
        where TCt : struct, Enum
    {
        return ConvertToEnum<TCt>(SelectedStaticComboItem);
    }

    public void SetStaticItemsInternal<TEnum>(LocalisationService locale) where TEnum : Enum
    {
        ConversionKit = Enum.GetValues(typeof(TEnum)).Cast<object?>()
            .ToDictionary(Convert.ToInt32, item => locale[item.ToString()].ToString());

        var localisedValues = ConversionKit.Values.ToArray();

        Verbose("Conversion kit for static items set: {@0}", ConversionKit);

        AddStaticComboItems(localisedValues);
    }

    public Identifier GetSelectedItemId(int i = 0)
    {
        var id = SelectedApiComboItems.ElementAtOrDefault(i).Value;

        return Identifier.Parse(id?.Value);
    }

    public string GetSelectedItemDisplay(int i = 0)
    {
        return SelectedApiComboItems.ElementAtOrDefault(i).Value?.Display ?? "COMBO_ITEM_NOT_FOUND";
    }

    public bool AssertThatStaticEquals(string display)
        => SelectedStaticComboItem == display;

    public bool AssertThatStaticIsAt(int i = 0)
        => ConversionKit?[i] == SelectedStaticComboItem;

    public int GetStaticPosition()
    {
        return ConversionKit.Values.ToList().IndexOf(SelectedStaticComboItem);
    }

    public bool AssertThatApiIsSelected(int i = 0)
        => !string.IsNullOrEmpty(SelectedApiComboItems[i]?.Value);

    public void AddStaticComboItems(params string[] items)
    {
        StaticComboItems.Clear();

        foreach (var item in items)
        {
            StaticComboItems.Add(item);
        }
    }

    [ObservableProperty]
    private ComboModel _api1;

    [ObservableProperty]
    private ComboModel _api2;

    [ObservableProperty]
    private ComboModel _api3;

    [ObservableProperty]
    private ComboModel _api4;

    [ObservableProperty]
    private ComboModel _api5;

    [ObservableProperty]
    private ComboModel _api6;

    [ObservableProperty]
    private ComboModel _api7;

    [ObservableProperty]
    private ComboModel _api8;

    [ObservableProperty]
    private ComboModel _api9;

    [ObservableProperty]
    private ComboModel _api10;

    [ObservableProperty]
    private ComboModel _api11;

    [ObservableProperty]
    private ComboModel _api12;

    [ObservableProperty]
    private ComboModel _api13;

    [ObservableProperty]
    private ComboModel _api14;

    [ObservableProperty]
    private ComboModel _api15;

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol1 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol2 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol3 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol4 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol5 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol6 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol7 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol8 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol9 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol10 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol11 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol12 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol13 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol14 = new();

    [ObservableProperty]
    private ObservableCollection<ComboModel> _apiCol15 = new();

    public void SetApiByIndex(int index, ComboModel value)
    {
        index++;

        switch (index)
        {
            case 1:
                Api1 = value;
                break;
            case 2:
                Api2 = value;
                break;
            case 3:
                Api3 = value;
                break;
            case 4:
                Api4 = value;
                break;
            case 5:
                Api5 = value;
                break;
            case 6:
                Api6 = value;
                break;
            case 7:
                Api7 = value;
                break;
            case 8:
                Api8 = value;
                break;
            case 9:
                Api9 = value;
                break;
            case 10:
                Api10 = value;
                break;
            case 11:
                Api11 = value;
                break;
            case 12:
                Api12 = value;
                break;
            case 13:
                Api13 = value;
                break;
            case 14:
                Api14 = value;
                break;
            case 15:
                Api15 = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 1 and 15");
        }

        OnApiComboItemChanged?.Invoke();
    }

    public ComboModel GetApiByIndex(int index)
    {
        index++;

        switch (index)
        {
            case 1:
                return Api1;
            case 2:
                return Api2;

            case 3:
                return Api3;

            case 4:
                return Api4;

            case 5:
                return Api5;

            case 6:
                return Api6;

            case 7:
                return Api7;

            case 8:
                return Api8;

            case 9:
                return Api9;

            case 10:
                return Api10;

            case 11:
                return Api11;

            case 12:
                return Api12;

            case 13:
                return Api13;

            case 14:
                return Api14;

            case 15:
                return Api15;

            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 1 and 15");
        }
    }

    public void SetApiCollectionByIndex(int index, ObservableCollection<ComboModel> collection)
    {
        index++;

        switch (index)
        {
            case 1:
                ApiCol1.Clear();

                foreach (var item in collection)
                {
                    ApiCol1.Add(item);
                }
                break;
            case 2:
                ApiCol2 = collection;
                break;
            case 3:
                ApiCol3 = collection;
                break;
            case 4:
                ApiCol4 = collection;
                break;
            case 5:
                collection.Observe(ApiCol5);
                OnPropertyChanged(nameof(ApiCol5));
                break;
            case 6:
                ApiCol6.Clear();
                foreach (var item in collection)
                {
                    ApiCol6.Add(item);
                }
                break;
            case 7:
                ApiCol7 = collection;
                break;
            case 8:
                ApiCol8 = collection;
                break;
            case 9:
                ApiCol9 = collection;
                break;
            case 10:
                ApiCol10 = collection;
                break;
            case 11:
                ApiCol11 = collection;
                break;
            case 12:
                ApiCol12 = collection;
                break;
            case 13:
                ApiCol13 = collection;
                break;
            case 14:
                ApiCol14 = collection;
                break;
            case 15:
                ApiCol15 = collection;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 1 and 15");
        }
    }

    public ObservableCollection<ComboModel> GetApiCollectionByIndex(int index)
    {
        index++;

        switch (index)
        {
            case 1:
                return ApiCol1;
            case 2:
                return ApiCol2;
            case 3:
                return ApiCol3;
            case 4:
                return ApiCol4;
            case 5:
                return ApiCol5;
            case 6:
                return ApiCol6;
            case 7:
                return ApiCol7;
            case 8:
                return ApiCol8;
            case 9:
                return ApiCol9;
            case 10:
                return ApiCol10;
            case 11:
                return ApiCol11;
            case 12:
                return ApiCol12;
            case 13:
                return ApiCol13;
            case 14:
                return ApiCol14;
            case 15:
                return ApiCol15;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 1 and 15");
        }
    }
}

