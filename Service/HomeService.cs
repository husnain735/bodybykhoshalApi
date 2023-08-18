using AutoMapper;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace bodybykhoshalApi.Service
{
  public class HomeService : IHomeService
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public HomeService(ApplicationDbContext dbContext, IMapper mapper)
    {
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public List<PackagesViewModel> GetPackages()
    {
      try
      {
        var packages = _dbContext.Packages.Where(x => x.IsDeleted == false).Select(x => new PackagesViewModel
        {
          Description = x.Description,
          PackagesId = x.PackagesId,
          PricePerSession = x.PricePerSession,
          TotalNumberOfSessions = x.TotalNumberOfSessions,
          TotalPrice = x.TotalPrice,
          CreatedDate = x.CreatedDate,
          IsDeleted = x.IsDeleted,
          OrderId = x.OrderId,
          PackageName = x.PackageName,
          UserId = x.UserId
        }).ToList();

        return packages;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public PackagesViewModel GetPackage(int PackageId, string userGuid)
    {
      try
      {
        var packages = new PackagesViewModel();
        var cart = _dbContext.ShoppingCart.Where(x => x.UserId == userGuid && x.StatusId == 2).FirstOrDefault();
        if (cart != null)
        {
          PackageId = Convert.ToInt32(cart.PackageId);
        }
        if (PackageId == 0 && cart == null)
        {
          return packages;
        }
        packages = _dbContext.Packages.Where(x => x.PackagesId == PackageId).Select(x => new PackagesViewModel
        {
          Description = x.Description,
          PackagesId = x.PackagesId,
          PricePerSession = x.PricePerSession,
          TotalNumberOfSessions = x.TotalNumberOfSessions,
          TotalPrice = x.TotalPrice,
          CreatedDate = PackageId != 0 && cart != null ? cart.CreatedDate : x.CreatedDate,
          IsDeleted = x.IsDeleted,
          OrderId = x.OrderId,
          PackageName = x.PackageName,
          UserId = x.UserId
        }).FirstOrDefault();

        return packages;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public bool AddToCart(int PackageId, string userGuid)
    {
      try
      {
        var checkSub = _dbContext.ShoppingCart.Where(x => x.UserId == userGuid && x.StatusId == 1).FirstOrDefault();
        if (checkSub != null)
        {
          return false;
        }
        var package = _dbContext.Packages.Where(x => x.PackagesId == PackageId).Select(x => new PackagesViewModel
        {
          Description = x.Description,
          PackagesId = x.PackagesId,
          PricePerSession = x.PricePerSession,
          TotalNumberOfSessions = x.TotalNumberOfSessions,
          TotalPrice = x.TotalPrice,
          CreatedDate = x.CreatedDate,
          IsDeleted = x.IsDeleted,
          OrderId = x.OrderId,
          PackageName = x.PackageName,
          UserId = x.UserId
        }).FirstOrDefault();

        if (package != null)
        {
          ShoppingCart cart = new ShoppingCart
          {
            UserId = userGuid,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false,
            PackageId = PackageId,
            StatusId = 1,
            TotalSessions = package.TotalNumberOfSessions
          };

          _dbContext.Add(cart);
          _dbContext.SaveChanges();
        }

        return true;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public List<ChatsViewModel> GetChatWithAdmin(string userGuid)
    {
      try
      {
        var adminUserGUID = _dbContext.Users.Where(x => x.RoleId == 1).FirstOrDefault();
        var chats = (from c in _dbContext.Chats
                     where (c.SenderOne == userGuid && c.SenderTwo == adminUserGUID.UserGUID)
        || (c.SenderOne == adminUserGUID.UserGUID && c.SenderTwo == userGuid)
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
    public bool SaveChat(SaveChatRequestHandler request)
    {
      try
      {
        var sendUserObj = _dbContext.Users.Where(x => x.UserGUID == request.SenderOne).FirstOrDefault();
        char firstChar = char.ToUpper(sendUserObj.FirstName[0]);
        char secondChar = char.ToUpper(sendUserObj.LastName[0]);
        string combinedLetters = $"{firstChar}{secondChar}";

        var receiverUserObj = _dbContext.Users.Where(x => x.RoleId == 1).FirstOrDefault();

        request.Timestamp = DateTime.Now;
        request.SenderName = combinedLetters;
        request.SenderTwo = receiverUserObj.UserGUID;
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
    public notificationViewModel getCustomerNotification(string userGuid)
    {
      try
      {
        var notification = new notificationViewModel();

        var IsNotify = _dbContext.Chats.Where(x => x.SenderTwo == userGuid && x.IsRead == false).ToList();
        if (IsNotify.Count() > 0)
        {
          notification.TotalNotification = IsNotify.Count();
          notification.IsNotify = true;
        }
        else
        {
          notification.TotalNotification = IsNotify.Count();
          notification.IsNotify = false;
        }
        return notification;
      }
      catch (Exception)
      {

        throw;
      }
    }
    public bool readAllMessages(string userGuid)
    {
      try
      {
        var chats = _dbContext.Chats.Where(x => x.SenderTwo == userGuid && x.IsRead == false).ToList();
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
    public List<BookinViewModel> getCustomerBookings(string UserGuid)
    {
      try
      {
        string format = "yyyy-MM-dd HH:mm:ss";

        var bookings = _dbContext.Booking.Where(x => x.UserId == UserGuid).Select(x => new BookinViewModel
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
    public BookinViewModel saveCustomerBooking(BookinViewModel request)
    {
      try
      {

        string format = "yyyy-MM-dd HH:mm:ss";
        var response = new BookinViewModel();
        var date = DateTime.ParseExact(request.Start, format, null);

                var checkBookings = _dbContext.Booking.Where(x => x.StartDate.HasValue && x.StartDate.Value.Date == date.Date && x.UserId == request.UserId).Count();
        if (checkBookings >= 2 && request.Id == 0)
        {
          return response;
        }
        if (request.Id != 0)
        {
          var booking = _dbContext.Booking.Where(x => x.UserId == request.UserId && x.BookingId == request.Id).FirstOrDefault();
          if (booking != null)
          {
            booking.StartDate = DateTime.ParseExact(request.Start, format, null);
            booking.EndDate = DateTime.ParseExact(request.End, format, null);
            booking.Details = request.Details;

            _dbContext.Update(booking);
            _dbContext.SaveChanges();

            response.Title = booking.Title;
            response.Id = request.Id;
            return response;
          }
          return response;

        }
        else
        {
          var Start = DateTime.ParseExact(request.Start, format, null);
          var End = DateTime.ParseExact(request.End, format, null);
          var checkBooking = _dbContext.Booking.Where(x => x.StartDate == Start && x.EndDate == End && x.UserId == request.UserId).FirstOrDefault();
          if (checkBooking != null)
          {
            response.Title = request.Title;
            response.Id = 0;
            return response;
          }

          request.StartDate = DateTime.ParseExact(request.Start, format, null);
          request.EndDate = DateTime.ParseExact(request.End, format, null);
          request.CreatedDate = DateTime.Now;
          request.StatusId = 1;
          request.IsDeleted = false;

          var booking = _mapper.Map<Booking>(request);
          _dbContext.Booking.Add(booking);
          _dbContext.SaveChanges();
          response = new BookinViewModel
          {
            Id = booking.BookingId,
            Title = booking.Title,
          };
          return response;
        }
      }
      catch (Exception)
      {

        throw;
      }

    }
        public PackagesViewModel GetCustomerPackage(string userGuid)
        {
            try
            {
                var package = new PackagesViewModel();
                var cart = _dbContext.ShoppingCart.Where(x => x.UserId == userGuid && x.StatusId == 2).FirstOrDefault();
                if (cart != null)
                {
                    package = (from sc in _dbContext.ShoppingCart
                                   join p in _dbContext.Packages on sc.PackageId equals p.PackagesId
                                   where cart.ShoppingCartId == sc.ShoppingCartId
                                   select new PackagesViewModel
                                   {
                                       Description = p.Description,
                                       PackagesId = p.PackagesId,
                                       PricePerSession = p.PricePerSession,
                                       TotalNumberOfSessions = p.TotalNumberOfSessions,
                                       TotalPrice = p.TotalPrice,
                                       CreatedDate = p.CreatedDate,
                                       IsDeleted = p.IsDeleted,
                                       OrderId = p.OrderId,
                                       PackageName = p.PackageName,
                                       UserId = p.UserId,
                                       StatusId = sc.StatusId,
                                       TotalSessions = sc.TotalSessions,
                                   }).FirstOrDefault();
                } else
                {
                    package.StatusId = 1;
                }
                
                return package;
            }
            catch (Exception)
            {

                throw;
            }
        }
  }
}
