using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StayCloudAPI.Application.DTOs;
using StayCloudAPI.Application.DTOs.Content.Hotel;
using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Core.Domain.Entities;
using StayCloudAPI.Infrastructure.SeedWorks;

namespace StayCloudAPI.Infrastructure.Implements.Content.HotelImplement
{
    public class HotelRepository : RepositoryBase<Hotel, Guid>, IHotelRepository
    {
        public HotelRepository(StayCloudAPIDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PagedResult<HotelResponseDto>> GetHotelsAsync(string? searchValue, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Hotels.AsQueryable();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => x.Name.Contains(searchValue.Trim()));
                }

                var totalCount = await query.CountAsync();

                query = query.OrderByDescending(x => x.DateCreated)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

                var result = new PagedResult<HotelResponseDto>
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Results = await _mapper.ProjectTo<HotelResponseDto>(query).ToListAsync(),
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
