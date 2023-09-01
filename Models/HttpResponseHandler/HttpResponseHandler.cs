namespace bodybykhoshalApi.Models.HttpResponseHandler
{
    public class HttpResponseHandler
    {
        public class approveAndRejectBookingResponseHandler
        {
            public int Id { get; set; }
            public bool Success { get; set; }
        }
        public class syncwithGoogleResponseHandler
        {
            public bool Success { get; set; }
            public string RedirectUrl { get; set; }
        }
    }
}
