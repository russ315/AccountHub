using System.Net;

namespace AccountHub.Domain.Exceptions;

public abstract class BaseException:Exception
{
    public abstract HttpStatusCode HttpStatusCode { get;  }
    public string Title { get; }
    public object? Details { get; init; }
    
    public BaseException(string title,string message):base(message)
    {
        Title = title;
        
    }
}