using Fly.Framework.Shared.Common.Http;

namespace Fly.Framework.Shared.Common.Features;

public interface IFeatureBase
{
}

public interface IFeatureBase<TDto> : IFeatureBase
{
    ValueTask<HttpResponseWrapper<TDto?>> GetOneAsync(Guid Id);
    ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync(TDto dto);
    ValueTask<HttpResponseWrapper<TDto>> DeleteRemoveAsync(Guid guid, Func<Task>? onExceptionAction = null);
    ValueTask<HttpResponseWrapperList<TDto>> GetAllAsync(int? page = null, int? limit = null, string? text = null, Dictionary<string, object[]>? filter = null);
}

public interface IFeatureBase<TDto, TGetAllQuery>
    : IFeatureBase
{
    ValueTask<HttpResponseWrapper<TDto?>> GetOneAsync(Guid Id);
    ValueTask<HttpResponseWrapperList<TDto>> GetAllByQueryAsync(TGetAllQuery getAllQuery);
}

public interface IFeatureBase<TDto, TGetAllQuery, TDeleteCommand>
    : IFeatureBase<TDto, TGetAllQuery>
{
    ValueTask<HttpResponseWrapper<TDto?>> DeleteRemoveAsync(TDeleteCommand guid);
}

public interface IFeatureBase<TDto, TGetAllQuery, TDeleteCommand, TCreateCommand, TUpdateCommand>
    : IFeatureBase<TDto, TGetAllQuery, TDeleteCommand>
{
    ValueTask<HttpResponseWrapper<TDto?>> PostCreateAsync(TCreateCommand dto);
    ValueTask<HttpResponseWrapper<TDto?>> PostUpdateAsync(TUpdateCommand dto);
}

public interface IFeatureBase<TDto, TGetAllQuery, TDeleteCommand, TCreateOrUpdateCommand>
    : IFeatureBase<TDto, TGetAllQuery, TDeleteCommand>
{
    ValueTask<HttpResponseWrapper<TDto?>> PostSaveAsync(TCreateOrUpdateCommand dto);
}