using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingAPI.Models.DomainModels
{
    [Index(nameof(RoomNumber), IsUnique = true, Name = "IX_Room_RoomNumber")]
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Room Number is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Room Number must be between 1 and 10 characters.")]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20 guests.")]
        public int Capacity { get; set; }

        [Required]
        [Precision(10, 2)]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 30000, ErrorMessage = "Price per day must be between 0 and 30,000.")]
        public decimal PricePerDay { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        [StringLength(250, ErrorMessage = "Room description cannot exceed 250 characters.")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "Floor info cannot exceed 50 characters.")]
        public string? Floor { get; set; }

        [StringLength(50, ErrorMessage = "View type cannot exceed 50 characters.")]
        public string? ViewType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [InverseProperty("Room")]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [Required]
        public int RoomTypeId { get; set; }
        [ForeignKey(nameof(RoomTypeId))]
        public RoomType RoomType { get; set; } = null!;
    }
}
