using System;
namespace BlogApp.Business.Exceptions.User
{
	public class UserNotFoundException:Exception
	{
		public UserNotFoundException() : base("User not found") { }

		public UserNotFoundException(string? message) : base(message) { }
    }
}

