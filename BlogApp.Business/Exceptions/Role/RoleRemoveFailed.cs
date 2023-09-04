using BlogApp.Business.Exceptions.Commons;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Exceptions.Role;

public class RoleRemoveFailed : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleRemoveFailed()
    {
        ErrorMessage = "When role removed something get wrong";
    }

    public RoleRemoveFailed(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

