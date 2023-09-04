using BlogApp.Business.Exceptions.Commons;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Exceptions.Role;

public class RoleUpdateFailed : Exception, IBaseException
{

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleUpdateFailed()
    {
        ErrorMessage = "Role update failed";
    }

    public RoleUpdateFailed(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

