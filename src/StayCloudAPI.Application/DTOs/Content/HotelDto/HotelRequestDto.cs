using Microsoft.AspNetCore.Http;

namespace StayCloudAPI.Application.DTOs.Content.Hotel
{
    public class HotelRequestDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public float Rating { get; set; }
        public List<IFormFile> ImageUrl { get; set; } = new List<IFormFile>();
        public int RoomCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
