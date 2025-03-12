using System.Net;

namespace AccountHub.Domain.Exceptions;

public class ForbiddenException:BaseException
{
    public ForbiddenException(string title, string message) : base(title, message)
    {
    }

    public override HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.Forbidden;
}