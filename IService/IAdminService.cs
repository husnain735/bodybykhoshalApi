using bodybykhoshalApi.Models.ViewModel;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

namespace bodybykhoshalApi.IService
{
    public interface IAdminService
    {
        List<UserViewModel> GetAllCustomers();
        List<ChatsViewModel> GetAdminChatWithCustomer(GetAdminChatWithCustomerRequestHandler request);
        bool saveChatForAdmin(SaveChatRequestHandler request);
    }
}
