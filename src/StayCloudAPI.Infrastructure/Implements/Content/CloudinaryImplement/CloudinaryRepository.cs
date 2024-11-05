using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StayCloudAPI.Application.Interfaces.Content.ICloudinary;
using StayCloudAPI.Core.ConfigOptions;

namespace StayCloudAPI.Infrastructure.Implements.Content.CloudinaryImplement
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryRepository(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadSingleImage(IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<List<string>> UploadMultipleImages(List<IFormFile> files)
        {
            var uploadUrls = new List<string>();

            foreach (var file in files)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                uploadUrls.Add(uploadResult.SecureUrl.ToString());
            }

            return uploadUrls;
        }

        public async Task<int> DeleteImages(List<string> images)
        {
            try
            {
                var deleteParams = new DelResParams()
                {
                    PublicIds = images,
                    Type = "upload",
                    ResourceType = ResourceType.Image
                };

                var result = await _cloudinary.DeleteResourcesAsync(deleteParams);

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
