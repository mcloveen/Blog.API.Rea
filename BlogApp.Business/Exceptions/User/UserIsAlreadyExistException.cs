namespace BlogApp.Business.Exceptions.User;

public class UserIsAlreadyExistException:Exception
{
    public UserIsAlreadyExistException() : base("User is already exist") { }

    public UserIsAlreadyExistException(string? message) : base(message) { }
}

