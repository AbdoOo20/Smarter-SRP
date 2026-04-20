using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Data.Converters;
using Leapay.Client.ClientModels.Dto.DtoModels.FastSales;

namespace Leapay.Client.Converters;

public class ItemsSourceLimiterConverter : IValueConverter
{
    public object? Convert(object values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!int.TryParse(parameter?.ToString(), out var limit))
            return values;
        
        if (values is ObservableCollection<ItemModel> itemsM)
            return new ObservableCollection<ItemModel>(itemsM.Take(limit));
        
        if (values is ObservableCollection<object> itemsObs)
            return itemsObs.Take(limit);
        
        if (values is IEnumerable<object> items)
            return items.Take(limit);
        
        return values;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value;
}