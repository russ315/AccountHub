using System.Net;

namespace AccountHub.Domain.Exceptions;

public class BadRequestException:BaseException
{
    public BadRequestException(string title, string message) : base(title, message)
    {
    }

    public override HttpStatusCode HttpStatusCode { get;  } = HttpStatusCode.BadRequest;
}