using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayCloudAPI.Core.Domain.Entities
{
    [Table("Hotels")]
    public class Hotel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public required string Address { get; set; }

        [Required]
        [MaxLength(250)]
        public required string City { get; set; }

        public float Rating { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        public int RoomCount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
