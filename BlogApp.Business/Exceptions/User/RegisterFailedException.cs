using System;
namespace BlogApp.Business.Exceptions.User
{
	public class RegisterFailedException:Exception
	{
		public RegisterFailedException() : base("Register failed for some reasons") { }

		public RegisterFailedException(string? message) : base(message) { }
    }
}

