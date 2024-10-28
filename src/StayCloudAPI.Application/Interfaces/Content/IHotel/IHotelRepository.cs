using StayCloudAPI.Application.DTOs;
using StayCloudAPI.Application.DTOs.Content.Hotel;
using StayCloudAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayCloudAPI.Application.Interfaces.Content.IHotel
{
    public interface IHotelRepository : IRepository<Hotel, Guid>
    {
        Task<PagedResult<HotelResponseDto>> GetHotelsAsync(string? searchValue, int page, int pageSize);
    }
}
