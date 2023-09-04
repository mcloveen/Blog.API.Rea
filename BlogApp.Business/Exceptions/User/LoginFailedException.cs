using System;
namespace BlogApp.Business.Exceptions.User
{
	public class LoginFailedException:Exception
	{
		public LoginFailedException() : base("Login failed for some reasons") { }
		public LoginFailedException(string? message) : base(message) { }
    }
}

