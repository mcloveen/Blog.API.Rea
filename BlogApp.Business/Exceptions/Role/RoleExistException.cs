using BlogApp.Business.Exceptions.Commons;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Exceptions.Role;

public class RoleExistException : Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleExistException()
    {
        ErrorMessage = "Role is already exist";
    }

    public RoleExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

