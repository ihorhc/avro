using Microsoft.Extensions.Logging;
using AVRO.TaskRegistry.Models;

namespace AVRO.TaskRegistry.Services;

/// <summary>
/// Task registry service interface
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Get task by ID
    /// </summary>
    Task<Models.Task?> GetTaskAsync(string taskId);

    /// <summary>
    /// Get all tasks
    /// </summary>
    Task<IEnumerable<Models.Task>> GetAllTasksAsync();

    /// <summary>
    /// Get tasks by status
    /// </summary>
    Task<IEnumerable<Models.Task>> GetTasksByStatusAsync(Models.TaskStatus status);

    /// <summary>
    /// Get tasks by category
    /// </summary>
    Task<IEnumerable<Models.Task>> GetTasksByCategoryAsync(Models.TaskCategory category);
}

/// <summary>
/// Task registry service implementation
/// </summary>
public class TaskService : ITaskService
{
    private readonly ILogger<TaskService> _logger;
    private readonly List<Models.Task> _tasks;

    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;
        _tasks = new List<Models.Task>();
    }

    public async Task<Models.Task?> GetTaskAsync(string taskId)
    {
        _logger.LogInformation("Retrieving task {TaskId}", taskId);
        await System.Threading.Tasks.Task.CompletedTask;
        return _tasks.FirstOrDefault(t => t.Id == taskId);
    }

    public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
    {
        _logger.LogInformation("Retrieving all tasks");
        await System.Threading.Tasks.Task.CompletedTask;
        return _tasks;
    }

    public async Task<IEnumerable<Models.Task>> GetTasksByStatusAsync(Models.TaskStatus status)
    {
        _logger.LogInformation("Retrieving tasks with status {Status}", status);
        await System.Threading.Tasks.Task.CompletedTask;
        return _tasks.Where(t => t.Status == status);
    }

    public async Task<IEnumerable<Models.Task>> GetTasksByCategoryAsync(Models.TaskCategory category)
    {
        _logger.LogInformation("Retrieving tasks in category {Category}", category);
        await System.Threading.Tasks.Task.CompletedTask;
        return _tasks.Where(t => t.Category == category);
    }
}
