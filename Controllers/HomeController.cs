using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;
using System.Net.Http.Headers;
using bodybykhoshalApi.Models.HttpRequestHandler;
using bodybykhoshalApi.Service;

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
                Claim UserNameClaim = user.FindFirst(ClaimTypes.Name);

                string UserGuid = UserGuidClaim?.Value;
                string UserName = UserNameClaim?.Value;


                usr.UserGUID = UserGuid;
                usr.FirstName = UserName;

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
            var user = _homeService.AddToCart(PackageId, UserGUID);
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
        [Authorize]
        [HttpGet("getCustomerNotification")]
        public IActionResult getCustomerNotification()
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var user = _homeService.getCustomerNotification(UserGUID);
            return Ok(new { Success = user });
        }
        [Authorize]
        [HttpGet("readAllMessages")]
        public IActionResult readAllMessages()
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var user = _homeService.readAllMessages(UserGUID);
            return Ok(new { Success = user });
        }
        [Authorize]
        [HttpGet("getCustomerBookings")]
        public IActionResult getCustomerBookings()
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var user = _homeService.getCustomerBookings(UserGUID);
            return Ok(user);
        }
        [Authorize]
        [HttpPost("saveCustomerBooking")]
        public IActionResult saveCustomerBooking(BookinViewModel request)
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            request.UserId = UserGUID;
            request.Title = userClaim.FirstName;
            var user = _homeService.saveCustomerBooking(request);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("GetCustomerPackage")]
        public IActionResult GetCustomerPackage()
        {
            var userClaim = getClaims();
            var UserGUID = userClaim.UserGUID;
            var packages = _homeService.GetCustomerPackage(UserGUID);
            return Ok(packages);
        }
        [Authorize]
        [Route("UploadChat")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadChat()
        {
            var file = Request.Form.Files[0];
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var path = Path.Combine(Directory.GetCurrentDirectory(), "chatImages");
            var url = path;
            bool exists = Directory.Exists(url);
            if (!exists)
            {
                Directory.CreateDirectory(url);
            }
            var fullPath = Path.Combine(url, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Ok("chatImages/" + fileName);
        }
        [Authorize]
        [HttpGet("syncwithGoogle")]
        public IActionResult syncwithGoogle()
        {
            var userClaim = getClaims();

            var request = new saveGoogleTokenRequestHandler
            {
                UserId = userClaim.UserGUID,
                scopeURL = _configuration.GetValue<string>("scopeURL"),
                redirectURL = _configuration.GetValue<string>("redirectClientURL"),
                clientId = _configuration.GetValue<string>("clientId"),
                scope = _configuration.GetValue<string>("scope"),
            };

            var user = _homeService.syncwithGoogle(request);
            return Ok(user);
        }
        [HttpGet("saveGoogleToken")]
        public async Task<IActionResult> saveGoogleToken()
        {
            string code = HttpContext.Request.Query["code"];
            string scope = HttpContext.Request.Query["scope"];
            var request = new saveGoogleTokenRequestHandler
            {
                code = code,
                scope = scope,
                redirectURL = _configuration.GetValue<string>("redirectClientURL"),
                clientId = _configuration.GetValue<string>("clientId"),
                clientSecret = _configuration.GetValue<string>("clientSecret"),
                tokenEndpoint = _configuration.GetValue<string>("tokenEndpoint"),
            };
            var token = await _homeService.saveGoogleToken(request);
            return Ok(token);
        }
    }
}
