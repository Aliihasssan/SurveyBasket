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

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(SignInRequest request,CancellationToken cancellationToken=default)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password,cancellationToken);
            return authResult is null ? BadRequest("Invalid email/password") : Ok(authResult);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            var config = new
            {
                Aud = _options.Audience,
                ket = _options.Key
        };
                return Ok(config);
        }
    }

}
