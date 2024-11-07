using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StayCloudAPI.Application.DTOs.Content.ProfileDto;
using StayCloudAPI.Application.Interfaces.Content.ICloudinary;
using StayCloudAPI.Core.Domain.Identity;
using StayCloudAPI.Infrastructure;
using StayCloudAPI.WebAPI.Extensions;
using System.Reflection.Metadata.Ecma335;

namespace StayCloudAPI.WebAPI.Controllers
{
    [Route("api/v1/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly StayCloudAPIDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICloudinaryRepository _cloudinaryRepository;
        private readonly IMapper _mapper;

        public ProfileController(
            StayCloudAPIDbContext context,
            UserManager<AppUser> userManager,
            ICloudinaryRepository cloudinaryRepository,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _cloudinaryRepository = cloudinaryRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return NotFound();

            var result = _mapper.Map<ProfileResponseDto>(user);

            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfileAsync(Guid userId, ProfileRequestDto request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(user.Avatar))
            {
                var lstFileNames = ConvertLstUrlsExtensions.ConvertLstUrls(user.Avatar.Split(",").ToList());
                await _cloudinaryRepository.DeleteImages(lstFileNames);
            }

            _mapper.Map(request, user);

            if (request.Avatar != null)
            {
                var image = await _cloudinaryRepository.UploadSingleImage(request.Avatar);
                user.Avatar = image;
            } else
            {
                user.Avatar = string.Empty;
            }

            var result = await _context.SaveChangesAsync();
            return result == 1 ? Ok(result) : BadRequest();
        }
    }
}
