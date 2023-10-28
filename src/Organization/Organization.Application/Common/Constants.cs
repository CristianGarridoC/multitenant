namespace Organization.Application.Common;

public static class Constants
{
    public static class User
    {
        public const string InvalidCredentials = "Invalid email or password, please try again";
        public const string InvalidEmailDueToAlreadyExisting = "Email is already in use";
        public const string UserNotFound = "We could not found your user";
    }
    
    public static class ErrorMessages
    {
        public const string ValidationError = "One or more validation failures have occurred";
        public const string UnhandledError = "An error occurred while processing your request";
        public const string BadRequestError = "We cannot proccess your request because is invalid";
        public const string NotFoundError = "We could not found the record";
    }
    
    public static class Organization
    {
        public const string OrganizationAlreadyExists = "The name of the organization is already registered.";
    }
}