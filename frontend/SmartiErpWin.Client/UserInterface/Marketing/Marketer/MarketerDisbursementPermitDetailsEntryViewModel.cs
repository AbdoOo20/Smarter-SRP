using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Store.Requests;
using SmartErpWin.Core;
using SmartiErpWin.Client.DtoModels.MarketingModels;

namespace SmartiErpWin.Client.UserInterface.Marketing.Marketer;

public sealed class MarketerDisbursementPermitDetailsEntryViewModel
    : BaseDetailsEntryViewModel<DtoMarketerDisbursementPermitDetailsModel>
{
    protected override string Endpoint => "MarketerDisbursementPermit";

    public MarketerDisbursementPermitDetailsEntryViewModel(
        LocalisationService locale,
        HttpClient client,
        Type pageType) : base(client)
    {
        PageType = pageType;

        InitBaseModel(locale, null, null);
        ApplicationTitle = GetPageName(pageType, EntryType);

        SetState(null);

        AddValidation(() => Model.ItemId, optional: false)
        .AddValidation(() => Model.UnitId, optional: false)
        ;
    }

    protected internal sealed override async void Refresh()
    {
        await PrepareComboBoxAsync("items", Model?.ItemId.GetValueOrNull());
        //this.WorkingModel.TemporaryStoreId =

        if (Model is not null && Model.ExpireDate is not null)
        {
            SetExpireDate();
            ComboModelBase.SetComboModel(x => x.Value == Model.ExpireDateString, 2);
        }
    }

    public async void SetExpireDate()
    {
        if (WorkingModel is null)
            return;

        var storeId = WorkingModel.TemporaryStoreId;
        var storeIdValue = storeId.GetValueOrDefault();
        var itemId = ComboModelBase.GetSelectedItemId();

        if (storeId == Identifier.Empty || itemId == Identifier.Empty)
        {
            ComboModelBase.Clear(2, true);
            return;
        }

        await RequestCombosUsingQueryRequestAsync("ExpireDate", 2, new ExpireDateQueryRequest
        {
            ItemId = itemId,
            StoreId = storeIdValue
        });
        ComboModelBase.SelectAtOnePoint(2);
    }

    public async void SetUnits()
    {
        var selectedItem = ComboModelBase.GetSelectedItemId();

        if (selectedItem == Identifier.Empty)
            return;

        await RequestApiComboItemsFromInnerEndpointAsync("units", 1, selectedItem.ToString());
        ComboModelBase.SetComboModelOrDefault(Model?.UnitId, 1);
        ComboModelBase.SelectAtOnePoint(1);
    }

    protected override void PreSaveActions()
    {
        Model.UnitId = ComboModelBase.GetSelectedItemId(1);
        Model.UnitName = ComboModelBase.GetSelectedItemDisplay(1);
        Model.ItemId = ComboModelBase.GetSelectedItemId();
        Model.ItemName = ComboModelBase.GetSelectedItemDisplay();
        //if (Model.ItemName.Contains("•"))
        //{
        //    var nnn = Model.ItemName.Split("•");
        //    Model.ite = nnn[1];
        //    Model.ItemName = nnn[0];
        //}
        Model.ExpireDate = null;
        if (ComboModelBase.SelectedApiComboItems[2] != null)
        {
            if (DateTime.TryParse(ComboModelBase.SelectedApiComboItems[2]!.Value, out var date))
            {
                Model.ExpireDate = date;
            }
        }

        // SetState(null);
    }

    protected override bool PreSaveValidations()
    {
        var model = GetWorkingModel()!;
        if (ComboModelBase.GetApiCollectionByIndex(2).Count > 1 & model.ExpireDate is null)
        {
            _ = DialogueServices.ShowMessageAsync(this["ValidationFailed"], "ExpireDateCannotBeEmpty", localeService: LocaleService);
            return false;
        }
        return base.PreSaveValidations();
    }

    protected override Task SaveAsync()
    {
        var model = GetWorkingModel();

        if (model is null)
            return Task.CompletedTask;

        var x = Model.TemporaryStoreId;
        model.Quantity = model.Quantity is 0 ? 1 : model.Quantity;

        PreSaveActions();

        if (!PreSaveValidations())
            return Task.CompletedTask;

        SynchroniseTypeBase(PageType, model);
        SetState(null);

        Model.TemporaryStoreId = x;
        ComboModelBase.SelectAtOnePoint(0);
        ComboModelBase.SetApiComboItems([], 2);

        return Task.CompletedTask;
    }

    public override Task OnSynchronisationRequestedAsync(params object[] args)
    {
        switch (args.FirstOrDefault())
        {
            case DtoMarketerDisbursementPermitDetailsModel model:
                SetState(model);
                break;
            case Identifier id:
                if (WorkingModel is null)
                    break;
                WorkingModel.TemporaryStoreId = id;
                SetExpireDate();
                break;
            case CodeBook.LimVis:
                break;
        }

        return Task.CompletedTask;
    }
}