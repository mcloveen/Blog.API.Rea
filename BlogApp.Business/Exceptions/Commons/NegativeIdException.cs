using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Exceptions.Commons;

public class NegativeIdException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public NegativeIdException() : base()
    {
        ErrorMessage = "Id 0-dan kicik ve ya beraber ola bilmez";
    }

    public NegativeIdException(string? message):base(message)
    {
        ErrorMessage = message;
    }

}

