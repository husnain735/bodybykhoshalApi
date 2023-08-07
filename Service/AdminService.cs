using AutoMapper;
using bodybykhoshalApi.Context;
using bodybykhoshalApi.IService;
using bodybykhoshalApi.Models.ViewModel;

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
    }
}
