namespace Avro.Mcp.Abstractions;

/// <summary>
/// Interface for validating server configurations
/// </summary>
public interface IServerConfigurationValidator
{
    /// <summary>
    /// Validates a server configuration
    /// </summary>
    /// <exception cref="ValidationException">Thrown when validation fails</exception>
    void Validate(ServerConfig config);
}
