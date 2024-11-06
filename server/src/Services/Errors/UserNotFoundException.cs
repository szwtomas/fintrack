namespace Fintrack.Services.Errors;

public class UserNotFoundException(string message) : Exception(message);