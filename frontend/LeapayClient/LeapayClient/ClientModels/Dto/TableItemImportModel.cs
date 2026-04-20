using System.Collections.ObjectModel;
using Avalonia.Threading;
using Leapay.Client.Services;

namespace Leapay.Client.ClientModels.Dto;

public class TableItemImportModel<T> where T: class, new()
{
    public TableItemImportModel(int index)
    {
        Index = index;
    }

    public T? Item { get; set; }
    
    public string? ImportError { get; set; }
    
    public int Index { get; set; }
    
    public string? ProcessError { get; set; }
    
    public bool IsDuplicated { get; set; }
    
    public Func<CancellationToken, Task<string?>>? ProcessLogic { get; set; }
    
    public async Task ImportAsync(ObservableCollection<ColumnImportModel> columns)
    {
        Item = new();

        foreach (var column in columns.OrderBy(c => c.ProcessOrder))
        {
            var propVal = column.CurrentValues.ElementAtOrDefault(Index);

            if (column.HasForeignKey)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    column.ForeignKeyMatchingMechanism?.Invoke((Item, propVal));
                });

                continue;
            }
            
            // Since the column is related to the properties of the item itself,
            // let's check them.
            
            var property = Item.GetType().GetProperty(column.DbName);

            if (property == null)
                continue;

            if (property.PropertyType == typeof(bool))
            {
                bool final = false;

                var locale = App.GetService<LocalisationService>();
                
                if (bool.TryParse(propVal, out var boolVal))
                    final = boolVal;
                else if(propVal == locale["Yes"] || propVal == locale["No"])
                {
                    final = propVal == locale["Yes"];
                }
                else if (propVal is "Yes" or "No")
                {
                    final = propVal == "Yes";
                }
                
                Item?.GetType().GetProperty(property.Name)?.SetValue(Item, final);
            }
            else if (property.PropertyType.IsEnum)
            {
                var locale = App.GetService<LocalisationService>();
                var reverseDictionary = locale.GetCurrentLocale();
                var match = reverseDictionary.FirstOrDefault(x => x.Value == propVal);

                if (Enum.TryParse(property.PropertyType, match.Key, ignoreCase: true, out var parsed))
                {
                    Item?.GetType()
                        .GetProperty(property.Name)?
                        .SetValue(Item, parsed);
                }
            }
            else if (property.PropertyType == typeof(decimal))
            {
                decimal final = 0;
                
                if (decimal.TryParse(propVal, out var deci))
                    final = deci;
                
                Item?.GetType().GetProperty(property.Name)?.SetValue(Item, final);
            }
            else
            {
                Item?.GetType().GetProperty(property.Name)?.SetValue(Item, propVal ?? column.DefaultValue);
            }
        }
        
        /*foreach (var property in typeof(T).GetProperties())
        {
            var associatedColumn = columns.FirstOrDefault(c => c.DbName == property.Name);
            if (associatedColumn is null)
                continue;


        }*/
    }
}