using bodybykhoshalApi.Models.ViewModel;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

namespace bodybykhoshalApi.IService
{
  public interface IHomeService
  {
    List<PackagesViewModel> GetPackages();
    PackagesViewModel GetPackage(int PackageId, string userGuid);
    bool AddToCart(int PackageId, string userGuid);
    List<ChatsViewModel> GetChatWithAdmin(string userGuid);
    bool SaveChat(SaveChatRequestHandler request);
    notificationViewModel getCustomerNotification(string userGuid);
    bool readAllMessages(string userGuid);
    List<BookinViewModel> getCustomerBookings(string UserGuid);
    BookinViewModel saveCustomerBooking(BookinViewModel request);
        PackagesViewModel GetCustomerPackage(string userGuid);
  }
}
