using HotelBookingAPI.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI.DTOs
{
    public class BookingCreateDto
    {
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Range(1, 4)]
        public int NumberOfGuests { get; set; }
        [Range(0, 999999.99)]
        public decimal TotalAmount { get; set; }
        [Required]
        public int GuestId { get; set; }
        [Required]
        public int RoomId { get; set; }
    }

    public class BookingUpdateDto
    {
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Range(1, 4)]
        public int NumberOfGuests { get; set; }
        [Range(0, 999999.99)]
        public decimal TotalAmount { get; set; }
        [Required]
        public int GuestId { get; set; }
        [Required]
        public int RoomId { get; set; }

        [Required]
        public BookingStatus Status { get; set; }
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
