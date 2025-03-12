using System.Net;

namespace AccountHub.Domain.Exceptions;

public class EntityNotFoundException:BaseException
{
    public override HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.NotFound;
    public EntityNotFoundException(string title,string message):base(title,message)
    {
        
    }
}