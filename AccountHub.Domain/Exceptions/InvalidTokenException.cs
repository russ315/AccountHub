using System.Net;

namespace AccountHub.Domain.Exceptions;

public class InvalidTokenException:BaseException
{
    public InvalidTokenException(string title,string message):base(title,message)
    {
        
    }

    public override HttpStatusCode HttpStatusCode { get;  } = HttpStatusCode.Unauthorized;
}