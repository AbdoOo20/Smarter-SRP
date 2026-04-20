using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Shared.Common;
using Fly.Framework.Shared.Common.Collections;
using Fly.Framework.Shared.Common.Dtos;
using Fly.Framework.Shared.Common.ValueTypes;
using System.Data;
using System.Linq.Expressions;

namespace Fly.Framework.Server.Persistence;


public interface IServiceBase<TDto>
    where TDto : DtoBase
{

    ValueTask<TDto> CreateAsync(TDto dto);
    ValueTask<TDto> UpdateAsync(TDto dto);
    ValueTask<TDto> RemoveAsync(TDto dto);
    ValueTask<TDto> GetByIdAsync(Guid id);
    ValueTask<TDto> GetAllAsync(Expression<Func<TDto, bool>> where);

}

public interface IServiceBase<TTdo, TEn>
    where TTdo : DtoBase
    where TEn : TEntity
{

    bool UseSoftDelete { get; }
    IRepositryBase<TEn> Repositry { get; }

    ValueTask<TTdo?> Add(TTdo entity);
    ValueTask<TTdo?> AddT(TEn db, TTdo entityDto);
    ValueTask<IEnumerable<TTdo>> AddRange(IEnumerable<TTdo> entities);

    ValueTask<TTdo?> Update(TTdo entity);
    ValueTask<TTdo?> UpdateT(TEn entity, TTdo entityDto);
    ValueTask<IEnumerable<TTdo>> UpdateRange(IEnumerable<TTdo> entities);

    ValueTask<TTdo> Remove(TTdo entity, bool? useSoftDelete = null);
    ValueTask Remove(Expression<Func<TTdo, bool>>? predicate = null, bool? useSoftDelete = null);
    ValueTask<IEnumerable<TTdo>> RemoveRange(IEnumerable<TTdo> entities);

    ValueTask<TTdo> Remove(Identifier id, bool? useSoftDelete = null);
    ValueTask<IEnumerable<Identifier>> RemoveRange(Identifier[] id, bool? useSoftDelete = null);

    ValueTask<TTdo?> GetById(Identifier id);
    ValueTask<TEn?> GetTById(Identifier id);

    ValueTask<PageList<TTdo>> GetAllAsync(Expression<Func<TTdo, bool>>? predicate = null, int? page = null, int? limit = null);

    ValueTask<TEn> Map(TTdo tdo);
    ValueTask<bool> BeforeUpdate(TEn entity);
    ValueTask<bool> AfterUpdate(TEn entity);
    ValueTask<IEnumerable<TTdo>> AddRange(IEnumerable<TEn> entities);
    ValueTask<TTdo?> GetByGuId(Guid id);
    ValueTask<bool> Exists(Identifier id);
    ValueTask<Identifier?> GetId(Guid id);
    ValueTask<bool> AnyAsync(Expression<Func<TTdo, bool>>? predicate = null);
    //ValueTask<bool> AnyAsync(Expression<Func<TEn, bool>>? predicate = null);
    ValueTask<PageList<TResult>> GetAllAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null, int? page = null, int? limit = null);
    ValueTask<PageList<TResult>> GetAllAsEntityAsync<TResult>(Expression<Func<TEn, TResult>> select, Expression<Func<TEn, bool>>? predicate = null, int? page = null, int? limit = null);
    ValueTask<decimal> GetSumAsync(Expression<Func<TTdo, decimal>> select, Expression<Func<TTdo, bool>>? predicate = null);
    ValueTask<TResult?> GetMaxAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null);
    ValueTask<PageList<SelectDataItem<Identifier>>> GetSelecListAsync(Func<TTdo, Identifier> selectId, Func<TTdo, string> text, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null);
    ValueTask<PageList<DefaultLookup>> GetDefaultLookupAsync(Func<TTdo, Identifier> selectId, Func<TTdo, string> text, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null);
    ValueTask<PageList<DefaultLookup>> GetSelecListAsync(Func<TTdo, string> text, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null);
    ValueTask<TTdo?> GetAsync(Expression<Func<TTdo, bool>>? predicate = null, int? page = null, int? limit = null);
    ValueTask<TResult?> GetOneAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null);
    Expression<Func<TTdo, bool>> Build();
    ValueTask<PageList<TResult>> GetTopListAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, int take = 100, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null, Expression<Func<TTdo, object>>? orderBy = null);
    ValueTask<TResult?> GetTopOneAsAsync<TResult>(Expression<Func<TTdo, TResult>> select, Expression<Func<TTdo, bool>>? predicate = null, Expression<Func<TTdo, object>>? descendingOrderBy = null, Expression<Func<TTdo, object>>? orderBy = null);
    ValueTask<long> NextNumber(Expression<Func<TTdo, long>> select);
    bool UpdateProperty<TProperty>(Expression<Func<TTdo, bool>> where, Func<TEn, TProperty> property, TProperty value);
    ValueTask<TTdo?> AddOrUpdateAsync(TTdo dto);
    Expression<Func<TEn, bool>> Build2();
    IDbTransaction BeginTransaction(IsolationLevel? isolationLevel = null);
    bool CommitTransaction();
    IServiceBase<TDto2, TEn2> MakeChildService<TDto2, TEn2>()
        where TDto2 : DtoBase
        where TEn2 : TEntity, new();
    TService MakeChildService<TService>();
    void BeginTransaction(IDbTransaction currentTransaction);


    //bool UpdateProperty<TPropery>(Expression<Func<TTdo, TPropery>> property, TPropery value);



    event EventHandler<TTdo> Added;
    event EventHandler<TTdo> Updated;
    event EventHandler<TTdo> Deleted;

    ValueTask<TResult> ExcuteQuery<TResult>(string query, params object[] objects);
    ValueTask<IEnumerable<TResult>> ExcuteListQuery<TResult>(string query, params object[] objects);
    void RolebackTransaction();
}
