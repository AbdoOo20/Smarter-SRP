using Fly.Shared.Modules.Accounting.Dtos;

namespace SmartiErpWin.Client.DtoModels.AccountingModels;

public partial class DtoAccountModel: ObservableObject
{
   [ObservableProperty]
   private DtoAccount? _account;
}