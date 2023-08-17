using bodybykhoshalApi.Models.HttpRequestHandler;
using bodybykhoshalApi.Models.ViewModel;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

namespace bodybykhoshalApi.IService
{
  public interface IAdminService
  {
    List<UserViewModel> GetAllCustomers();
    List<ChatsViewModel> GetAdminChatWithCustomer(GetAdminChatWithCustomerRequestHandler request);
    bool saveChatForAdmin(SaveChatRequestHandler request);
    bool readAllMessages(GetAdminChatWithCustomerRequestHandler request);
    List<BookinViewModel> getCustomersBookings(string UserGuid);
    int approveAndRejectBooking(ApproveAndRejectBookingRequestHandler request);
    List<ShoppingCartViewModel> getAllCustomerPackages();
    bool paymentApproved(paymentApprovedRequestHandler request); 
  }
}
