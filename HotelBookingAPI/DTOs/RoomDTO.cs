namespace HotelBookingAPI.DTOs
{
    // DTO for creating a new room
    public class RoomCreateDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? Description { get; set; }
        public string? Floor { get; set; }
        public string? ViewType { get; set; }
        public int RoomTypeId { get; set; }
    }

    // DTO for updating an existing room
    public class RoomUpdateDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public string? Floor { get; set; }
        public string? ViewType { get; set; }
        public int RoomTypeId { get; set; }
    }

    // DTO for reading room details (response)
    public class RoomReadDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public string? Floor { get; set; }
        public string? ViewType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RoomTypeName { get; set; } = string.Empty; 
    }
}
