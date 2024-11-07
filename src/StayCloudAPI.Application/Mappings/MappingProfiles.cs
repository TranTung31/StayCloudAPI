using AutoMapper;
using StayCloudAPI.Application.DTOs.Content.Hotel;
using StayCloudAPI.Application.DTOs.Content.HotelDto;
using StayCloudAPI.Application.DTOs.Content.ProfileDto;
using StayCloudAPI.Application.DTOs.Content.RoomDto;
using StayCloudAPI.Core.Domain.Entities;
using StayCloudAPI.Core.Domain.Identity;

namespace StayCloudAPI.Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Hotel
            CreateMap<Hotel, HotelRequestDto>().ReverseMap();
            CreateMap<Hotel, HotelResponseDto>().ReverseMap();

            // Room
            CreateMap<Room, RoomRequestDto>().ReverseMap();
            CreateMap<Room, RoomResponseDto>().ReverseMap();

            // Profile
            CreateMap<AppUser, ProfileResponseDto>().ReverseMap();
            CreateMap<AppUser, ProfileRequestDto>().ReverseMap();
        }
    }
}
