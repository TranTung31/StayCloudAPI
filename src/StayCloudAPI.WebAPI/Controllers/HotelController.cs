using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayCloudAPI.Application.DTOs.Content.Hotel;
using StayCloudAPI.Application.DTOs.Content.HotelDto;
using StayCloudAPI.Application.Interfaces;
using StayCloudAPI.Application.Interfaces.Content.ICloudinary;
using StayCloudAPI.Application.Interfaces.Content.IHotel;
using StayCloudAPI.Core.Domain.Entities;

namespace StayCloudAPI.WebAPI.Controllers
{
    [Route("api/v1/hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ICloudinaryRepository _cloudinaryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelController(
            ICloudinaryRepository cloudinaryRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _cloudinaryRepository = cloudinaryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelsAsync([FromQuery] string? searchValue, int page, int pageSize)
        {
            var result = await _unitOfWork.Hotels.GetHotelsAsync(searchValue, page, pageSize);
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
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);

            if (hotel == null) return NotFound();

            return Ok(_mapper.Map<HotelResponseDto>(hotel));
        }

        [HttpPost]
        public async Task<IActionResult> AddHotelAsync(HotelRequestDto request)
        {
            var lstImages = await _cloudinaryRepository.UploadMultipleImages(request.ImageUrl);
            var hotelEntity = _mapper.Map<Hotel>(request);

            hotelEntity.ImageUrl = string.Join(",", lstImages);

            _unitOfWork.Hotels.Add(hotelEntity);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotelAsync(Guid id, HotelRequestDto request)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);

            if (hotel == null) return NotFound();

            if (!string.IsNullOrEmpty(hotel.ImageUrl))
            {
                var lstFileNames = ConvertLstUrls(hotel.ImageUrl.Split(",").ToList());
                await _cloudinaryRepository.DeleteImages(lstFileNames);
            }

            _mapper.Map(request, hotel);

            if (request.ImageUrl.Count > 0)
            {
                var lstImages = await _cloudinaryRepository.UploadMultipleImages(request.ImageUrl);
                hotel.ImageUrl = string.Join(",", lstImages);
            } else
            {
                hotel.ImageUrl = string.Empty;
            }

            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Ok(result) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotelAsync(Guid id)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);

            if (hotel == null) return NotFound();

            if (!string.IsNullOrEmpty(hotel.ImageUrl))
            {
                var lstImages = ConvertLstUrls(hotel.ImageUrl.Split(",").ToList());
                await _cloudinaryRepository.DeleteImages(lstImages);
            }

            _unitOfWork.Hotels.Remove(hotel);

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
