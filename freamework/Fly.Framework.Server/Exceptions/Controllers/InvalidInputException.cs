namespace Fly.Framework.Server.Exceptions.Controllers;

public class InvalidInputException : Exception
{
    public InvalidInputException(string msg) : base(msg)
    {

    }
}