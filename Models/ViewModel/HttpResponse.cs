namespace bodybykhoshalApi.Models.ViewModel
{
    public class HttpResponse
    {
        public class LoginResponseHandler
        {
            public int? RoleId { get; set; }
            public string? Token { get; set; }
            public bool? Success { get; set; }
        }
    }
}
