using AutoMapper;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;

namespace SmartiErpWin.Client.UserInterface.Personnel.Employees.Partials;

public partial class SalaryCountingWindowViewModel : BaseRefreshableViewModel<DtoBase>
{
    public SalaryCountingWindow? Window { get; set; }

    [ObservableProperty]
    private string _actionText;

    [ObservableProperty]
    private long _mounthNumber;

    public SalaryCountingWindowViewModel(
        SessionService session,
        LocalisationService localeService,
        HttpClient client,
        IMapper mapper) : base(client)
    {
        InitBaseModel(localeService, session, null);
        InfoText = LocaleService["MounthNumber"];
        ApplicationTitle = LocaleService["CountingSalary"];
        ActionText = LocaleService?["Search"];
        _mapper = mapper;
    }

    [ObservableProperty]
    private string? _infoText;

    private readonly IMapper _mapper;

    [RelayCommand]
    private async Task OnActionAsync(string parameter)
    {
        SetRefreshState(true);

        if (MounthNumber < 1 || MounthNumber > 12)
        {
            SetRefreshState(false);
            SetInfo(LocaleService["MounthNumberError"]);
            return;
        }

        try
        {
            SetInfo(LocaleService["CountingSalaryWorking"]);
            var request = await PostSpecificOneAsync<TypeOf<bool>>($"Employee/CountingSalary/{MounthNumber}");

            await Task.Delay(1000);

            if (request is { Data: not null, Status: true })
            {
                Success(request.Data);
            }
            else
            {
                Unsuccessful(request?.ErrorMessage ?? "NO ERROR MESSAGE");
            }
        }
        catch (Exception x)
        {
            Unsuccessful(x.Message);
        }

        return;

        void Unsuccessful(string message)
        {
            SetRefreshState(false);
            InfoText = $"{LocaleService["UnsuccessfulLookup"]}\n{message}";
        }

        async void Success(TypeOf<bool> sale)
        {
            //SetInfo(this["LoadingInvoice"]);
            Window.DialogResult = true;
            Window?.Close();
        }
    }
}