using bodybykhoshalApi.Models.ViewModel;

namespace bodybykhoshalApi.IService
{
    public interface IAdminService
    {
        List<UserViewModel> GetAllCustomers();
    }
}
