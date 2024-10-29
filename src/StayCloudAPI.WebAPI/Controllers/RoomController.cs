using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayCloudAPI.Application.DTOs.Content.RoomDto;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Core.Domain.Entities;

namespace StayCloudAPI.WebAPI.Controllers
{
    [Route("api/v1/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels([FromQuery] string? searchValue, int page, int pageSize)
        {
            var result = await _unitOfWork.Rooms.GetRoomsAll(searchValue, page, pageSize);
            return Ok(new
            {
                IsSuccess = true,
                Message = "Successfully!",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(Guid id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room == null) return NotFound();

            var hotel = await _unitOfWork.Hotels.GetByIdAsync(room.HotelId);

            var result = _mapper.Map<RoomResponseDto>(room);

            if (hotel.Name != null) result.HotelName = hotel.Name;

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomAsync([FromBody] RoomRequestDto roomRequestDto)
        {
            var room = _mapper.Map<Room>(roomRequestDto);

            _unitOfWork.Rooms.Add(room);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAsync(Guid id, RoomRequestDto roomRequestDto)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room == null) return NotFound();

            _mapper.Map(roomRequestDto, room);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotelAsync(Guid id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room == null) return NotFound();

            _unitOfWork.Rooms.Remove(room);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }
    }
}
