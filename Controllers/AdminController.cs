using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.HttpRequestHandler;
using bodybykhoshalApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using bodybykhoshalApi.Models.ViewModel;

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
    [Authorize]
    [HttpPost("readAllMessages")]
    public IActionResult readAllMessages(GetAdminChatWithCustomerRequestHandler request)
    {
      var user = _adminService.readAllMessages(request);
      return Ok(new { Success = user });
    }
    [Authorize]
    [HttpGet("getCustomersBookings")]
    public IActionResult getCustomersBookings()
    {
      var userClaim = getClaims();
      var UserGUID = userClaim.UserGUID;
      var user = _adminService.getCustomersBookings(UserGUID);
      return Ok(user);
    }
    [Authorize]
    [HttpPost("approveAndRejectBooking")]
    public IActionResult approveAndRejectBooking(ApproveAndRejectBookingRequestHandler request)
    {
      var userClaim = getClaims();
      request.UserGuid = userClaim.UserGUID;
      var user = _adminService.approveAndRejectBooking(request);
      return Ok(user);
    }
    [Authorize]
    [HttpGet("getAllCustomerPackages")]
    public IActionResult getAllCustomerPackages()
    {
      var user = _adminService.getAllCustomerPackages();
      return Ok(user);
    }
    [Authorize]
    [HttpPost("paymentApproved")]
    public IActionResult paymentApproved(paymentApprovedRequestHandler request)
    {
      var userClaim = getClaims();
      request.UserGuid = userClaim.UserGUID;
      var user = _adminService.paymentApproved(request);
      return Ok(user);
    }
    [Authorize]
    [HttpPost("addSession")]
    public IActionResult addSession(addSessionRequestHandler request)
    {
      var userClaim = getClaims();
      request.UserId = userClaim.UserGUID;
      var user = _adminService.addSession(request);
      return Ok(user);
    }
    [Authorize]
    [HttpGet("completeSession/{BookingId}")]
    public IActionResult completeSession(int BookingId)
    {
      var user = _adminService.completeSession(BookingId);
      return Ok(user);
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
    [HttpGet("GetPackages")]
    public IActionResult GetPackages()
    {
      var packages = _adminService.GetPackages();
      return Ok(packages);
    }
    [Authorize]
    [HttpPost("SavePackage")]
    public IActionResult SavePackage(PackagesViewModel request)
    {
      var userClaim = getClaims();
      request.UserId = userClaim.UserGUID;
      var packages = _adminService.SavePackage(request);
      return Ok(packages);
    }
    [Authorize]
    [HttpGet("DeletePackage/{PackageId}")]
    public IActionResult DeletePackage(int PackageId)
    {
      var user = _adminService.DeletePackage(PackageId);
      return Ok(user);
    }
  }
}
