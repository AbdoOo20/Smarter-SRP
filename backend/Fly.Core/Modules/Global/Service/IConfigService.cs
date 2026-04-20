using Fly.Core.Persistence.Ef.Models.Global;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Global.Dtos;
using System.Linq.Expressions;

namespace Fly.Core.Modules.Global.Service;

/*
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
TRUNCATE `tdisbursementpermit`;
TRUNCATE `tdisbursementpermitdetail`;
TRUNCATE `tentry`;
TRUNCATE `tentrydetail`;
TRUNCATE `tinventory`;
TRUNCATE `tpurchaseinvoice`;
TRUNCATE `tpurchaseinvoicecost`;
TRUNCATE `tpurchaseinvoicedetail`;
TRUNCATE `tsalesinvoice`;
TRUNCATE `tsalesinvoicedetail`;
TRUNCATE `tsupplypermit`;
TRUNCATE `tsupplypermitdetail`;
SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1);
*/

public interface IConfigService : IServiceBase<DtoConfig, TConfig>
{
    ValueTask<T?> GetConfigValue<T>(Expression<Func<DtoConfig, T>> selector);
}