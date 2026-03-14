namespace Application.Common.Exceptions;

public sealed class AccessDeniedException(string message): Exception(message);