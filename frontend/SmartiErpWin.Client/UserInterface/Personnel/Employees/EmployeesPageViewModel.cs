using AutoMapper;
using Fly.Shared.Modules.Personnel.Dtos;
using SmartiErpWin.Client.UserInterface.Personnel.Employees.Partials;
using Wpf.Ui;

namespace SmartiErpWin.Client.UserInterface.Personnel.Employees;

public partial class EmployeesPageViewModel : BasePageViewModel<DtoEmployee>
{
    protected override string Endpoint => "Employee";
    private readonly IMapper _mapper;

    public EmployeesPageViewModel(HttpClient client,
        IContentDialogService contentDialogService,
        SessionService sessionService,
        LocalisationService localisationService,
        IMapper mapper) : base(client)
    {
        _mapper = mapper;
        InitBaseModel(localisationService, sessionService, contentDialogService);

        OnItemEdit += OnEdit;
        OnItemDelete += OnDelete;
    }

    private void OnEdit(object? sender, EventArgs args)
    {
        var model = DataGridServices.FindModel<DtoEmployee>(sender);

        if (model is null)
            return;

        var entry = new EmployeesEntryWindow(LocaleService, model,
            EntryType.Edit, HttpClient, GetType());

        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnAdd()
    {
        var entry = new EmployeesEntryWindow(LocaleService, null,
            EntryType.Add, HttpClient, GetType());

        entry.ShowDialog();
    }

    [RelayCommand]
    private void OnCountingSalary()
    {
        var entry = new SalaryCountingWindow(new SalaryCountingWindowViewModel(Session, LocaleService, HttpClient, _mapper));

        entry.ShowDialog();
    }
}