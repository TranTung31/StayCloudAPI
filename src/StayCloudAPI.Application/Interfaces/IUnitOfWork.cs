using StayCloudAPI.Application.Interfaces.Content.IHotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayCloudAPI.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IHotelRepository Hotel { get; }
        Task<int> CompleteAsync();
    }
}
