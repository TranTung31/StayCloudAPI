using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayCloudAPI.Core.Domain.Entities
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Size { get; set; }
        public string View { get; set; } = string.Empty;
        public string BedType { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Guid HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
