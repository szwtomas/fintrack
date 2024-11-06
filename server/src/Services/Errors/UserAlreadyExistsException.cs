namespace Fintrack.Services.Errors;

public class UserAlreadyExistsException(string message) : Exception(message);