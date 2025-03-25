using System.Net;

namespace AccountHub.Domain.Exceptions;

public class ServiceException:BaseException
{
    public ServiceException(string title, string message) : base(title, message)
    {
    }

    public override HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.InternalServerError;
}