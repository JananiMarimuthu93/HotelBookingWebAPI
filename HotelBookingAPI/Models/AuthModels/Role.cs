namespace HotelBookingAPI.Models.AuthModels
{

    public class Role
    {
        public string RoleId { get; set; } = string.Empty;   // e.g. R001, R002
        public string RoleName { get; set; } = string.Empty; // e.g. Admin, Doctor, Patient

        // Navigation
        public ICollection<User>? Users { get; set; } = new List<User>();
    }
}
