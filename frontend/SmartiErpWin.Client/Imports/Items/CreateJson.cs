using System.IO;
using System.Text.Json;
using AutoMapper;
using ClosedXML.Excel;
using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Dtos;
using SmartiErpWin.Client.DtoModels;
using SmartiErpWin.Client.DtoModels.StoresCategoriesModels;
using SmartiErpWin.Client.UserInterface.Stores.Categories;
using SmartiErpWin.Client.UserInterface.Stores.Items;
using SmartiErpWin.Client.UserInterface.Stores.Units;
using SmartiErpWin.Client.ViewModels.Windows;

namespace SmartiErpWin.Client.Imports.Items;

public class CreateJson
{
    public static async Task StartImportAsync()
    {
        /*await CreateUnitsAsync();
        await CreateCategoriesAsync();
        await CreateItemsAsync();*/
    }
    
    public static void CreateJsonFromExcel()
    {
        const string excelFilePath = "D:\\database_import.xlsx";
        const string outPutJsonPath = "D:\\categories.json";
        
        ExtractUnitsOrCategoriesToJson(excelFilePath, outPutJsonPath);
    }
    
    public static void ExtractItemsToJson(string excelPath, string jsonOutputPath)
    {
        var items = new List<ItemInfo>();

        using var workbook = new XLWorkbook(excelPath);
        var worksheet = workbook.Worksheets.First();
        var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // skip header

        foreach (var row in rows)
        {
            var item = new ItemInfo
            {
                ItemNo = row.Cell("B").GetString(),     // item_no
                ItemName = row.Cell("C").GetString(),   // item_name
                TypId = row.Cell("D").GetString(),      // typ_id
                Price = row.Cell("L").GetValue<decimal>(), // price
                Unit1 = row.Cell("O").GetString(),      // unit1
                UnitId = row.Cell("AQ").GetString()     // unitid
            };

            items.Add(item);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(items, options);
        File.WriteAllText(jsonOutputPath, json);
    }
    
    public static void ExtractUnitsOrCategoriesToJson(string excelPath, string jsonOutputPath)
    {
        var units = new List<UnitOrCategoryInfo>();

        using var workbook = new XLWorkbook(excelPath);
        var worksheet = workbook.Worksheets.ElementAtOrDefault(2);
        var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // skip header

        foreach (var row in rows)
        {
            var unit = new UnitOrCategoryInfo
            {
                Id = row.Cell("A").GetString(),           // id
                Number = row.Cell("B").GetString(),       // number
                Description = row.Cell("C").GetString()   // description
            };

            units.Add(unit);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(units, options);
        File.WriteAllText(jsonOutputPath, json);
    }
    
    public class UnitOrCategoryInfo
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
    }
    
    public class ItemInfo
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string TypId { get; set; }
        public decimal Price { get; set; }
        public string Unit1 { get; set; }
        public string UnitId { get; set; }
    }

    public static async Task CreateCategoriesAsync()
    {
        const string outPutJsonPath = "D:\\categories.json";
        var data = await JsonSerializer.DeserializeAsync<List<UnitOrCategoryInfo>>(
            new FileStream(outPutJsonPath, FileMode.Open, FileAccess.Read));
        
        foreach(var item in data)
        {
            var vm = new CategoryPageEntryViewModel(null, App.GetService<LocalisationService>(), new(), EntryType.Add,
                App.GetService<HttpClient>(), typeof(CategoriesPageViewModel));

            vm.Model.Name = item.Description;
            await vm.SaveCommand.ExecuteAsync(null);
        }
    }
    
    public static async Task CreateUnitsAsync()
    {
        const string outPutJsonPath = "D:\\units.json";
        var data = await JsonSerializer.DeserializeAsync<List<UnitOrCategoryInfo>>(
            new FileStream(outPutJsonPath, FileMode.Open, FileAccess.Read));
        
        foreach(var item in data)
        {
            var vm = new CommonPageEntryViewModel<DtoUnit>(null,
                App.GetService<LocalisationService>(),
                new(), EntryType.Add, "Unit",
                App.GetService<HttpClient>(),
                typeof(UnitsPageViewModel));

            vm.Model.Name = item.Description;
            await vm.SaveCommand.ExecuteAsync(null);
        }
    }
    
    public static async Task CreateItemsAsync()
    {
        const string outPutJsonPath = "D:\\items.json";
        var data = await JsonSerializer.DeserializeAsync<List<ItemInfo>>(
            new FileStream(outPutJsonPath, FileMode.Open, FileAccess.Read));
        
        foreach(var item in data)
        {
            var index = data.IndexOf(item) + 1;
            
            Information("Processing item {0} of {1}", index, data.Count);
            
            var vm = new ItemPageEntryViewModel(null,
                App.GetService<LocalisationService>(),
                new(), EntryType.Add,
                App.GetService<HttpClient>(),
                App.GetService<IMapper>(),
                typeof(CategoriesPageViewModel));

            vm.Model.DtoItem.Name = item.ItemName;
            vm.Model.DtoItem.Barcode = item.ItemNo;
            vm.Model.DtoItem.Price = item.Price;
            
            var units = (await vm.RequestComboModelsAsync<ComboModel>("units")).Data;

            var wantedUnit = units.FirstOrDefault(u => u.Display == item.Unit1);

            if (wantedUnit is null)
            {
                Warning("No unit found matching that of {0}", item.ItemName);
                continue;
            }
            
            vm.Model.ItemUnitModels =
            [
                new DtoItemUnitModel
                {
                    UnitId = Identifier.Parse(wantedUnit.Value),
                    UnitName = wantedUnit.Display,
                    IsPrimary = true
                }
            ];
            
            const string outPutJsonPathCats = "D:\\categories.json";
            var dataCats = await JsonSerializer.DeserializeAsync<List<UnitOrCategoryInfo>>(
                new FileStream(outPutJsonPathCats, FileMode.Open, FileAccess.Read));
            
            var cats = (await vm.RequestComboModelsAsync<ComboModel>("gategories")).Data;

            var wantedCat = cats.FirstOrDefault(u => u.Display == dataCats.FirstOrDefault(dataCat => dataCat.Id == item.TypId).Description);
            
            if (wantedCat is null)
            {
                Warning("No category found matching that of {0}", item.ItemName);
                continue;
            }
            
            vm.Model.DtoItem.CategoryId = Identifier.Parse(wantedCat.Value);

            try
            {
                await vm.SaveCommand.ExecuteAsync(null);
            }
            catch (Exception x)
            {
                Error("IMPORT ERROR: {i} {0} - {1}", index, item.ItemName, x.Message);
            }
        }
    }
}