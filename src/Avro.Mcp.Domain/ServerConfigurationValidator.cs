namespace Avro.Mcp.Domain;

/// <summary>
/// Validates server configurations against business rules
/// </summary>
public class ServerConfigurationValidator : IServerConfigurationValidator
{
    /// <summary>
    /// Validates a server configuration
    /// </summary>
    /// <exception cref="ValidationException">Thrown when validation fails</exception>
    public void Validate(ServerConfig config)
    {
        if (config is null)
            throw new ValidationException("Server configuration cannot be null");

        if (string.IsNullOrWhiteSpace(config.Name))
            throw new ValidationException("Server name is required");

        if (string.IsNullOrWhiteSpace(config.Command))
            throw new ValidationException("Server command is required");

        if (config.TimeoutSeconds <= 0)
            throw new ValidationException("Timeout must be greater than zero");

        if (config.Name.Length > 100)
            throw new ValidationException("Server name must not exceed 100 characters");
    }
}
