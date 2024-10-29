using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Application.Interfaces.Content.IRoom;

namespace StayCloudAPI.Application.Interfaces
{
    // Gói tất cả repositories vào object duy nhất, để có thể sử dụng ở nhiều nơi
    public interface IUnitOfWork : IDisposable
    {
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        Task<int> CompleteAsync();
    }
}
