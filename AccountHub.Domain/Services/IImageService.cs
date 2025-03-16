

namespace AccountHub.Domain.Services;

public interface IImageService
{
    Task<string> UploadImage(string fileName,Stream file);

    Task<string> DeleteImage(string imageUrl);
    
}