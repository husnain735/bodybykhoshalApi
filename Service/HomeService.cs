using AutoMapper;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.ViewModel;
using System;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

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
                var cart = _dbContext.ShoppingCart.Where(x => x.UserId == userGuid).FirstOrDefault();
                if (cart != null)
                {
                    PackageId = Convert.ToInt32(cart.PackageId);
                }
                var packages = _dbContext.Packages.Where(x => x.PackagesId == PackageId).Select(x => new PackagesViewModel
                {
                    Description = x.Description,
                    PackagesId = x.PackagesId,
                    PricePerSession = x.PricePerSession,
                    TotalNumberOfSessions = x.TotalNumberOfSessions,
                    TotalPrice = x.TotalPrice,
                    CreatedDate = PackageId == 0 ? cart.CreatedDate : x.CreatedDate,
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
                        PackageId = PackageId
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
                String[] userGuids = new String[] { userGuid, adminUserGUID.UserGUID };
                var chats = (from c in _dbContext.Chats
                             join u in _dbContext.Users on c.UserId equals u.UserGUID
                             where userGuids.Contains(c.UserId)
                             orderby c.ChatId ascending
                             select new ChatsViewModel
                             {
                                 Content = c.Content,
                                 Timestamp = c.Timestamp,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 RoleId = u.RoleId
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
                request.Timestamp = DateTime.Now;
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
    }
}
