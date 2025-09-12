namespace Darak.Domain.Exceptions;
public class UnAuthorizedAccessException(string message)
    : Exception(message)
{
}

