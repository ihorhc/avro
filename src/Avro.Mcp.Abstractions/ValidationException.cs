namespace Avro.Mcp.Abstractions;

/// <summary>
/// Exception thrown when server configuration validation fails
/// </summary>
[Serializable]
public class ValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ValidationException class
    /// </summary>
    public ValidationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with inner exception
    /// </summary>
    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
}
