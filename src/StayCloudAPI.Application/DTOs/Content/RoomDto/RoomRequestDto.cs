using Microsoft.AspNetCore.Http;

namespace StayCloudAPI.Application.DTOs.Content.RoomDto
{
    public class RoomRequestDto
    {
        public required string Name { get; set; }
        public List<IFormFile> ImageUrl { get; set; } = new List<IFormFile>();
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Size { get; set; }
        public string View { get; set; } = string.Empty;
        public string BedType { get; set; } = string.Empty;
        public Guid HotelId { get; set; }
    }
}
