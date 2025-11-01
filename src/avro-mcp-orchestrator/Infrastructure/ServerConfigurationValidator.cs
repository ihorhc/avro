namespace Avro.Mcp.Orchestrator.Infrastructure;

using Avro.Mcp.Orchestrator.Services;

/// <summary>
/// Validates server configurations
/// </summary>
public class ServerConfigurationValidator : IServerConfigurationValidator
{
    public void Validate(ServerConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Name))
            throw new ArgumentException("Server name is required");
        
        if (string.IsNullOrWhiteSpace(config.Command))
            throw new ArgumentException("Server command is required");
        
        if (config.TimeoutSeconds < 1 || config.TimeoutSeconds > 300)
            throw new ArgumentException("Timeout must be between 1 and 300 seconds");
    }
}
