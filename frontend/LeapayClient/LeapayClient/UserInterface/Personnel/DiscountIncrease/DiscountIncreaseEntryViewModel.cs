using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Enums;
using Leapay.Client.Polymorphism.Bases;
using SmartErpWin.Core.Enums;

namespace Leapay.Client.UserInterface.Personnel.DiscountIncrease;

public class DiscountIncreaseEntryViewModel : BaseEntryViewModel<DtoDiscountIncrease>
{
    protected override string Endpoint => "DiscountIncrease";
    
    public SuperComboVm<DtoDiscountIncrease> Employees { get; } = new();
    
    public StaticComboVm<DiscountIncreaseMode> DiscountIncreaseType { get; } = new();

    public DiscountIncreaseEntryViewModel(
        DtoDiscountIncrease? model,
        EntryType entryType,
        Type pageType)
    {
        SetDetails(model, entryType, pageType);
        
        AddValidation(() => Model.EmployeeId, optional: false)
            .AddValidation(() => Model.Amount,
                [new(d => d > 0, "AmountMustGreaterThanZero")], optional: false);
        
        Refresh();
    }

    private async void Refresh()
    {
        await Employees.LoadAsync(this, "employees", Model.EmployeeId, s => Model.EmployeeId = s.Id);
        DiscountIncreaseType.Set(Model.Mode, s => Model.Mode = s);
    }
}