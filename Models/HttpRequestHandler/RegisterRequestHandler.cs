namespace bodybykhoshalApi.Models.HttpRequestHandler
{
    public class RegisterRequestHandler
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
    public class ApproveAndRejectBookingRequestHandler
    {
        public int? BookinId { get; set; }
        public string? UserGuid { get; set; }
        public int? StatusId { get; set; }
    }
    public class paymentApprovedRequestHandler
    {
        public int? ShoppingCartId { get; set; }
        public string? UserGuid { get; set; }
        public int? StatusId { get; set; }
    }
    public class saveGoogleTokenRequestHandler
    {
        public string UserId { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string redirectURL { get; set; }
        public string tokenEndpoint { get; set; }
        public string code { get; set; }
        public string scopeURL { get; set; }
        public string scope { get; set; }

    }
}
