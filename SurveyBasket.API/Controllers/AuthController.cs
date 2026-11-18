using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.API.Contracts.Authentication;

namespace SurveyBasket.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService,IOptions<JwtOptions> options) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly JwtOptions _options = options.Value;
        //------------------------------------------------------
        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(SignInRequest request,CancellationToken cancellationToken=default)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password,cancellationToken);
            return authResult.IsFailure ? BadRequest(authResult.Error) : Ok(authResult.Value);
        }

        //=============================
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return authResult.IsFailure ? BadRequest(authResult.Error) : Ok(authResult.Value);
        }
        //============================
        //=============================
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return isRevoked.IsSuccess ? Ok() : BadRequest(isRevoked.Error);
        }
        //============================
    }

}
