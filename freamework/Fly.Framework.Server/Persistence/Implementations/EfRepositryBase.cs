using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore.Query;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Server.Persistence.Contexts;
using Fly.Framework.Shared.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data;

namespace Fly.Framework.Server.Persistence.Implementations;


internal class EfRepositryBase : IRepositryBase
{

    private readonly DbCustomeContext _aplicationContext;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger _logger;

    public EfRepositryBase(DbCustomeContext dbApplicationContext, ILogger<EfRepositryBase> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        this.serviceProvider = serviceProvider;
        _aplicationContext = dbApplicationContext;
    }

    private IDbContextTransaction? dbContextTransaction = null;
    public System.Data.IDbTransaction? DbTransaction => dbContextTransaction?.GetDbTransaction();

    public System.Data.IDbTransaction BeginTransaction(IsolationLevel? isolation = null)
    {
        if (dbContextTransaction is null)
        {
            if (isolation is null)
                dbContextTransaction = _aplicationContext.Database.BeginTransaction();
            else
                dbContextTransaction = _aplicationContext.Database.BeginTransaction(isolation.Value);
        }
        return DbTransaction!;
    }
    public void BeginTransaction(System.Data.IDbTransaction currentTransaction)
    {
        if (currentTransaction is IDbContextTransaction contextTransaction)
        {
            dbContextTransaction = contextTransaction;
        }
        else
        {
            dbContextTransaction = _aplicationContext.Database.UseTransaction((DbTransaction)currentTransaction);
        }
    }
    public bool CommitTransaction()
    {
        //  thow excption
        if (dbContextTransaction is null) return false;
        try
        {
            dbContextTransaction.Commit();
            return true;
        }
        catch
        {
            dbContextTransaction.Rollback();
            return false;
        }
    }
    /**/
    /**/
    public async ValueTask BeginTransactionAsync(bool recreate = false)
    {
        if (dbContextTransaction is null || recreate)
            dbContextTransaction = await _aplicationContext.Database.BeginTransactionAsync();
    }
    /**/
    public async ValueTask<bool> CommitTransactionAsync()
    {
        if (dbContextTransaction is not null)
        {
            try
            {
                await dbContextTransaction.CommitAsync();
                return true;
            }
            catch
            {
                await dbContextTransaction.RollbackAsync();
                return false;
            }
            finally
            {
                dbContextTransaction.Dispose();
                dbContextTransaction = null;
            }
        }
        return false;
    }
    public void RolebackTransaction()
    {
        if (dbContextTransaction is null) return;
        DbTransaction!.Rollback();
    }
    /**/
    /**/
    public async ValueTask<TResult?> Query<TResult>(string query, params object[] objects)
    {
        return await _aplicationContext.Database
            .SqlQueryRaw<TResult>(query, objects)
            .FirstOrDefaultAsync();
    }
    public async ValueTask<IEnumerable<TResult>> QueryList<TResult>(string query, params object[] objects)
    {
        return await _aplicationContext.Database
            .SqlQueryRaw<TResult>(query, objects)
            .ToListAsync();
    }
    /**/

}

internal class EfRepositryBase<T> : EfRepositryBase, IRepositryBase<T>
    where T : TEntity
{
    /**/
    /**/
    private readonly DbCustomeContext _aplicationContext;
    private readonly ILogger<EfRepositryBase<T>> _logger;
    private readonly IServiceProvider serviceProvider;

    /**/
    /**/
    public Microsoft.EntityFrameworkCore.DbSet<T> DbSet => _aplicationContext.Set<T>();
    public DbCustomeContext ApplicationContext => _aplicationContext;
    public Guid DatabaseAccesorId => _aplicationContext.ContextUniqeId;
    /**/
    /**/
    public EfRepositryBase(DbCustomeContext dbApplicationContext, ILogger<EfRepositryBase<T>> logger, IServiceProvider serviceProvider)
        : base(dbApplicationContext, logger, serviceProvider)
    {
        _logger = logger;
        this.serviceProvider = serviceProvider;
        _aplicationContext = dbApplicationContext;
    }
    /**/
    /**/
    public ExpressionStarter<T> CreatePredicateBuilder()
    {
        var builder = PredicateBuilder.New<T>(true);
        return builder;
    }
    /**/
    /**/
    /**/
    /**/
    public IQueryable<T> GetAll(Expression<Func<T, bool>>? where = null,
        int? page = null,
        int? limit = null,
        bool tracking = false,
        string[]? includes = null,
        Expression<Func<T, object>>[]? navigationIncludes = null,
        Expression<Func<T, object>>? descendingOrderBy = null,
        Expression<Func<T, object>>? orderBy = null)
    {
        //
        IQueryable<T> query = DbSet;
        //
        if (where != null)
            query = query.Where(where);
        //
        if (tracking)
            query = query.AsTracking();
        else
            query = query.AsNoTracking();
        //
        if (includes != null && includes.Any())
        {
            foreach (var a in includes)
            {
                query = query.Include(a);
            }
        }
        //
        if (includes != null && includes.Any())
        {
            foreach (var a in includes)
            {
                query = query.Include(a);
            }
        }
        //
        if (navigationIncludes != null && navigationIncludes.Any())
        {
            foreach (var a in navigationIncludes)
            {
                query = query.Include(a);
            }
        }
        //
        if (orderBy is not null)
            query = query.OrderBy(orderBy);
        else if (descendingOrderBy is not null)
            query = query.OrderByDescending(descendingOrderBy);
        else
            query = query.OrderByDescending(x => x.Id);
        //
        if (page != null & limit != null)
            query = query
                .Skip((page!.Value - 1) * limit!.Value)
                .Take(limit.Value);
        //
        return query;
    }
    /**/
    public IQueryable<T> GetById(Identifier id, Expression<Func<T, bool>>? where = null, int? page = null, int? limit = null, bool tracking = false, string[]? includes = null)
    {
        var w = CreatePredicateBuilder();
        w.And(x => x.Id == id);
        var q = GetAll(w, page, limit, tracking, includes);
        return q;
    }
    /**/
    /**/
    public IQueryable<T> GetById(Guid id, Expression<Func<T, bool>>? where = null, int? page = null, int? limit = null, bool tracking = false, string[]? includes = null)
    {
        var w = CreatePredicateBuilder();
        w.And(x => x.Guid == id);
        var q = GetAll(w, page, limit, tracking, includes);
        return q;
    }
    /**/
    /**/
    public async ValueTask<bool> Add(T item, bool save = false)
    {
        await DbSet.AddAsync(item);
        if (save)
            return await Save();
        return false;
    }
    /**/
    public ValueTask<bool> Update(T item, bool save = false)
    {
        //DbSet(item);
        var ent = _aplicationContext.Entry(item);
        ent.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        if (save)
            return Save();
        return ValueTask.FromResult(false);
    }
    /**/
    public bool Update(IQueryable<T> q, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateRole)
    {
        return q.ExecuteUpdate(updateRole) > 0;
    }
    /**/
    public ValueTask<bool> Remove(T item, bool save = false)
    {
        DbSet.Remove(item);
        if (save)
            return Save();
        return ValueTask.FromResult(false);
    }
    /**/
    public async ValueTask<bool> RemoveById(Identifier id, bool save = false)
    {
        return await DbSet.Where(w => w.Id == id).ExecuteDeleteAsync() == 1;
    }
    /**/
    public async ValueTask<bool> RemoveById(Guid id, bool save = false)
    {
        return await DbSet.Where(w => w.Guid == id).ExecuteDeleteAsync() == 1;
    }
    /**/
    public async ValueTask<bool> SoftRemoveById(Identifier id, bool save = false)
    {
        return await DbSet.Where(w => w.Id == id).ExecuteUpdateAsync(opt => opt.SetProperty(pr => pr.IsDeleted, true)) == 1;
    }
    /**/
    public async ValueTask<bool> SoftRemoveById(Guid id, bool save = false)
    {
        return await DbSet.Where(w => w.Guid == id).ExecuteUpdateAsync(opt => opt.SetProperty(pr => pr.IsDeleted, true)) == 1;
    }
    /**/
    public async ValueTask<bool> Save()
    {
        var changes = await _aplicationContext.SaveChangesAsync();
        // todo: test some cases, it's not stable solution
        var all = _aplicationContext.ChangeTracker.Entries();
        foreach (var ent in all)
        {
            ent.State = EntityState.Detached;
        }

        return changes > 0;
        //try
        //{
        //    return ValueTask.FromResult();
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Save new Entity");

        //    //throw ex;
        //    return ValueTask.FromResult(false);
        //}
    }
    /**/
    /**/
    //private IDbContextTransaction? transaction;
    //public ValueTask BeginTransaction(bool recreate = false)
    //{
    //    IDbContextTransaction Create() => _aplicationContext.Database.BeginTransaction();
    //    if (transaction is null)
    //        transaction = Create();
    //    else
    //    {
    //        if (recreate) transaction = Create();
    //    }
    //    return ValueTask.CompletedTask;TransactionId = {ba5b725c-4a09-45c5-83e1-45def620a5e2}
    //}



    public IRepositryBase<TChild> MakeChildRepositry<TChild>()
        where TChild : TEntity, new()
    {
        var __logger = serviceProvider.GetRequiredService<ILogger<EfRepositryBase<TChild>>>();

        return new EfRepositryBase<TChild>(this.ApplicationContext, __logger, serviceProvider);
    }
    /**/
    /**/
}
