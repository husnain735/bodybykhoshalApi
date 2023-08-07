using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.HttpRequestHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bodybykhoshalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IConfiguration configuration, IAuthenticationService authenticationService)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestHandler request)
        {
            var tokenString = _authenticationService.LoginUser(request);
            return Ok(new { Token = tokenString });
        }
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(RegisterRequestHandler request)
        {
            var user = _authenticationService.CreateUser(request);
            return Ok(new { Success = user });
        }
    }
}
