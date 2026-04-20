using Fly.Framework.Shared.Common.Features;
using Fly.Framework.Shared.Common.Http;
using Fly.Shared.Modules.Marketers.Response;

namespace Fly.Shared.Modules.Subscription.Features;

public record SubscriperInfoResponse(string Token, string Name, string Phone, string Email, string Avatar);

public record SigningRequest(string Phone, string Password, bool IsMarketer = false);

public interface ISigningFeature : IFeatureBase
{
    ValueTask<HttpResponseWrapper<SubscriperInfoResponse>> Signing(SigningRequest request);
    ValueTask<HttpResponseWrapper<DtoUserInfo>> GetUserInfo();
}