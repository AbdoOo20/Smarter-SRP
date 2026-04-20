using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Common.Requests;
using Fly.Framework.Shared.Common.ValueTypes;
using Fly.Shared.Modules.Accounting.Requests;
using Fly.Shared.Modules.Personnel.Dtos;
using Fly.Shared.Modules.Personnel.Requests;

namespace Fly.Shared.Modules.Personnel.Features;

public interface IEmployeeFeatures : IFeatureBase<DtoEmployee, EmployeeQueryRequest, DeleteRequestBase, CreateUpdateEmployeeRequest>
{
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupAccounts(AccountQueryRequest request);
    ValueTask<HttpResponseWrapperList<DefaultLookup>> GetLookupDepartments(DepartmentQueryRequest request);
}