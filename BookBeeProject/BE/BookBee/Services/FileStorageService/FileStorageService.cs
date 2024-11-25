using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BookBee.Services.FileStorageService
{
    public class FileStorageService/* : IFileStorageService*/
    {
        //private readonly Cloudinary _cloudinary;

        //public FileStorageService(IConfiguration config)
        //{
        //    var account = new Account(config["CloudinarySettings:CloudName"], config["CloudinarySettings:ApiKey"], config["CloudinarySettings:ApiSecret"]);
        //    _cloudinary = new Cloudinary(account);
        //}

        //public async Task<string?> UploadImage(IFormFile? file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return null;
        //    }

        //    await using var stream = file.OpenReadStream();
        //    var uploadParams = new ImageUploadParams
        //    {
        //        File = new FileDescription(file.FileName, stream),
        //        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
        //    };
        //    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        //    return uploadResult.Url.ToString();
        //}

        //public async Task DeleteImage(string? url)
        //{
        //    if (url == null)
        //    {
        //        return;
        //    }
        //    var publicId = url.Split("/").Last();
        //    var deleteParams = new DeletionParams(publicId);
        //    await _cloudinary.DestroyAsync(deleteParams);
        //}


        
    }
}
