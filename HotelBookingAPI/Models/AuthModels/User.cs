namespace HotelBookingAPI.Models.AuthModels
{
    public class User
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; } // store hashed password

        // Foreign Key
        public string? RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
