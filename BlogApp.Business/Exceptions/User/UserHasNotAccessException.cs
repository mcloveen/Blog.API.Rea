namespace BlogApp.Business.Exceptions.User;

public class UserHasNotAccessException : Exception
{
    public UserHasNotAccessException():base("User has not access for this command")
    {
    }

    public UserHasNotAccessException(string? message) : base(message)
    {
    }
}

