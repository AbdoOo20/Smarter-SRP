using Fly.Shared.Modules.Accounting.Dtos;

namespace Leapay.Client.ClientModels.Dto.DtoModels.AccountingModels;

public partial class DtoAccountModel: ObservableObject
{
   [ObservableProperty]
   private DtoAccount? _account;
}