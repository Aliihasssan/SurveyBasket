using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Authentication;

namespace SurveyBasket.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService,IConfiguration configuration) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(SignInRequest request,CancellationToken cancellationToken=default)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password,cancellationToken);
            return authResult is null ? BadRequest("Invalid email/password") : Ok(authResult);
        }

        [HttpGet]
        public IActionResult Test()
        {
            var config = new
            {
                fd = _configuration["Key"],
                one = _configuration["oneDrive"]
        };
                return Ok(config);
        }
    }

}
