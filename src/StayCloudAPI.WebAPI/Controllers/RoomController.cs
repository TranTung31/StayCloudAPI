using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayCloudAPI.Application.DTOs.Content.RoomDto;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Application.Interfaces.Content.ICloudinary;
using StayCloudAPI.Core.Domain.Entities;

namespace StayCloudAPI.WebAPI.Controllers
{
    [Route("api/v1/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ICloudinaryRepository _cloudinaryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomController(ICloudinaryRepository cloudinaryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cloudinaryRepository = cloudinaryRepository;
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
        public async Task<IActionResult> CreateRoomAsync(RoomRequestDto roomRequestDto)
        {
            var lstImages = await _cloudinaryRepository.UploadMultipleImages(roomRequestDto.ImageUrl);
            var room = _mapper.Map<Room>(roomRequestDto);

            room.ImageUrl = string.Join(",", lstImages);

            _unitOfWork.Rooms.Add(room);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAsync(Guid id, RoomRequestDto request)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room == null) return NotFound();

            if (!string.IsNullOrEmpty(room.ImageUrl))
            {
                var lstFileNames = ConvertLstUrls(room.ImageUrl.Split(",").ToList());
                await _cloudinaryRepository.DeleteImages(lstFileNames);
            }

            _mapper.Map(request, room);

            if (request.ImageUrl.Count() > 0)
            {
                var lstImages = await _cloudinaryRepository.UploadMultipleImages(request.ImageUrl);
                room.ImageUrl = string.Join(",", lstImages);
            }
            else
            {
                room.ImageUrl = string.Empty;
            }

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotelAsync(Guid id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room == null) return NotFound();

            if (!string.IsNullOrEmpty(room.ImageUrl))
            {
                var lstImages = ConvertLstUrls(room.ImageUrl.Split(",").ToList());
                await _cloudinaryRepository.DeleteImages(lstImages);
            }

            _unitOfWork.Rooms.Remove(room);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        public static List<string> ConvertLstUrls(List<string> lstUrls)
        {
            var result = new List<string>();

            foreach (var url in lstUrls)
            {
                var fileType = url.Split("/")[^1];
                var fileName = fileType.Split(".")[0];
                result.Add(fileName);
            }

            return result;
        }
    }
}
