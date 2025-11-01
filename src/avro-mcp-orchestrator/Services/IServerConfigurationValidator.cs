namespace Avro.Mcp.Orchestrator.Services;

/// <summary>
/// Validates server configurations
/// </summary>
public interface IServerConfigurationValidator
{
    void Validate(ServerConfig config);
}
