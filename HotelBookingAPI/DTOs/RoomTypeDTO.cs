namespace HotelBookingAPI.DTOs
{
    public class RoomTypeCreateDTO
    {
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class RoomTypeUpdateDto
    {
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class RoomTypeReadDto
    {
       
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
