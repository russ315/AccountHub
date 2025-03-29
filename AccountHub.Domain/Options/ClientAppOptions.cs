namespace AccountHub.Domain.Options;

public class ClientAppOptions
{
    public required string Url { get; init; }
    public required string ApiEndpoint { get; set; }
}