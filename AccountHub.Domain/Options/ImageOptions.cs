namespace AccountHub.Domain.Options;

public class ImageOptions
{
    public required string ApiKey { get; set; }
    public required string SecretKey { get; set; }
    public required string CloudName { get; set; }

}