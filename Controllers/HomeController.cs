using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

namespace bodybykhoshalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHomeService _homeService;
        public HomeController(IConfiguration configuration, IHomeService homeService)
        {
            _configuration = configuration;
            _homeService = homeService;
        }

        private Users getClaims()
        {
            Users usr = new Users();
            ClaimsPrincipal user = User;
            if (user.Identity.IsAuthenticated)
            {
                Claim UserGuidClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                string UserGuid = UserGuidClaim?.Value;

                usr.UserGUID = UserGuid;


                return usr;
            }
            return usr;
        }

        [HttpGet("GetPackages")]
        public IActionResult GetPackages()
        {
            var packages = _homeService.GetPackages();
            return Ok(packages);
        }
        [Authorize]
        [HttpGet("GetPackage/{PackageId}")]
        public IActionResult GetPackage(int PackageId)
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var packages = _homeService.GetPackage(PackageId, UserGUID);
            return Ok(packages);
        }

        [Authorize]
        [HttpGet("AddToCart/{PackageId}")]
        public IActionResult AddToCart(int PackageId)
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var user = _homeService.AddToCart(PackageId,UserGUID);
            return Ok(new { Success = user });
        }
        [Authorize]
        [HttpGet("GetChatWithAdmin")]
        public IActionResult GetChatWithAdmin()
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var user = _homeService.GetChatWithAdmin(UserGUID);
            return Ok(new { Success = user });
        }
        [Authorize]
        [HttpPost("saveChat")]
        public IActionResult saveChat(SaveChatRequestHandler request)
        {
            var userClaim = getClaims();
            request.SenderOne = userClaim.UserGUID;
            var user = _homeService.SaveChat(request);
            return Ok(new { Success = user });
        }
        
    }
}
