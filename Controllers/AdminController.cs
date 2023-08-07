using bodybykhoshalApi.IService;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var packages = _adminService.GetAllCustomers();
            return Ok(packages);
        }
    }
}
