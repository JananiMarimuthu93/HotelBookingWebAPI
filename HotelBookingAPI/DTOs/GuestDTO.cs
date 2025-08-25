namespace HotelBookingAPI.DTOs
{
    // DTO for creating a new Guest
    public class GuestCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    // DTO for updating an existing Guest
    public class GuestUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    // DTO for reading Guest information (response)
    public class GuestReadDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Optional: number of bookings or list of booking IDs
        public int TotalBookings { get; set; }
    }
}
