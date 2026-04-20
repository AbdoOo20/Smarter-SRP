using Fly.Shared.Modules.Global.Dtos;
using Fly.Framework.Shared.Common.Http;
using Fly.Shared.Modules.Global.Requests;
using Fly.Framework.Shared.Common.Features;

namespace Fly.Shared.Modules.Global.Features;

public interface IConfigFeatures : IFeatureBase
{
    ValueTask<HttpResponseWrapper<DtoConfig>> PostSaveAsync(SettingsRequest request);
}
