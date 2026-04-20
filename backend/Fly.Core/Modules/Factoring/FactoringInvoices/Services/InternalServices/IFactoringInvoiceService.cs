using Fly.Core.Modules.Factoring.FactoringInvoices.Models.DbModels;
using Fly.Framework.Server.Persistence;
using Fly.Shared.Modules.Factoring.Dtos;

namespace Fly.Core.Modules.Factoring.FactoringInvoices.Services.InternalServices;


public interface IFactoringInvoiceService : IServiceBase<DtoFactoringInvoice, TFactoringInvoice> { }