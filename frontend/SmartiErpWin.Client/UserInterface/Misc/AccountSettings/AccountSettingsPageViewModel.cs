using Fly.Framework.Shared.Common;
using Fly.Shared.Modules.Global.Dtos;
using Fly.Shared.Modules.Global.Requests;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SmartiErpWin.Client.DtoModels;
using Microsoft.Win32;
using SmartErpWin.Core;
using Wpf.Ui;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace SmartiErpWin.Client.UserInterface.Misc.AccountSettings;

public partial class AccountSettingsPageViewModel : BaseEntryViewModel<DtoConfig>
{
    protected override string Endpoint => "Config";

    private readonly IServiceProvider _prov;

    [ObservableProperty]
    private DtoCompanyInfo? _companyInfo;

    [ObservableProperty]
    private string? _image;

    public AccountSettingsPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        IServiceProvider ser) : base(client)
    {
        InitBaseModel(localisationService, sessionService, contentDialogService);

        _prov = ser;

        Refresh();
    }

    public void BrowseImage()
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
        };

        if (openFileDialog.ShowDialog() ?? false)
            return;
        
        Image = openFileDialog.FileName;

        CompanyInfo.Image = Image;
        OnPropertyChanged(nameof(CompanyInfo));
    }

    private async void Refresh()
    {
        var request = (await RequestComboModelsAsync<ComboModel>("accounts")).Data;

        ComboModelBase.SetApiComboItems(request);
        ComboModelBase.SetApiComboItems(request, 1);
        ComboModelBase.SetApiComboItems(request, 2);
        ComboModelBase.SetApiComboItems(request, 3);
        ComboModelBase.SetApiComboItems(request, 4);
        ComboModelBase.SetApiComboItems(request, 5);
        ComboModelBase.SetApiComboItems(request, 6);
        ComboModelBase.SetApiComboItems(request, 7);
        ComboModelBase.SetApiComboItems(request, 8);
        ComboModelBase.SetApiComboItems(request, 9);
        ComboModelBase.SetApiComboItems(request, 10);
        ComboModelBase.SetApiComboItems(request, 11);
        ComboModelBase.SetApiComboItems(request, 12);
        ComboModelBase.SetApiComboItems(request, 13);

        Model = (await GetInnerOneAsync("account-settings")).Data;
        CompanyInfo = (await GetSpecificInnerOneAsync<DtoCompanyInfo>("company-settings")).Data;
        Image = string.IsNullOrEmpty(CompanyInfo?.Image)
            ? Config.Get?.ClientConfig.ImagePath
              ?? ImagePlaceholder : CompanyInfo.Image;

        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.ClientsAccount)));
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.SupplyersAccount)), 1);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.ExpenseAccount)), 2);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.BankAccount)), 3);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.PurchaseInvoicesAccount)), 4);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.SalesInvoicesAccountId)), 5);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.GeneralClientAccountAccount)), 6);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.GeneralSupplierAccountAccount)), 7);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.MarketerInvoicesAccountId)), 8);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.SafeAccount)), 9);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.EmployeesAccount)), 10);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.AssetsAccount)), 11);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.DiscountAccount)), 12);
        ComboModelBase.SetComboModelOrDefault(Identifier.Create(Guid.Parse(Model.GeneralShareAccount)), 13);
    }

    protected override void PreSaveActions()
    {
        Model.ClientsAccount = ComboModelBase.GetSelectedItemId().Value.ToString();
        Model.SupplyersAccount = ComboModelBase.GetSelectedItemId(1).Value.ToString();
        Model.ExpenseAccount = ComboModelBase.GetSelectedItemId(2).Value.ToString();
        Model.BankAccount = ComboModelBase.GetSelectedItemId(3).Value.ToString();
        Model.PurchaseInvoicesAccount = ComboModelBase.GetSelectedItemId(4).Value.ToString();
        Model.SalesInvoicesAccountId = ComboModelBase.GetSelectedItemId(5).Value.ToString();
        Model.GeneralClientAccountAccount = ComboModelBase.GetSelectedItemId(6).Value.ToString();
        Model.GeneralSupplierAccountAccount = ComboModelBase.GetSelectedItemId(7).Value.ToString();
        Model.MarketerInvoicesAccountId = ComboModelBase.GetSelectedItemId(8).Value.ToString();
        Model.SafeAccount = ComboModelBase.GetSelectedItemId(9).Value.ToString();
        Model.EmployeesAccount = ComboModelBase.GetSelectedItemId(10).Value.ToString();
        Model.AssetsAccount = ComboModelBase.GetSelectedItemId(11).Value.ToString();
        Model.DiscountAccount = ComboModelBase.GetSelectedItemId(12).Value.ToString();
        Model.GeneralShareAccount = ComboModelBase.GetSelectedItemId(12).Value.ToString();
    }

    protected override async Task SaveAsync()
    {

        if (Model is null || !PreSaveChecks())
            return;

        PreSaveActions();

        if (!PreSaveValidations())
            return;

        //await PostSaveSpecificAsync(CompanyInfo, "Company");
        //await PostSaveAsync(Model);

        SettingsRequest settingsRequest = new()
        {
            CompanyInfo = CompanyInfo.Adapt<CompanyInfoRequest>(),
            Config = Model.Adapt<ConfigRequest>(),
        };
        var response = await PostSaveSpecificAsync(settingsRequest);

        if (response is { Data: { }, Status: true })
        {
            var config = Config.Get;

            config.ClientConfig = new()
            {
                Address = CompanyInfo.Address,
                Decription = CompanyInfo.Type,
                NameAr = CompanyInfo.Name,
                Phone1 = CompanyInfo.PhoneNumber1,
                Phone2 = CompanyInfo.PhoneNumber2,
                Email = CompanyInfo.Email,
            };

            config.ClientConfig.MoveImage(CompanyInfo.Image);
            
            Config.SaveSettings(config);
        }
        else
        {

        }

        //  todo: checleck if needed
        //ReportRunner.OnSaveComplate(new(Model));

        EntryWindow?.Close();

        SynchroniseTypeBase(PageType);
    }

    [RelayCommand]
    private Task RefreshAccountsAsync()
    {
        _prov.GetRequiredService<AccountSettingsPage>().ResetInstance();

        return Task.CompletedTask;
    }
}