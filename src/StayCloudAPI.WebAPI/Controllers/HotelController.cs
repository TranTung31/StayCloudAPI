using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayCloudAPI.Application.DTOs.Content.Hotel;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Core.Domain.Entities;

namespace StayCloudAPI.WebAPI.Controllers
{
    [Route("api/v1/hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelController(IHotelRepository hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelsAsync([FromQuery] string? searchValue, int page, int pageSize)
        {
            var result = await _unitOfWork.Hotel.GetHotelsAsync(searchValue, page, pageSize);
            return Ok(new
            {
                IsSuccess = true,
                Message = "Successfully!",
                Data = result,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelByIdAsync(Guid id)
        {
            var hotel = await _unitOfWork.Hotel.GetByIdAsync(id);

            if (hotel == null) return NotFound();

            return Ok(_mapper.Map<HotelResponseDto>(hotel));
        }

        [HttpPost]
        public async Task<IActionResult> AddHotelAsync([FromBody] HotelRequestDto hotel)
        {
            var hotelEntity = _mapper.Map<Hotel>(hotel);

            _unitOfWork.Hotel.Add(hotelEntity);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotelAsync(Guid id, [FromBody] HotelRequestDto hotelRequestDto)
        {
            var hotel = await _unitOfWork.Hotel.GetByIdAsync(id);

            if (hotel == null) return NotFound();

            _mapper.Map(hotelRequestDto, hotel);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotelAsync(Guid id)
        {
            var hotel = await _unitOfWork.Hotel.GetByIdAsync(id);

            if (hotel == null) return NotFound();

            _unitOfWork.Hotel.Remove(hotel);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }
    }
}
