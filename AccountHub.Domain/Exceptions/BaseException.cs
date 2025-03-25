using System.Net;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Domain.Exceptions;

public abstract class BaseException:Exception
{
    public abstract HttpStatusCode HttpStatusCode { get;  }
    public string Title { get; }
    public IEnumerable<IdentityError>? Details { get; init; }
    
    public BaseException(string title,string message):base(message)
    {
        Title = title;
        
    }
}