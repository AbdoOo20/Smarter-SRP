using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using ClosedXML.Excel;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using Avalonia.Threading;
using Fly.Framework.Shared.Common.Dtos;
using Leapay.Client.Helpers;
using Leapay.Client.Polymorphism.Interfaces;
using SmartErpWin.Core;
using SukiUI.ColorTheme;
using SukiUI.Content;

namespace Leapay.Client.ClientModels.Dto;

public partial class TableImportModel<T> : BaseRefreshableEntryViewModel<T> where T : class, new()
{
    public TableImportModel()
    {
        CancelCommandText = this["Cancel"];
        ClearState();
    }
    
    private CancellationTokenSource? _cancellationTokenSource;
    
    [ObservableProperty]
    public partial string? TableName { get; set; }
    
    [ObservableProperty]
    public partial string? ColumnsState { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<ColumnImportModel> Columns { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<ForeignColumnImportModel> ForeignColumns { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<TableItemImportModel<T>> Items { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ItemImportErrorModel> Errors { get; set; } = [];
    
    [ObservableProperty]
    public partial ObservableCollection<ItemImportErrorModel> Duplicates { get; set; } = [];
    
    [ObservableProperty]
    public partial double ProgressPercentage { get; set; }
    
    [ObservableProperty]
    public partial int ProcessedCount { get; set; }
    
    [ObservableProperty]
    public partial bool CanProcess { get; set; }
    
    [ObservableProperty]
    public partial bool IsProcessing { get; set; }
    
    [ObservableProperty]
    public partial bool ErrorsVisible { get; set; }
    
    [ObservableProperty]
    public partial bool DuplicatesVisible { get; set; }
    
    [ObservableProperty]
    public partial bool ProcessErrorsVisible { get; set; }

    [ObservableProperty]
    public partial bool SkipDuplicates { get; set; } = true;
    
    [ObservableProperty]
    public partial string ProblemsState { get; set; }
    
    [ObservableProperty, NotifyPropertyChangedFor(nameof(ShowNormalUi))]
    public partial bool ShowProcessingUi { get; set; }
    
    [ObservableProperty]
    public partial bool CancelVisible { get; set; }
    
    [ObservableProperty]
    public partial bool ShowProgressCircle { get; set; }

    [ObservableProperty]
    public partial IBrush? RedBackground { get; set; } = ContentHelper.GetRedGradient();
    
    [ObservableProperty]
    public partial IBrush? YellowBackground { get; set; } = ContentHelper.GetWarningYellowGradient();
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFileChosen))]
    public partial bool IsFileChosen { get; set; }
    
    [ObservableProperty]
    public partial string CancelCommandText { get; set; }
    
    [ObservableProperty]
    public partial string CancelParameter { get; set; }
    
    [ObservableProperty]
    public partial string? FileChosenText { get; set; }
    
    [ObservableProperty]
    public partial bool IsIndeterminate { get; set; }

    [ObservableProperty]
    public partial List<ISynchronisationAware> SynchronisationInstances { get; set; } = [];
    
    [ObservableProperty]
    public partial Type RefreshPage { get; set; }
    
    [ObservableProperty]
    public partial LinearGradientBrush SuccessBackground { get; set; } = NotificationColor.SuccessIconForeground;
    
    [ObservableProperty]
    public partial Geometry SuccessIcon { get; set; } = Icons.Check;
    
    public NotificationType FileNotification => IsFileChosen ? NotificationType.Success : NotificationType.Information;

    public bool ShowNormalUi => !ShowProcessingUi;
    
    [ObservableProperty]
    public partial string? InfoText { get; set; }
    
    [ObservableProperty]
    public partial string? CurrentFilePath { get; set; }
    
    [ObservableProperty]
    public partial string? CurrentFileName { get; set; }
    
    public Func<T?, (string, bool)>? ItemValidationMechanism { get; set; }
    
    public Func<T?, (string, bool)>? ItemDuplicateValidationMechanism { get; set; }
    
    public Func<T?, CancellationToken, Task<(string, bool)>>? ItemProcessMechanism { get; set; }
    
    [RelayCommand]
    private async Task ImportFileAsync()
    {
        if (WindowLock is null)
        {
            Warning("WindowLock cannot be null while importing Excel.");
            return;
        }
        
        var doc = await WindowLock.StorageProvider.TryGetFolderFromPathAsync(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        var options = new FilePickerOpenOptions
        {
            SuggestedStartLocation = doc,
            FileTypeFilter = [ new FilePickerFileType(".xlsx") ] 
        };

        var picker = await WindowLock.StorageProvider.OpenFilePickerAsync(options);

        if (!picker.Any())
        {
            _ = DialogueServices.ShowErrorAsync("PleaseSelectExcelFile");
            return;
        }

        var storageFile = picker[0];
        var file = storageFile.TryGetLocalPath();
        
        if (string.IsNullOrEmpty(file) || string.IsNullOrWhiteSpace(file) || !file.EndsWith(".xlsx"))
        {
            _ = DialogueServices.ShowErrorAsync("PleaseSelectExcelFile");
            return;
        }

        CurrentFilePath = file;
        CurrentFileName = storageFile.Name;
        
        await StartImportLogicAsync(file, storageFile.Name);
    }

    public override async Task OnSynchronisationRequestedAsync(params object[] args)
    {
        if (string.IsNullOrEmpty(CurrentFilePath) || string.IsNullOrEmpty(CurrentFileName))
            return;

        if (ShowProcessingUi)
            return;
        
        await StartImportLogicAsync(CurrentFilePath, CurrentFileName);
    }

    private async Task StartImportLogicAsync(string file, string storageFile)
    {
        try
        {
            ShowProcessingUi = true;
            ShowProgressCircle = true;
            CanProcess = false;
            ClearState();
            IsIndeterminate = true;
            CancelVisible = false;
            InfoText = this["ReadingFile"];

            var result = await Task.Run(async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    foreach (var sync in SynchronisationInstances)
                    {
                        Information("Invoking import refresh");
                        await sync.OnSynchronisationRequestedAsync(CodeBook.IsNew);
                    }
                });

                Information("Starting import read");
                var processResult = new FileProcessResult();
                try
                {
                    using var workbook = new XLWorkbook(file);
                    var worksheet = workbook.Worksheets.FirstOrDefault();
                    if (worksheet is null)
                        throw new InvalidOperationException("NoWorksheetFound");

                    var headerRow = worksheet.Row(1);
                    var columnMap = new Dictionary<int, ColumnImportModel>();

                    foreach (var columnImportModel in Columns)
                    {
                        processResult.ColumnValues[columnImportModel] = new List<string>();
                    }

                    foreach (var cell in headerRow.Cells())
                    {
                        var columnName = cell.GetString();
                        var matchedColumn = Columns.FirstOrDefault(c => c.Name == columnName);
                        if (matchedColumn != null)
                        {
                            processResult.AvailableColumns.Add(matchedColumn);
                            columnMap[cell.Address.ColumnNumber] = matchedColumn;
                        }
                    }

                    var lastRowUsed = worksheet.LastRowUsed()?.RowNumber() ?? 1;
                    for (var i = 2; i <= lastRowUsed; i++)
                    {
                        var row = worksheet.Row(i);
                        var itemIndex = i - 2;
                        var item = new TableItemImportModel<T>(itemIndex);
                        processResult.Items.Add(item);

                        foreach (var (colNumber, colModel) in columnMap)
                        {
                            processResult.ColumnValues[colModel].Add(row.Cell(colNumber).GetString());
                        }
                    }

                    foreach (var column in processResult.AvailableColumns)
                    {
                        column.IsAvailable = true;
                        column.CurrentValues.Clear();
                        foreach(var value in processResult.ColumnValues[column])
                        {
                            column.CurrentValues.Add(value);
                        }

                        Columns[Columns.IndexOf(Columns.FirstOrDefault(c => c.DbName == column.DbName))] = column;
                    }
                    
                    foreach (var item in processResult.Items)
                    {
                        await item.ImportAsync(Columns);
                    }
                    
                    if (ItemValidationMechanism is not null)
                    {
                        for (var index = 0; index < processResult.Items.Count; index++)
                        {
                            var itemModel = processResult.Items[index];
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                var validationResult = ItemValidationMechanism(itemModel.Item);
                                if (!validationResult.Item2)
                                {
                                    processResult.Errors.Add(new ItemImportErrorModel
                                    {
                                        Header = GetItemName(itemModel, index),
                                        Error = validationResult.Item1
                                    });
                                }
                            });
                        }
                    }
                    
                    if (ItemDuplicateValidationMechanism is not null)
                    {
                        for (var index = 0; index < processResult.Items.Count; index++)
                        {
                            var itemModel = processResult.Items[index];
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                var validationResult = ItemDuplicateValidationMechanism(itemModel.Item);
                                if (!validationResult.Item2)
                                {
                                    processResult.Duplicates.Add(new ItemImportErrorModel
                                    {
                                        Header = GetItemName(itemModel, index),
                                        Error = validationResult.Item1
                                    });

                                    itemModel.IsDuplicated = true;
                                }
                            });
                        }
                    }

                    processResult.ForeignColumns = PopulateForeignColumns(processResult.AvailableColumns);
                    processResult.Success = true;
                    return processResult;
                }
                catch (Exception e)
                {
                    processResult.Success = false;
                    processResult.Exception = e;
                    return processResult;
                }
            });

            if (!result.Success)
            {
                if (result.Exception is InvalidOperationException { Message: "NoWorksheetFound" })
                {
                    _ = DialogueServices.ShowErrorAsync("NoWorksheetFound");
                }
                else
                {
                    ShowUserException(result.Exception);
                }
                return;
            }
            
            // Clear again in case of a race condition
            ForeignColumns.Clear();
            Items.Clear();
            Errors.Clear();
            ForeignColumns.Clear();
            Errors.Clear();
            Duplicates.Clear();
            
            foreach (var item in result.Items) Items.Add(item);
            foreach(var foreign in result.ForeignColumns) ForeignColumns.Add(foreign);
            foreach (var error in result.Errors) Errors.Add(error);
            foreach (var dup in result.Duplicates) Duplicates.Add(dup);
            
            SetColumnState();
            IsFileChosen = true;
            FileChosenText = this["FileChosen"] + $" {storageFile}";
        }
        catch (Exception exception)
        {
            ShowUserException(exception);
        }
        finally
        {
            ShowProcessingUi = false;
            CanProcess = Columns.All(c => c.IsReady) &&
                         ForeignColumns.All(c => c.ForeignColumns.All(a => a.IsReady))
                         && !Errors.Any();
            InfoText = "";
            ErrorsVisible = Errors.Any();
            DuplicatesVisible = Duplicates.Any();
            ProblemsState = ErrorsVisible ? this["SomeProblemsExist"] : "";
        }
    }

    private static void ShowUserException(Exception exception)
    {
        Error(exception.ToString());
        if (exception is IOException)
        {
            _ = DialogueServices.ShowErrorAsync("CheckFileHasAccessAndClosed");
            return;
        }
        _ = DialogueServices.ShowContactSupportMessageAsync();
    }

    private void ClearState(bool clearErrors = true)
    {
        Items.Clear();
        Items = new();
        
        ForeignColumns.Clear();
        
        foreach (var column in Columns)
        {
            column.CurrentValues.Clear();
            column.CurrentValues = new();
            column.IsAvailable = false;
        }

        if (clearErrors)
        {
            Errors.Clear();
            Errors = new();
        }
        
        Duplicates.Clear();
        Duplicates = new();
        ErrorsVisible = false;
        DuplicatesVisible = false;
        IsFileChosen = false;
        FileChosenText = this["NoFileChosen"];
        ColumnsState = this["CheckThatRequiredColumnsAvailable"];
        CanProcess = false;
        ProcessedCount = 0;
        
        GC.Collect();
    }

    private void SetColumnState()
    {
        if (Columns.All(c => c.IsAvailable))
        {
            ColumnsState = this["AllColumnsAvailable"];
        }
        else if (Columns.All(c => c.IsReady))
        {
            ColumnsState = this["AllRequiredColumnsAvailable"];
        }
        else if (Columns.Any(c => !c.IsReady))
        {
            ColumnsState = this["SomeRequiredColumnsNotAvailable"];
        }
    }

    [RelayCommand]
    private async Task ProcessAsync()
    {
        if (ItemProcessMechanism is null)
        {
            _ = DialogueServices.ShowErrorAsync("DEV ERROR CODE: No process mechanism provided.");
            return;
        }
        
        var finalItems = SkipDuplicates ? Items.Where(i => !i.IsDuplicated) : Items;
        var items = finalItems as TableItemImportModel<T>[] ?? finalItems.ToArray();

        if (items.Length == 0)
        {
            _ = DialogueServices.ShowErrorAsync(this["NoItemsToImportShowReason"]);
            return;
        }

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        

        IsProcessing = true;
        IsIndeterminate = true;
        InfoText = this["ReadingFile"];
        ShowProcessingUi = true;
        ShowProgressCircle = true;
        CancelVisible = true;
        CancelParameter = "cancel";
        CancelCommandText = this["Cancel"];
        
        IsIndeterminate = false;
        ProgressPercentage = 0;
        ProblemsState = this["ProblemsWhileSaving"];
        
        var errorCount = 0;

        for (var index = 0; index < items.Length; index++)
        {
            if (token.IsCancellationRequested)
            {
                InfoText = this["OperationCancelled"];
                Debug("Import cancelled.");
                IsProcessing = false;
                return;
            }
            
            var itemModel = items[index];
            InfoText = this["ImportInProgress"] + " " + GetItemName(itemModel, index);

            var processResult = await Task.Run(async () => {
                try
                {
                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var res = await ItemProcessMechanism(itemModel.Item, token);
                        return new ItemProcessResult { Success = true, Result = res };
                    });
                }
                catch (Exception e)
                {
                    return new ItemProcessResult { Success = false, Exception = e };
                }

                return new ItemProcessResult { Success = true, Result = ("", true) };
            }, token);

            if (processResult.Success)
            {
                if (!processResult.Result.Item2)
                {
                    Errors.Add(new ItemImportErrorModel
                    {
                        Header = (itemModel.Item as DtoNamesBase)?.Name ?? (index + 1).ToString(),
                        Error = processResult.Result.Item1
                    });
                    errorCount++;
                }
            }
            else
            {
                Error("Error importing an item: " + processResult.Exception);
                Errors.Add(new ItemImportErrorModel
                {
                    Header = GetItemName(itemModel, index),
                    Error = processResult.Exception.Message
                });
                errorCount++;
            }
            
            ProgressPercentage = Math.Round((double)(index + 1) / Items.Count * 100);
            
            if (errorCount > 0)
                ProcessErrorsVisible = true;
        }

        InfoText = $"{this["FinishedImportProcess"]}\n{this["ItemImportSuccess"]}: {items.Length - errorCount} | {this["ItemImportFail"]}: {errorCount}";
        CancelCommandText = this["GoBack"];
        CancelParameter = "back";
        ShowProgressCircle = false;
        SynchroniseTypeBase(RefreshPage);
        
        ClearState(false);
    }

    private static string GetItemName(TableItemImportModel<T> itemModel, int index)
    {
        if (itemModel.Item is not null)
        {
            if (itemModel.Item is DtoNamesBase namesBase)
                return namesBase.Name;

            var props = itemModel.Item.GetType().GetProperties()
                .FirstOrDefault(c => c.Name == "Name");
            
            var itemValue = props?.GetValue(itemModel.Item);

            if (itemValue is string str && !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                return str;
        }

        return (index + 1).ToString();
    }

    [RelayCommand]
    private async Task OnCancelAsync()
    {
        if (CancelParameter is "cancel")
        {
            if(!await DialogueServices.ConfirmAsync("Cancel", "CancelConfirm"))
                return;

            CancelVisible = false;
            await _cancellationTokenSource?.CancelAsync();
            IsProcessing = false;
            InfoText = this["OperationCancelled"];
            ShowProgressCircle = false;
            ProcessErrorsVisible = false;

            await Task.Delay(3000);
        }
        
        ClearState();
        ProcessErrorsVisible = false;
        ShowProcessingUi = false; 
    }

    public override void Dispose()
    {
        ClearState();
        base.Dispose();
    }

    private List<ForeignColumnImportModel> PopulateForeignColumns(List<ColumnImportModel> columns)
    {
        var list = new List<ForeignColumnImportModel>();
        
        foreach (var column in columns.Where(c => c is { HasForeignKey: true, IsReady: true, ShowInUi: true }))
        {
            if (column.ForeignKeySatisfactionMechanism is null)
            {
                Debug("No foreign matching mechanism was provided to populate foreign column '{0}' in import. Skipping.", column.DbName);
                return [];
            }

            var foreignColumnImportModel = new ForeignColumnImportModel
            {
                Header = column.Name + " - ",
                ForeignColumns = new ObservableCollection<ColumnImportModel>(column.CurrentValues
                    .Distinct()
                    .Select(distinctItem =>
                        new ColumnImportModel
                        {
                            Name = distinctItem,
                            IsAvailable = column.ForeignKeySatisfactionMechanism(distinctItem),
                            IsRequired = column.IsRequired,
                            ItemAdditionMechanism = column.ItemAdditionMechanism
                        }).OrderBy(c => c.IsReady))
            };

            if (!foreignColumnImportModel.ForeignColumns.Any())
                continue;
            
            foreignColumnImportModel.State = foreignColumnImportModel.ForeignColumns.All(c => c.IsAvailable)
                ? this["AllColumnItemsAvailable"]
                : this["SomeColumnItemsNotAvailable"];
            
            list.Add(foreignColumnImportModel);
        }

        return list;
    }
    
    private class FileProcessResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
        public List<TableItemImportModel<T>> Items { get; set; } = new();
        public List<ItemImportErrorModel> Errors { get; set; } = new();
        public List<ItemImportErrorModel> Duplicates { get; set; } = new();
        public Dictionary<ColumnImportModel, List<string>> ColumnValues { get; set; } = new();
        public List<ColumnImportModel> AvailableColumns { get; set; } = new();
        
        public List<ForeignColumnImportModel> ForeignColumns { get; set; } = new();
    }

    private class ItemProcessResult
    {
        public bool Success { get; set; }
        public (string, bool) Result { get; set; }
        public Exception Exception { get; set; }
    }
}
