using StayCloudAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayCloudAPI.Application.DTOs.Content.Hotel
{
    public class HotelRequestDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public float Rating { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int RoomCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
