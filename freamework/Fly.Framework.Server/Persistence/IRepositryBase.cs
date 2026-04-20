using System.Data;
using System.Linq.Expressions;
using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using LinqKit;

namespace Fly.Framework.Server.Persistence;

public interface IDatabaseContext
{
    System.Data.IDbTransaction BeginTransaction();

}

public interface IRepositryBase
{


    bool CommitTransaction();
    void RolebackTransaction();
    ValueTask<bool> CommitTransactionAsync();
    ValueTask BeginTransactionAsync(bool recreate = false);
    void BeginTransaction(IDbTransaction currentTransaction);
    System.Data.IDbTransaction BeginTransaction(IsolationLevel? isolation = null);


    ValueTask<TResult?> Query<TResult>(string query, params object[] objects);
    ValueTask<IEnumerable<TResult>> QueryList<TResult>(string query, params object[] objects);
}
public interface IRepositryBase<T> : IRepositryBase
    where T : TEntity
{
    Guid DatabaseAccesorId { get; }

    

    IQueryable<T> GetAll(Expression<Func<T, bool>>? where = null, int? page = null, int? limit = null, bool tracking = false, string[]? includes = null, Expression<Func<T, object>>[]? navigationIncludes = null, Expression<Func<T, object>>? descendingOrderBy = null, Expression<Func<T, object>>? orderBy = null);

    IQueryable<T> GetById(Identifier id, Expression<Func<T, bool>>? where = null, int? page = null, int? limit = null, bool tracking = false, string[]? includes = null);
    IQueryable<T> GetById(Guid id, Expression<Func<T, bool>>? where = null, int? page = null, int? limit = null, bool tracking = false, string[]? includes = null);


    ValueTask<bool> Save();

    ValueTask<bool> Add(T item, bool save = false);

    ValueTask<bool> Remove(T item, bool save = false);

    ValueTask<bool> Update(T item, bool save = false);

    ValueTask<bool> RemoveById(Identifier id, bool save = false);
    ValueTask<bool> SoftRemoveById(Identifier id, bool save = false);
    ValueTask<bool> SoftRemoveById(Guid id, bool save = false);
    ValueTask<bool> RemoveById(Guid id, bool save = false);
    IRepositryBase<TChild> MakeChildRepositry<TChild>() where TChild : TEntity, new();

    ExpressionStarter<T> CreatePredicateBuilder();


}