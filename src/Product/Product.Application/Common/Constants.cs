namespace Product.Application.Common;

public static class Constants
{
    public static class Product
    {
        public const string ProductAlreadyExists = "The name of the product is already registered.";
    }
    
    public static class ErrorMessages
    {
        public const string ValidationError = "One or more validation failures have occurred";
        public const string UnhandledError = "An error occurred while processing your request";
        public const string BadRequestError = "We cannot proccess your request because is invalid";
        public const string NotFoundError = "We could not found the record";
    }
}