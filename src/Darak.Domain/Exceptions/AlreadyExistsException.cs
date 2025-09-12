namespace Darak.Domain.Exceptions;

public class AlreadyExistsException(string resourceType)
    : Exception(resourceType + " Already Exists")
{
}
