using AutoMapper;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.ViewModel;
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
