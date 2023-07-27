using bodybykhoshalApi.Models.HttpRequestHandler;

namespace bodybykhoshalApi.IService
{
    public interface IAuthenticationService
    {
        string CreateUser(RegisterRequestHandler requestHandler);
        string LoginUser(LoginRequestHandler requestHandler);
    }
}
