using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Requests;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.Accounting.Requests;

namespace Fly.Shared.Modules.Accounting.Features;

public interface IExpenseTypesFeatures : IFeatureBase<DtoExpenseTypes, ExpenseTypesQueryRequest, DeleteRequestBase, CreateUpdateNamedRequestBase>
{
}