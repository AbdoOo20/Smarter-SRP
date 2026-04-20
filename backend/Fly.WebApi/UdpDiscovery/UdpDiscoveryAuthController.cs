using Microsoft.AspNetCore.Mvc;

namespace Fly.WebApi.UdpDiscovery;

[ApiController]
[Route("udp-auth")]
public class UdpDiscoveryAuthController : ControllerBase
{
    private static readonly UdpDiscoveryAuth Auth = new();

    [HttpGet("challenge")]
    public IActionResult Challenge()
        => Ok(new { nonce = Auth.CreateChallenge() });

    [HttpPost("verify")]
    public IActionResult Verify([FromBody] VerifyReq req)
    {
        var token = Auth.VerifyChallenge(req.Nonce, req.Response);
        return token is null
            ? Unauthorized()
            : Ok(new { token });
    }

    public record VerifyReq(string Nonce, string Response);
}