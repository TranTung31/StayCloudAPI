using AutoMapper;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Infrastructure.Implements.Content.HotelImplement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayCloudAPI.Infrastructure.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StayCloudAPIDbContext _context;

        public UnitOfWork(StayCloudAPIDbContext context, IMapper mapper)
        {
            _context = context;
            Hotel = new HotelRepository(context, mapper);
        }

        public IHotelRepository Hotel { get; private set; }

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
