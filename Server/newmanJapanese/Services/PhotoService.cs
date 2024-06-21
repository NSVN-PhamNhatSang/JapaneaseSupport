using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using JLearning.Interfaces;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;

namespace JLearning.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();

            if (photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(800)
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }


    }
}
