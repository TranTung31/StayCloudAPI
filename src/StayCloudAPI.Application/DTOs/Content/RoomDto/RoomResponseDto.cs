namespace StayCloudAPI.Application.DTOs.Content.RoomDto
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Size { get; set; }
        public string View { get; set; } = string.Empty;
        public string BedType { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public Guid HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
    }
}
