using System.Net;

namespace AccountHub.Domain.Exceptions;

public abstract class BaseException:Exception
{
    public abstract HttpStatusCode HttpStatusCode { get; set; }
    public string Title { get; set; }
    public BaseException(string title,string message):base(message)
    {
        Title = title;
    }
}