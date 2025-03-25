using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Options;
using AccountHub.Domain.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace AccountHub.Infrastructure.Services;

public class ImageService:IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IOptions<ImageOptions> options)
    {
        var imageOptions = options.Value;
        var account = new Account(imageOptions.CloudName, imageOptions.ApiKey, imageOptions.SecretKey);
        _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
    }
    public async Task<string> UploadImage(string fileName,Stream file,CancellationToken cancellationToken)
    {
        var image = new FileDescription(fileName, file);
        var imageUploadParams = new ImageUploadParams()
        {
            File = image,
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Transformation = new Transformation()
                .FetchFormat("auto")
                .Quality("auto")
        };
        
        var result = await _cloudinary.UploadAsync(imageUploadParams,cancellationToken);
        if(result.Error != null)
            throw new ServiceException("Image uploading error",result.Error.Message);
        return result.Url.ToString(); 
    }

  

    public async Task<string> DeleteImage(string imageUrl)
    {
        var imageDestroyParams = new DeletionParams(imageUrl);
        var result = await _cloudinary.DestroyAsync(imageDestroyParams);
        return result.Result;
    }
}