using AutoMapper;
using StayCloudAPI.Application.DTOs.Content.Hotel;
using StayCloudAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayCloudAPI.Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Hotel
            CreateMap<Hotel, HotelRequestDto>().ReverseMap();
            CreateMap<Hotel, HotelResponseDto>().ReverseMap();
        }
    }
}
