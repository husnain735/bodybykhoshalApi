﻿namespace bodybykhoshalApi.Models.ViewModel
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string UserGUID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsNotify { get; set; }
        public int TotalNotification { get; set; }
    }
}
