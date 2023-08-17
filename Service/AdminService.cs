using AutoMapper;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.HttpRequestHandler;
using bodybykhoshalApi.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

namespace bodybykhoshalApi.Service
{
  public class AdminService : IAdminService
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public AdminService(ApplicationDbContext dbContext, IMapper mapper)
    {
      _dbContext = dbContext;
      _mapper = mapper;
    }
    public List<UserViewModel> GetAllCustomers()
    {
      try
      {
        var users = new List<UserViewModel>();

        users = _dbContext.Users.Where(x => x.RoleId == 2 && x.IsDeleted == false).Select(x => new UserViewModel
        {
          RoleId = x.RoleId,
          Email = x.Email,
          FirstName = x.FirstName,
          LastName = x.LastName,
          UserGUID = x.UserGUID,
        }).ToList();

        foreach (var item in users)
        {
          var IsNotify = _dbContext.Chats.Where(x => x.SenderOne == item.UserGUID && x.IsRead == false).ToList();
          if (IsNotify.Count() > 0)
          {
            item.TotalNotification = IsNotify.Count();
            item.IsNotify = true;
          }
          else
          {
            item.TotalNotification = IsNotify.Count();
            item.IsNotify = false;
          }
        }


        return users;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public List<ChatsViewModel> GetAdminChatWithCustomer(GetAdminChatWithCustomerRequestHandler request)
    {
      try
      {
        var chats = (from c in _dbContext.Chats
                     where (c.SenderOne == request.SenderOne && c.SenderTwo == request.SenderTwo)
        || (c.SenderOne == request.SenderTwo && c.SenderTwo == request.SenderOne)
                     orderby c.ChatId ascending
                     select new ChatsViewModel
                     {
                       Content = c.Content,
                       Timestamp = c.Timestamp,
                       SenderName = c.SenderName,
                       RoleId = c.RoleId
                     }).ToList();
        return chats;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public bool saveChatForAdmin(SaveChatRequestHandler request)
    {
      try
      {
        var sendUserObj = _dbContext.Users.Where(x => x.UserGUID == request.SenderOne).FirstOrDefault();
        char firstChar = char.ToUpper(sendUserObj.FirstName[0]);
        char secondChar = char.ToUpper(sendUserObj.LastName[0]);
        string combinedLetters = $"{firstChar}{secondChar}";


        request.Timestamp = DateTime.Now;
        request.SenderName = combinedLetters;
        request.SenderTwo = request.SenderTwo;
        request.RoleId = sendUserObj.RoleId;
        request.IsRead = false;

        var chat = _mapper.Map<Chats>(request);
        _dbContext.Chats.Add(chat);
        _dbContext.SaveChanges();
        return true;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public bool readAllMessages(GetAdminChatWithCustomerRequestHandler request)
    {
      try
      {
        var chats = _dbContext.Chats.Where(x => x.SenderOne == request.SenderOne && x.IsRead == false).ToList();
        if (chats.Count > 0)
        {
          foreach (var item in chats)
          {
            item.IsRead = true;
          }
          _dbContext.UpdateRange(chats);
          _dbContext.SaveChanges();
          return true;
        }
        return false;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public List<BookinViewModel> getCustomersBookings(string UserGuid)
    {
      try
      {
        string format = "yyyy-MM-dd HH:mm:ss";

        var bookings = _dbContext.Booking.Where(x => x.UserId != UserGuid).Select(x => new BookinViewModel
        {
          Id = x.BookingId,
          Title = x.Title,
          StartDate = x.StartDate,
          EndDate = x.EndDate,
          Details = x.Details,
          StatusId = x.StatusId,
          CreatedDate = x.CreatedDate,
          IsDeleted = x.IsDeleted,
          UserId = x.UserId,
        }).ToList();

        foreach (var booking in bookings)
        {

          booking.Start = booking.StartDate.HasValue
? booking.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
: "<not available>";

          booking.End = booking.EndDate.HasValue
? booking.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
: "<not available>";
        }


        return bookings;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public int approveAndRejectBooking(ApproveAndRejectBookingRequestHandler request)
    {
      try
      {
        var booking = _dbContext.Booking.Where(x => x.BookingId == request.BookinId).FirstOrDefault();
        if (booking != null)
        {
          booking.StatusId = request.StatusId;
          _dbContext.Update(booking);
          _dbContext.SaveChanges();
          return booking.BookingId;
        }
        return Convert.ToInt32(request.BookinId);
      }
      catch (Exception)
      {

        throw;
      }
    }
    public List<ShoppingCartViewModel> getAllCustomerPackages()
    {
      try
      {
        var res = (from sc in _dbContext.ShoppingCart
                   join p in _dbContext.Packages on sc.PackageId equals p.PackagesId
                   join u in _dbContext.Users on sc.UserId equals u.UserGUID
                   select new ShoppingCartViewModel
                   {
                     ShoppingCartId = sc.ShoppingCartId,
                     FirstName = u.FirstName,
                     LastName = u.LastName,
                     PackageName = p.PackageName,
                     TotalNumberOfSessions = p.TotalNumberOfSessions,
                     StatusId = sc.StatusId,
                     TotalPrice = p.TotalPrice,
                     PhoneNumber = u.PhoneNumber,
                     Email = u.Email
                   }).ToList();


        return res;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public bool paymentApproved(paymentApprovedRequestHandler request)
    {
      try
      {
        var cart = _dbContext.ShoppingCart.Where(x => x.ShoppingCartId == request.ShoppingCartId).FirstOrDefault();
        if (cart != null)
        {
          cart.StatusId = request.StatusId;
          _dbContext.Update(cart);
          _dbContext.SaveChanges();
          return true;
        }
        return false;
      }
      catch (Exception)
      {

        throw;
      }
    }
  }
}
