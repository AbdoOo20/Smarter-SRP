using Fly.Shared.Modules.Accounting.Dtos;
using Leapay.Client.ClientModels.Dto.DtoModels;
using Leapay.Client.Events.Arguments;
using Leapay.Client.Polymorphism.Bases;
using Leapay.Client.Services;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Accounting.Entries;

public partial class EntriesEntryViewModel : BasePageEntryViewModel<DtoEntry, DtoEntryDetail>
{
    protected override string Endpoint => "Entry";
    
    public EntriesEntryViewModel(
        DtoEntry? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType, false);
        
        if (EntryType == EntryType.Add)
        {
            Model.Date = DateTime.Now;
        }
        
        Model.Details ??= [];
        
        OnItemDelete += OnDelete;
        OnItemEdit += OnEdit;

        //OnDataGridSelectionChanged += OnDataSelectionChanged;
        RefreshState(async () => await StartupAsync());
    }

    public void SetDate(DatePicker picker)
    {
        picker.SelectedDate = Model.Date;
        picker.SelectedDateChanged += (_, e)
            => Model.Date = e.NewDate.GetValueOrDefault().DateTime;
    }
    
    private async Task StartupAsync(bool refresh = true)
    {
        var accounts = (await RequestComboModelsAsync<ComboModel>("accounts")).Data;
        
        var data = Model?.Details?.Select(dto =>
        {
            dto.AccountName = accounts.FirstOrDefault(u 
                => u.Value == dto.AccountId.Value.ToString())?.Display;
            dto.TemporaryId = dto.Id;

            return dto;
        }).ToList();

        Model?.Details?.Clear();
        Model?.Details?.AddRange(data ?? []);

        SetDetailsModel(null);
        if (refresh)
        {
            await RefreshAsync();
        }
    }

    #region Model

    protected override Task RefreshAsync(bool refreshCombos = true)
    {
        SetData(Model?.Details);
        RefreshDataGrid();

        return Task.CompletedTask;
    }
    
    /*[RelayCommand]
    private void OnAdd()
    {
        var @new = new DtoEntryDetail
        {
            TemporaryId = Identifier.Create(Guid.NewGuid())
        };
        
        Model.Details.Add(@new);
        SetData(Model.Details);
        RefreshDataGrid();

        SelectInDataGrid(@new);
    }*/
    
    private void OnEdit(object? sender, DataGridButtonEventArgs<DtoEntryDetail> args)
    {
        SetDetailsModel(args.Model);
    }

    private void OnDataSelectionChanged(object? sender, DtoEntryDetail e)
    {
        SetDetailsModel(e);
    }
    
    protected override async void OnDelete(object? sender, DataGridButtonEventArgs<DtoEntryDetail> args)
    {
        if (!await DialogueServices.ConfirmDeleteAsync())
            return;

        Model.Details = HelperServices
            .RemoveItemFromModelList(Model.Details, args.Model)
            .OfType<DtoEntryDetail>()
            .ToList();

        _ = RefreshAsync(false);
    }

    public void UpdateListing(DtoEntryDetail model)
    {
        var list = Model.Details.ToList();
        
        var item = list.FirstOrDefault(m => m.TemporaryId == model.TemporaryId);

        if (item is null)
        {
            list.Add(model);
        }
        else
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TemporaryId == model.TemporaryId)
                    list[i] = model;
            }
        }

        Model.Details = list;
        SetData(Model.Details.ToList());
        RefreshDataGrid();
    }

    #endregion
}