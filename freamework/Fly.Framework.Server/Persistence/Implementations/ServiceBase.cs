using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Fly.Framework.Server.Persistence.Implementations;

public class ServiceBase<TTdo, TEn> : IServiceBase<TTdo, TEn>
where TTdo : DtoBase
where TEn : TEntity, new()
{
    protected bool useAutomapper = false;
    private IServiceScope _serviceScope;

    protected readonly ILogger _logger;
    protected IServiceProvider _serviceProvider;
    protected readonly AutoMapper.IMapper _mapper;
    protected readonly IRepositryBase<TEn> _repositry;
    protected readonly MapsterMapper.IMapper _mapsterMapper;

    public event EventHandler<TTdo>? Added;
    public event EventHandler<TTdo>? Updated;
    public event EventHandler<TTdo>? Deleted;

    public bool UseSoftDelete { get; } = true;
    public IRepositryBase<TEn> Repositry => _repositry;

    public ServiceBase(ILogger<ServiceBase<TTdo, TEn>> logger,
        AutoMapper.IMapper mapper,
    MapsterMapper.IMapper mapsterMapper,
    IServiceProvider serviceProvider)
    {
        _serviceScope = serviceProvider.CreateScope();
        _serviceProvider = _serviceScope.ServiceProvider;

        _logger = logger;
        _mapper = mapper;
        _mapsterMapper = mapsterMapper;
        _serviceProvider = serviceProvider;
        _repositry = _serviceProvider.GetRequiredService<IRepositryBase<TEn>>();
    }

    #region Commands


    #region Insert

    public virtual ValueTask<bool> BeforeAdd(TEn entity)
    {
        return ValueTask.FromResult(true);
    }

    public virtual async ValueTask<TTdo?> Add(TTdo entity)
    {
        var db = _mapsterMapper.Map<TEn>(entity);

        await BeforeAdd(db);
        var addingStatus = await Repositry.Add(db, true);
        if (addingStatus)
        {
            await AfterAdd(db);
            _mapsterMapper.Map(db, entity);
            Added?.Invoke(db, entity);
            return entity;
        }
        else return null;
    }

    public virtual async ValueTask<TTdo?> AddT(TEn db, TTdo entityDto)
    {
        await BeforeAdd(db);
        var addingStatus = await Repositry.Add(db, true);
        if (addingStatus)
        {
            await AfterAdd(db);
            _mapsterMapper.Map(db, entityDto);
            Added?.Invoke(db, entityDto);
            return entityDto;
        }
        
        else return null;
    }
    
    public virtual ValueTask<bool> AfterAdd(TEn entity)
    {
        return ValueTask.FromResult(true);
    }

    public async ValueTask<IEnumerable<TTdo>> AddRange(IEnumerable<TTdo> entities)
    {
        foreach (var ent in entities)
        {
            var db = _mapsterMapper.Map<TEn>(ent);
            await BeforeAdd(db);
            var status = await Repositry.Add(db, false);
            await AfterAdd(db);
            _mapsterMapper.Map(db, ent);
            Added?.Invoke(db, ent);
        }
        if (entities.Any())
            await _repositry.Save();

        var all = _repositry.GetAll().MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper).AsEnumerable();
        return all;
    }

    public ValueTask<IEnumerable<TTdo>> AddRange(IEnumerable<TEn> entities)
    {
        foreach (var ent in entities)
        {
            _repositry.Add(ent, false);
        }
        if (entities.Any())
            _repositry.Save();

        var all = _repositry.GetAll().MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper).AsEnumerable();
        //var all = entities;
        return ValueTask.FromResult(all);
    }

    public async ValueTask<TTdo?> AddOrUpdateAsync(TTdo dto)
    {
        if (dto.IsNew())
            return await Add(dto);
        else
            return await Update(dto);
    }

    #endregion


    #region Remove

    public async ValueTask<TTdo> Remove(TTdo entity, bool? useSoftDelete = null)
    {
        await Remove(entity.Id, useSoftDelete);
        return entity;
    }
    public async ValueTask Remove(Expression<Func<TTdo, bool>>? predicate = null, bool? useSoftDelete = null)
    {
        useSoftDelete ??= UseSoftDelete;
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        if (_repositry is EfRepositryBase<TEn> repo)
        {
            var q = _repositry.GetAll(expression);
            if (useSoftDelete == true)
            {
                await q.ExecuteUpdateAsync(w => w.SetProperty(s => s.IsDeleted, true));
            }
            else
            {
                await q.ExecuteDeleteAsync();
            }
        }
        else
        {
            var all = _repositry
                .GetAll(expression)
                .Select(s => new TEn() { Id = s.Id })
                .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
                .ToList();
            await RemoveRange(all);
        }

    }
    public async ValueTask<TTdo> Remove(Guid id, bool? useSoftDelete = null)
    {
        useSoftDelete ??= UseSoftDelete;
        var identifier = await GetId(id);
        if (identifier is null) throw new FileNotFoundException();
        return await Remove(identifier.Value, useSoftDelete);
    }

    public async ValueTask<TTdo> Remove(Identifier id, bool? useSoftDelete = null)
    {
        var status = false;
        useSoftDelete ??= UseSoftDelete;
        var ent = await GetById(id);
        //  TODO:
        if (ent == null) throw new FileNotFoundException();
        if (useSoftDelete == true)
        {
            status = await _repositry.SoftRemoveById(id);
        }
        else
        {
            status = await _repositry.RemoveById(id);
        }
        if (status)
            Deleted?.Invoke(id, ent);
        return ent;
    }

    public async ValueTask<IEnumerable<TTdo>> RemoveRange(IEnumerable<TTdo> entities)
    {
        var arr = entities.Select(s => s.Id).ToArray();
        await RemoveRange(arr);
        return entities;
        //throw new NotImplementedException();
    }

    public async ValueTask<IEnumerable<Identifier>> RemoveRange(Identifier[] id, bool? useSoftDelete = null)
    {
        useSoftDelete ??= UseSoftDelete;
        var total = 0;
        if (_repositry is EfRepositryBase<TEn> efRepo)
        {
            var q = efRepo.DbSet.Where(w => id.Contains(w.Id));
            if (useSoftDelete.Value)
            {
                total = await q.ExecuteUpdateAsync(x => x.SetProperty(c => c.IsDeleted, true));
            }
            else
            {
                await q.ExecuteDeleteAsync();
            }
            if (total == id.Length)
            {
                foreach (var item in id)
                {
                    Deleted?.Invoke(id, null);
                }
            }
            return id.AsEnumerable();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    #endregion


    #region Update

    public virtual ValueTask<bool> BeforeUpdate(TEn entity)
    {
        return ValueTask.FromResult(true);
    }

    public async ValueTask<TTdo?> Update(TTdo entity)
    {
        var dbEnt = _repositry.GetById(entity.Id);
        if (!dbEnt.Any())
            //throw new Exception();
            return null;

        var db = dbEnt.Single();
        _mapsterMapper.Map(entity, db);

        await BeforeUpdate(db);
        var status = await Repositry.Update(db, true);
        if (!status) return null;

        await AfterUpdate(db);
        _mapsterMapper.Map(db, entity);

        if (status) Updated?.Invoke(db, entity);
        return entity;
    }

    public async ValueTask<TTdo?> UpdateT(TEn entity, TTdo entityDto)
    {
        var dbEnt = _repositry.GetById(entity.Id);
        if (!dbEnt.Any())
            //throw new Exception();
            return null;

        var db = dbEnt.Single();
        _mapsterMapper.Map(entity, db);

        await BeforeUpdate(db);
        var status = await Repositry.Update(db, true);
        if (!status) return null;

        await AfterUpdate(db);
        _mapsterMapper.Map(db, entity);

        if (status) Updated?.Invoke(db, entityDto);
        return entityDto;
    }
    
    public virtual ValueTask<bool> AfterUpdate(TEn entity)
    {
        return ValueTask.FromResult(true);
    }

    public async ValueTask<IEnumerable<TTdo>> UpdateRange(IEnumerable<TTdo> entities)
    {
        foreach (var item in entities)
        {
            await Update(item);
        }
        return entities;
    }

    #endregion


    #endregion


    #region Queries


    #region Lists

    public ValueTask<PageList<TResult>> GetTopListAsAsync<TResult>(Expression<Func<TTdo, TResult>> select,
                                                                                int take = 100,
                                                                                Expression<Func<TTdo, bool>>? predicate = null,
                                                                                Expression<Func<TTdo, object>>? descendingOrderBy = null,
                                                                                Expression<Func<TTdo, object>>? orderBy = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var descendingOrderByRepo = _mapper.Map<Expression<Func<TEn, object>>>(descendingOrderBy);
        var orderByRepo = _mapper.Map<Expression<Func<TEn, object>>>(orderBy);
        var selectRepo = _mapper.Map<Expression<Func<TEn, TResult>>>(select);

        var allQ = _repositry
            .GetAll(expression, descendingOrderBy: descendingOrderByRepo, orderBy: orderByRepo)
            .Take(take)
            .Select(selectRepo)
        ;
        var all = allQ.AsEnumerable();

        return ValueTask.FromResult(PageList<TResult>.Create(all, 1));
    }

    public ValueTask<PageList<SelectDataItem<Identifier>>> GetSelecListAsync(Func<TTdo, Identifier> selectId, Func<TTdo, string> text, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null)
    {
        descendingOrderBy ??= x=> x.Id;
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var descendingOrderBymapped = _mapper.Map<Expression<Func<TEn, object>>>(descendingOrderBy);
        var allQ = _repositry.GetAll(expression, descendingOrderBy: descendingOrderBymapped);

        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .Select(x => new SelectDataItem<Identifier>() { Id = selectId(x), Text = text(x) })
            .AsEnumerable();

        return ValueTask.FromResult(PageList<SelectDataItem<Identifier>>.Create(all, 1));
    }
    public ValueTask<PageList<DefaultLookup>> GetSelecListAsync(Func<TTdo, string> text, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null)
    {
        descendingOrderBy ??= x=> x.Id;
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var descendingOrderBymapped = _mapper.Map<Expression<Func<TEn, object>>>(descendingOrderBy);
        var allQ = _repositry.GetAll(expression, descendingOrderBy: descendingOrderBymapped);

        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .Select(x => new DefaultLookup { Value = x.Id, Display = text(x) })
            .ToList();

        return ValueTask.FromResult(PageList<DefaultLookup>.Create(all, 1));
    }

    public ValueTask<PageList<DefaultLookup>> GetDefaultLookupAsync(Func<TTdo, Identifier> selectId, Func<TTdo, string> text, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null)
    {

        descendingOrderBy ??= x=> x.Id;
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var descendingOrderBymapped = _mapper.Map<Expression<Func<TEn, object>>>(descendingOrderBy);
        var allQ = _repositry.GetAll(expression, descendingOrderBy: descendingOrderBymapped);

        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .Select(x => new DefaultLookup { Value = selectId(x).Value, Display = text(x) })
            .ToList();

        return ValueTask.FromResult(PageList<DefaultLookup>.Create(all, 1));
    }

    public ValueTask<PageList<TResult>> GetAllAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null, int? page = null, int? limit = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var allQ = _repositry.GetAll(expression, descendingOrderBy: x => x.Id, page: page, limit: limit);
        //  useAutomapper = true
        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .Select(select);

        return ValueTask.FromResult(PageList<TResult>.Create(all, 1));
    }
    
    public ValueTask<PageList<TResult>> GetAllAsEntityAsync<TResult>(Expression<Func<TEn, TResult>> select, Expression<Func<TEn, bool>>? predicate = null, int? page = null, int? limit = null)
    {
        var allQ = _repositry.GetAll(predicate, descendingOrderBy: x => x.Id, page: page, limit: limit);
        //  useAutomapper = true
        var all = allQ
            .Select(select)
            .MapToDto<TResult>(_mapper, _mapsterMapper, useAutomapper)
            ;

        return ValueTask.FromResult(PageList<TResult>.Create(all, 1));
    }

    public ValueTask<PageList<TTdo>> GetAllAsync(Expression<Func<TTdo, bool>>? predicate = null, int? page = null, int? limit = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);

        if (page is null)
        {
            var nonPaged = _repositry.GetAll(expression, null, null, descendingOrderBy: x => x.Id);
            var allNonPaged = nonPaged
            .MapToDto<TTdo>(_mapper, _mapsterMapper, false)
            .ToList();
            if(nonPaged.Count() != allNonPaged.Count())
            {
                _logger.LogWarning("Different data count in non-paged query. " +
                                   "Entity: {Entity}, NonPaged Count: {NonPagedCount}, " +
                                   "Paged Count: {PagedCount}", 
                    typeof(TEn).Name, nonPaged.Count(), allNonPaged.Count);
            }
            return ValueTask.FromResult(PageList<TTdo>.Create(allNonPaged, 0));
        }

        limit ??= 20; //UIPagingConstants.PAGE_LIMIT;
        page = page == 0 ? 1 : page;
        limit = limit == 0 ? 20 : limit;

        var allQ = _repositry.GetAll(expression, page, limit, descendingOrderBy: x => x.Id);
        var allQ2 = _repositry.GetAll(expression, null, null, descendingOrderBy: x => x.Id);

        //  useAutomapper = true
        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, false)
            .ToList();
        //  NOTE: sync call
        var allRowsCount = allQ2.Count();
        var totalPages = (int)Math.Ceiling((decimal)allRowsCount / limit.Value);
        return ValueTask.FromResult(PageList<TTdo>.Create(all, page.Value, totalPages));
    }

    #endregion


    #region One

    //private Expression<Func<TEn, bool>> Map2()
    //    =>
    //    Mapster.TypeAdapterConfig.GlobalSettings.Default.Config.CreateMapExpression(
    //          new Mapster.Models.TypeTuple(typeof(TTdo), typeof(TEn)),
    //          Mapster.MapType.Projection);

    public ValueTask<TResult?> GetTopOneAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null, Expression<Func<TTdo, object>>? orderBy = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var descendingOrderByRepo = _mapper.Map<Expression<Func<TEn, object>>>(descendingOrderBy);
        var orderByRepo = _mapper.Map<Expression<Func<TEn, object>>>(orderBy);
        var selectRepo = _mapper.Map<Expression<Func<TEn, TResult>>>(select);

        var allQ = _repositry
            .GetAll(expression, descendingOrderBy: descendingOrderByRepo, orderBy: orderByRepo)
            .Select(selectRepo)
        //.Select(selectRepo)
        ;
        var all = allQ.FirstOrDefault();
        return ValueTask.FromResult(all);
    }

    public ValueTask<long> NextNumber(Expression<Func<TTdo, long>> select)
    {
        var lasNumb = 0L;
        var selectRepo = _mapper.Map<Expression<Func<TEn, long>>>(select);
        var a = _repositry.GetAll(descendingOrderBy: x => x.Id);
        var se = a.Select(selectRepo);

        if (!a.Any()) return ValueTask.FromResult(1L);
        else if (se.Any()) lasNumb = se.Max();

        return ValueTask.FromResult(++lasNumb);
    }

    public ValueTask<bool> AnyAsync(Expression<Func<TTdo, bool>>? predicate = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var allQ = _repositry.GetAll(expression);
        var any = allQ.Any();
        return ValueTask.FromResult(any);
    }
    public ValueTask<bool> AnyAsync(Expression<Func<TEn, bool>>? predicate = null)
    {
        var allQ = _repositry.GetAll(predicate);
        var any = allQ.Any();
        return ValueTask.FromResult(any);
    }

    public ValueTask<TResult?> GetOneAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var allQ = _repositry.GetAll(expression, descendingOrderBy: x => x.Id);

        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .Select(select)
            .FirstOrDefault();
        return ValueTask.FromResult(all);
    }

    public ValueTask<TTdo?> GetAsync(Expression<Func<TTdo, bool>>? predicate = null, int? page = null, int? limit = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var allQ = _repositry.GetAll(expression, page, limit, descendingOrderBy: x => x.Id);

        var all = allQ
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .FirstOrDefault();

        return ValueTask.FromResult(all);
    }

    public ValueTask<TTdo?> GetById(Identifier id)
    {
        var q = _repositry.GetById(id);
        //  TODO: Inhencment
        //  Use SingleOrDefault
        var ent = q
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .FirstOrDefault();

        return ValueTask.FromResult(ent);
    }

    public ValueTask<TEn?> GetTById(Identifier id)
    {
        var q = _repositry.GetById(id);

        return ValueTask.FromResult(q.FirstOrDefault());
    }
    
    public ValueTask<TTdo?> GetByGuId(Guid id)
    {
        var q = _repositry.GetAll(w => w.Guid == id);
        //  TODO: Inhencment
        //  Use SingleOrDefault
        var ent = q
            .MapToDto<TTdo>(_mapper, _mapsterMapper, useAutomapper)
            .FirstOrDefault();

        return ValueTask.FromResult(ent);
    }

    #endregion


    #region Info

    public ValueTask<bool> Exists(Identifier id)
    {
        var q = _repositry.GetAll(x => x.Id == id);
        var isExists = q.Any();
        return ValueTask.FromResult(isExists);
    }

    public ValueTask<Identifier?> GetId(Guid id)
    {
        Identifier? ident = null;
        var q = _repositry.GetAll(x => x.Guid == id);
        var isExists = q.Any();
        if (isExists) ident = q.Select(s => s.Id).First();
        return ValueTask.FromResult(ident);
    }

    #endregion


    #endregion


    #region Helpers
    private IDbTransaction currentTransaction;
    public IDbTransaction BeginTransaction(IsolationLevel? isolationLevel = null)
    {
        currentTransaction = _repositry.BeginTransaction();
        return currentTransaction;
    }
    public void BeginTransaction(IDbTransaction currentTransaction)
    {
        _repositry.BeginTransaction(currentTransaction);
    }
    public bool CommitTransaction()
    {
        return _repositry.CommitTransaction();
    }
    public void RolebackTransaction()
    {
        _repositry.RolebackTransaction();
    }

    public ValueTask<TEn> Map(TTdo tdo)
    {
        throw new NotImplementedException();
    }

    public Expression<Func<TTdo, bool>> Build()
    {
        return PredicateBuilder.New<TTdo>(true);
    }
    public Expression<Func<TEn, bool>> Build2()
    {
        return PredicateBuilder.New<TEn>(true);
    }

    public ValueTask<decimal> GetSumAsync(Expression<Func<TTdo, decimal>> select, Expression<Func<TTdo, bool>>? predicate = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var selectX = _mapper.Map<Expression<Func<TEn, decimal>>>(select);
        var q = _repositry.GetAll(expression).Sum(selectX);
        return ValueTask.FromResult(q);
    }
    public async ValueTask<TResult?> GetMaxAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null)
    {
        var expression = _mapper.Map<Expression<Func<TEn, bool>>>(predicate);
        var selectX = _mapper.Map<Expression<Func<TEn, TResult>>>(select);
        var q = _repositry.GetAll(expression);
        if (await q.AnyAsync())
        {
            var x = q.Max(selectX);
            return x;
        }
        else
        {
            return default(TResult);
        }
    }

    public bool UpdateProperty<TProperty>(Expression<Func<TTdo, bool>> where, Func<TEn, TProperty> property, TProperty value)
    {
        if (_repositry is EfRepositryBase<TEn> ef)
        {
            /*
            var _property = _mapper.Map<Func<TEn, TProperty>>(property);
            var _func = _property;
            return ef.Update(x => x.SetProperty(_func!, value)); // 
            */
            //ef.Update(x=> x.SetProperty(property, value));
            var expression = _mapper.Map<Expression<Func<TEn, bool>>>(where);
            var q = _repositry.GetAll(expression);
            return ef.Update(q, x => x.SetProperty(property!, value)); // 
        }
        else
        {
            throw new NotImplementedException("UpdateProperty not found in this provider");
        }
    }

    #endregion

    public IServiceBase<TDto2, TEn2> MakeChildService<TDto2, TEn2>()
        where TDto2 : DtoBase
        where TEn2 : TEntity, new()
    {
        var __repo = _repositry.MakeChildRepositry<TEn2>();
        var __logger = _serviceProvider.GetRequiredService<ILogger<ServiceBase<TDto2, TEn2>>>();

        return new ServiceBase<TDto2, TEn2>(__logger, this._mapper, this._mapsterMapper, _serviceProvider);
    }

    public TService MakeChildService<TService>()
    {
        var serivce = _serviceProvider.GetRequiredService<TService>();
        return serivce;
    }

    public ValueTask<TResult> ExcuteQuery<TResult>(string query, params object[] objects)
    {
        return _repositry.Query<TResult>(query, objects);
    }
    public ValueTask<IEnumerable<TResult>> ExcuteListQuery<TResult>(string query, params object[] objects)
    {
        return _repositry.QueryList<TResult>(query, objects);
    }

}
