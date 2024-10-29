using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StayCloudAPI.Application.DTOs;
using StayCloudAPI.Application.DTOs.Content.RoomDto;
using StayCloudAPI.Application.Interfaces.Content.IRoom;
using StayCloudAPI.Core.Domain.Entities;
using StayCloudAPI.Infrastructure.SeedWorks;

namespace StayCloudAPI.Infrastructure.Implements.Content.RoomImplement
{
    public class RoomRepository : RepositoryBase<Room, Guid>, IRoomRepository
    {
        public RoomRepository(StayCloudAPIDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PagedResult<RoomResponseDto>> GetRoomsAll(string? searchValue, int page, int pageSize)
        {
            try
            {
                var query = _context.Rooms.AsQueryable();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.Name.Contains(searchValue.Trim()));
                }

                var totalCount = await query.CountAsync();

                query = query.OrderByDescending(x => x.DateCreated)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

                return new PagedResult<RoomResponseDto>
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Results = await _mapper.ProjectTo<RoomResponseDto>(query).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
