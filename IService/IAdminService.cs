using bodybykhoshalApi.Models.HttpRequestHandler;
using bodybykhoshalApi.Models.ViewModel;
using static bodybykhoshalApi.Models.HttpResponseHandler.HttpResponseHandler;
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
        approveAndRejectBookingResponseHandler approveAndRejectBooking(ApproveAndRejectBookingRequestHandler request);
        List<ShoppingCartViewModel> getAllCustomerPackages();
        bool paymentApproved(paymentApprovedRequestHandler request);
        bool addSession(addSessionRequestHandler request);
        bool completeSession(int BookingId);
        List<PackagesViewModel> GetPackages();
        bool SavePackage(PackagesViewModel request);
        bool DeletePackage(int PackageId);
        syncwithGoogleResponseHandler syncwithGoogle(saveGoogleTokenRequestHandler request);
        Task<int> saveGoogleToken(saveGoogleTokenRequestHandler request);
    }
}
