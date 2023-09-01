using AutoMapper;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.HttpRequestHandler;
using bodybykhoshalApi.Models.ViewModel;
using static bodybykhoshalApi.Models.HttpResponseHandler.HttpResponseHandler;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.EntityFrameworkCore;

namespace bodybykhoshalApi.Service
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly string _clientId = "816323560279-pdp5qk5dbo7bcg7ekmm35jd74l747634.apps.googleusercontent.com";
        private readonly string _clientSecret = "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";
        private readonly HttpClient _httpClient;

        public AdminService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = new HttpClient();
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
                                 RoleId = c.RoleId,
                                 ChatType = c.ChatType,
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

                var bookings = _dbContext.Booking.Where(x => x.UserId != UserGuid && x.IsDeleted == false).Select(x => new BookinViewModel
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
        public approveAndRejectBookingResponseHandler approveAndRejectBooking(ApproveAndRejectBookingRequestHandler request)
        {
            try
            {
                var response = new approveAndRejectBookingResponseHandler();

                var booking = _dbContext.Booking.Where(x => x.BookingId == request.BookinId).FirstOrDefault();
                if (booking != null)
                {
                    var cart = _dbContext.ShoppingCart.Where(x => x.ShoppingCartId == booking.ShoppingCartId).FirstOrDefault();
                    if (cart != null && cart.TotalSessions > 0)
                    {
                        booking.StatusId = request.StatusId;
                        _dbContext.Update(booking);
                        _dbContext.SaveChanges();
                        response.Id = booking.BookingId;
                        response.Success = true;
                        return response;
                    }
                    response.Success = false;
                    return response;
                }
                response.Success = false;
                return response;
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
                               TotalNumberOfSessions = sc.TotalSessions,
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
        public bool addSession(addSessionRequestHandler request)
        {
            try
            {
                var cart = _dbContext.ShoppingCart.Where(x => x.ShoppingCartId == request.ShoppingCartId).FirstOrDefault();
                if (cart != null)
                {
                    cart.TotalSessions += request.SessionCount;
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
        public bool completeSession(int BookingId)
        {
            try
            {
                var booking = _dbContext.Booking.Where(x => x.BookingId == BookingId).FirstOrDefault();
                if (booking != null)
                {
                    booking.StatusId = 4;
                    var cart = _dbContext.ShoppingCart.Where(x => x.ShoppingCartId == booking.ShoppingCartId).FirstOrDefault();
                    if (cart != null)
                    {
                        if (cart.TotalSessions > 0)
                        {
                            cart.TotalSessions -= 1;
                        }
                        _dbContext.Update(booking);
                        _dbContext.Update(cart);
                        _dbContext.SaveChanges();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
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
        public bool SavePackage(PackagesViewModel request)
        {
            try
            {
                if (request.PackagesId == 0 || request.PackagesId == null)
                {
                    request.IsDeleted = false;
                    request.CreatedDate = DateTime.Now;
                    var package = _mapper.Map<Packages>(request);
                    _dbContext.Add(package);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var package = _dbContext.Packages.Where(x => x.PackagesId == request.PackagesId).FirstOrDefault();
                    if (package != null)
                    {
                        package.PackageName = request.PackageName;
                        package.TotalNumberOfSessions = request.TotalNumberOfSessions;
                        package.TotalPrice = request.TotalPrice;
                        package.PricePerSession = request.PricePerSession;
                        package.Description = request.Description;
                        package.OrderId = request.OrderId;
                        _dbContext.Update(package);
                        _dbContext.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeletePackage(int PackageId)
        {
            try
            {
                var package = _dbContext.Packages.Where(x => x.PackagesId == PackageId).FirstOrDefault();
                if (package != null)
                {
                    package.IsDeleted = true;
                    _dbContext.Update(package);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public syncwithGoogleResponseHandler syncwithGoogle(saveGoogleTokenRequestHandler request)
        {
            try
            {
                var response = new syncwithGoogleResponseHandler();

                var userObj = _dbContext.Users.Where(x => x.UserGUID == request.UserId).FirstOrDefault();
                if (userObj != null)
                {
                    if (userObj.GoogleCalendarToken != null)
                    {

                    }
                    else
                    {
                        string scopeURL = request.scopeURL;
                        var redirectURL = request.redirectURL;
                        string prompt = "consent";
                        string response_type = "code";
                        string clientID = request.clientId;
                        string scope = request.scope;
                        string access_type = "offline";
                        string redirect_uri_encode = ExtensionMethod.ExtensionMethod.urlEncodeForGoogle(redirectURL);
                        var mainURL = string.Format(scopeURL, redirect_uri_encode, prompt, response_type, clientID, scope, access_type);
                        response.RedirectUrl = mainURL;
                        response.Success = true;
                        return response;
                    }
                }
                response.RedirectUrl = null;
                response.Success = false;
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> saveGoogleToken(saveGoogleTokenRequestHandler request)
        {
            try
            {
                var clientId = request.clientId;
                string clientSecret = request.clientSecret;
                var redirectURL = request.redirectURL;
                var tokenEndpoint = request.tokenEndpoint;
                var content = new StringContent($"code={request.code}&redirect_uri={Uri.EscapeDataString(redirectURL)}&client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await _httpClient.PostAsync(tokenEndpoint, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);
                    //var userObj = _dbContext.Users.Where(x => x.UserGUID == userId).FirstOrDefault(); 
                    //if (userObj != null)
                    //{
                    //userObj.GoogleCalendarToken = tokenResponse.refresh_token;
                    //_dbContext.Update(userObj);
                    //_dbContext.SaveChanges();

                    var varifiedBookings = await _dbContext.Booking.Where(x => x.StatusId == 2 && x.IsDeleted == false && x.IsAdminGoogleBooking == null).ToListAsync();
                    if (varifiedBookings.Count() > 0)
                    {
                        foreach (var booking in varifiedBookings)
                        {
                            var googleCalendarReqDTO = new GoogleCalendarReqDTO();
                            googleCalendarReqDTO.Summary = booking.Title;
                            googleCalendarReqDTO.StartTime = Convert.ToDateTime(booking.StartDate);
                            googleCalendarReqDTO.EndTime = Convert.ToDateTime(booking.EndDate);
                            googleCalendarReqDTO.refreshToken = tokenResponse.refresh_token;
                            googleCalendarReqDTO.Description = booking.Details;
                            AddToGoogleCalendar(googleCalendarReqDTO);

                            booking.IsAdminGoogleBooking = true;
                            _dbContext.Update(booking);
                            _dbContext.SaveChanges();
                        }
                    }
                    //}
                    return 1;
                }
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string AddToGoogleCalendar(GoogleCalendarReqDTO googleCalendarReqDTO)
        {
            try
            {
                var token = new TokenResponse { RefreshToken = googleCalendarReqDTO.refreshToken };
                var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = new ClientSecrets { ClientId = _clientId, ClientSecret = _clientSecret }
                    }), "user", token);


                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                });

                Event newEvent = new Event()
                {
                    Summary = googleCalendarReqDTO.Summary,
                    Description = googleCalendarReqDTO.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = googleCalendarReqDTO.StartTime,
                        //TimeZone = Method.WindowsToIana();    //users time zone
                    },
                    End = new EventDateTime()
                    {
                        DateTime = googleCalendarReqDTO.EndTime,
                        //TimeZone = Method.WindowsToIana();    //users time zone
                    },
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[] {
                                    new EventReminder() { Method = "email", Minutes = 30 },
                                    new EventReminder() { Method = "popup", Minutes = 15 },
                                    new EventReminder() { Method = "popup", Minutes = 1 },
                                }
                    }
                };

                EventsResource.InsertRequest insertRequest = service.Events.Insert(newEvent, "primary");
                Event createdEvent = insertRequest.Execute();
                return createdEvent.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }

        }
    }
}
