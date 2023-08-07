using bodybykhoshalApi.Models.HttpRequestHandler;
using static bodybykhoshalApi.Models.ViewModel.HttpResponse;

namespace bodybykhoshalApi.IService
{
    public interface IAuthenticationService
    {
        string CreateUser(RegisterRequestHandler requestHandler);
        LoginResponseHandler LoginUser(LoginRequestHandler requestHandler);
    }
}
