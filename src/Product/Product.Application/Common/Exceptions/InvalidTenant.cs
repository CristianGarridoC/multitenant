namespace Product.Application.Common.Exceptions;

public class InvalidTenant : Exception
{
    public InvalidTenant(string message) : base(message)
    {
        
    }
}