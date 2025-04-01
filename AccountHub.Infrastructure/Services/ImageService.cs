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
        var imageId = ExtractPublicId(imageUrl);
        var imageDestroyParams = new DeletionParams(imageId);
        var result = await _cloudinary.DestroyAsync(imageDestroyParams);
        return result.Result;
    }
    private string ExtractPublicId(string url)
    {
        // Remove any query string parameters.
        string cleanUrl = url.Split('?')[0];

        // Find the "/upload/" segment.
        const string uploadSegment = "/upload/";
        int uploadIndex = cleanUrl.IndexOf(uploadSegment, StringComparison.Ordinal);
        if (uploadIndex == -1)
            return null; // URL doesn't match expected Cloudinary pattern

        // Position right after "/upload/"
        int startIndex = uploadIndex + uploadSegment.Length;

        // Check for a version segment (e.g., "v1647610701/")
        if (cleanUrl[startIndex] == 'v')
        {
            int slashAfterVersion = cleanUrl.IndexOf('/', startIndex);
            if (slashAfterVersion > 0)
            {
                startIndex = slashAfterVersion + 1;
            }
        }

        // Extract the remainder of the URL (public id with extension)
        string idWithExtension = cleanUrl.Substring(startIndex);

        // Remove the file extension by finding the last dot.
        int dotIndex = idWithExtension.LastIndexOf('.');
        if (dotIndex > 0)
            return idWithExtension.Substring(0, dotIndex);
    
        return idWithExtension;
    }

    
}