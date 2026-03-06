namespace Application.Common.Exceptions;

public sealed class ApplicationException(string message): Exception(message);