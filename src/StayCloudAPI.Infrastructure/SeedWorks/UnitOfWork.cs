using AutoMapper;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Application.Interfaces.Content.IRoom;
using StayCloudAPI.Infrastructure.Implements.Content.HotelImplement;
using StayCloudAPI.Infrastructure.Implements.Content.RoomImplement;

namespace StayCloudAPI.Infrastructure.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StayCloudAPIDbContext _context;
        public IHotelRepository Hotels { get; private set; }
        public IRoomRepository Rooms { get; private set; }

        public UnitOfWork(StayCloudAPIDbContext context, IMapper mapper)
        {
            _context = context;
            Hotels = new HotelRepository(context, mapper);
            Rooms = new RoomRepository(context, mapper);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
