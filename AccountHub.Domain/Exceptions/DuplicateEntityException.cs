﻿using System.Net;

namespace AccountHub.Domain.Exceptions;

public class DuplicateEntityException:BaseException
{
    public DuplicateEntityException(string title,string message):base(title,message)
    {
        
    }

    public override HttpStatusCode HttpStatusCode { get;  } = HttpStatusCode.Conflict;
}