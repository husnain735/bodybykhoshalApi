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
}
