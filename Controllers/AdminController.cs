using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

namespace bodybykhoshalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;
        public AdminController(IConfiguration configuration, IAdminService adminService)
        {
            _configuration = configuration;
            _adminService = adminService;
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
        [Authorize]
        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var packages = _adminService.GetAllCustomers();
            return Ok(packages);
        }
        [Authorize]
        [HttpPost("GetAdminChatWithCustomer")]
        public IActionResult GetAdminChatWithCustomer(GetAdminChatWithCustomerRequestHandler request)
        {
            var userClaim = getClaims();
            request.SenderOne = userClaim.UserGUID;
            var packages = _adminService.GetAdminChatWithCustomer(request);
            return Ok(packages);
        }
        [Authorize]
        [HttpPost("saveChatForAdmin")]
        public IActionResult saveChatForAdmin(SaveChatRequestHandler request)
        {
            var userClaim = getClaims();
            request.SenderOne = userClaim.UserGUID;
            var user = _adminService.saveChatForAdmin(request);
            return Ok(new { Success = user });
        }
    }
}
