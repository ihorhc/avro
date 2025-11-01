namespace AVRO.TaskRegistry.Models;

/// <summary>
/// Represents a task in the registry
/// </summary>
public class Task
{
    /// <summary>
    /// Unique task identifier (e.g., TASK-0001)
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Task title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Task category
    /// </summary>
    public TaskCategory Category { get; set; }

    /// <summary>
    /// Priority level
    /// </summary>
    public TaskPriority Priority { get; set; }

    /// <summary>
    /// Current status
    /// </summary>
    public TaskStatus Status { get; set; }

    /// <summary>
    /// Assigned project ID
    /// </summary>
    public string? AssignedProject { get; set; }

    /// <summary>
    /// Task dependencies (other task IDs)
    /// </summary>
    public List<string> Dependencies { get; set; } = new();

    /// <summary>
    /// AI execution metadata
    /// </summary>
    public AIMetadata? AIMetadata { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Completion timestamp
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}

/// <summary>
/// Task category enumeration
/// </summary>
public enum TaskCategory
{
    Backend,
    Frontend,
    Infrastructure,
    AI,
    Documentation,
    Testing
}

/// <summary>
/// Task priority enumeration
/// </summary>
public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}

/// <summary>
/// Task status enumeration
/// </summary>
public enum TaskStatus
{
    Pending,
    InProgress,
    Blocked,
    Completed,
    Cancelled
}

/// <summary>
/// AI execution metadata
/// </summary>
public class AIMetadata
{
    /// <summary>
    /// Task complexity level
    /// </summary>
    public string Complexity { get; set; } = "medium";

    /// <summary>
    /// Estimated effort
    /// </summary>
    public string EstimatedEffort { get; set; } = "4h";

    /// <summary>
    /// Requires human review
    /// </summary>
    public bool RequiresHumanReview { get; set; }

    /// <summary>
    /// Context files for AI agents
    /// </summary>
    public List<string> ContextFiles { get; set; } = new();
}
