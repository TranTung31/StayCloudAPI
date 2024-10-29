using StayCloudAPI.Application.DTOs;
using StayCloudAPI.Application.DTOs.Content.RoomDto;
using StayCloudAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayCloudAPI.Application.Interfaces.Content.IRoom
{
    public interface IRoomRepository : IRepository<Room, Guid>
    {
        Task<PagedResult<RoomResponseDto>> GetRoomsAll(string? searchValue, int page, int pageSize);
    }
}
