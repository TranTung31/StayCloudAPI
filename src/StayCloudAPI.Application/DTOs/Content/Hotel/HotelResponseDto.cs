using StayCloudAPI.Core.Domain.Entities;

namespace StayCloudAPI.Application.DTOs.Content.Hotel
{
    public class HotelResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public float Rating { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int RoomCount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
