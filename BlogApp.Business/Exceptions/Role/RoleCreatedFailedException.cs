using BlogApp.Business.Exceptions.Commons;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Exceptions.Role;

public class RoleCreatedFailedException : Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleCreatedFailedException()
    {
        ErrorMessage = "Something went wrong";
    }

    public RoleCreatedFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

