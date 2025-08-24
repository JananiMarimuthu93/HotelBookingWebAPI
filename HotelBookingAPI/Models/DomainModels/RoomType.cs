using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingAPI.Models.DomainModels
{
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_RoomType_TypeName")]
    public class RoomType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Room Type Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Room Type Name must be between 3 and 50 characters.")]
        public string TypeName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [InverseProperty("RoomType")]
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
