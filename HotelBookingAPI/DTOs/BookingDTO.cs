using HotelBookingAPI.Models.DomainModels;

namespace HotelBookingAPI.DTOs
{
    public class BookingCreateDto
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public int GuestId { get; set; }
        public int RoomId { get; set; }
    }

    public class BookingUpdateDto
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
    }

    public class BookingReadDto
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
    }
}
