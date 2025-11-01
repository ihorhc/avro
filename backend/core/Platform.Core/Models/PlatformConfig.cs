namespace AVRO.Platform.Core.Models;

/// <summary>
/// Platform configuration settings
/// </summary>
public class PlatformConfig
{
    /// <summary>
    /// Platform name
    /// </summary>
    public string Name { get; set; } = "AVRO";

    /// <summary>
    /// Platform version
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Environment (Development, Staging, Production)
    /// </summary>
    public string Environment { get; set; } = "Development";

    /// <summary>
    /// Maximum number of concurrent projects
    /// </summary>
    public int MaxConcurrentProjects { get; set; } = 100;

    /// <summary>
    /// Maximum number of tasks in registry
    /// </summary>
    public int MaxTasks { get; set; } = 10000;

    /// <summary>
    /// AI agent configuration
    /// </summary>
    public AIConfig AI { get; set; } = new();
}

/// <summary>
/// AI-specific configuration
/// </summary>
public class AIConfig
{
    /// <summary>
    /// Enable autonomous workflows
    /// </summary>
    public bool EnableAutonomousWorkflows { get; set; } = true;

    /// <summary>
    /// Require human review for supervised changes
    /// </summary>
    public bool RequireHumanReview { get; set; } = true;

    /// <summary>
    /// Default autonomy level for new projects
    /// </summary>
    public string DefaultAutonomyLevel { get; set; } = "supervised";

    /// <summary>
    /// Context optimization enabled
    /// </summary>
    public bool ContextOptimizationEnabled { get; set; } = true;
}
