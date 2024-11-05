using Microsoft.AspNetCore.Http;

namespace StayCloudAPI.Application.Interfaces.Content.ICloudinary
{
    public interface ICloudinaryRepository
    {
        Task<string> UploadSingleImage(IFormFile file);
        Task<List<string>> UploadMultipleImages(List<IFormFile> files);
        Task<int> DeleteImages(List<string> images);
    }
}
