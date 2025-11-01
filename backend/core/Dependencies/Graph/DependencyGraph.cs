using Microsoft.Extensions.Logging;

namespace AVRO.Dependencies.Graph;

/// <summary>
/// Dependency graph for tracking cross-project dependencies
/// </summary>
public class DependencyGraph
{
    private readonly ILogger<DependencyGraph> _logger;
    private readonly Dictionary<string, List<string>> _dependencies;
    private readonly Dictionary<string, List<string>> _dependents;

    public DependencyGraph(ILogger<DependencyGraph> logger)
    {
        _logger = logger;
        _dependencies = new Dictionary<string, List<string>>();
        _dependents = new Dictionary<string, List<string>>();
    }

    /// <summary>
    /// Add a dependency relationship
    /// </summary>
    public void AddDependency(string projectId, string dependsOn)
    {
        _logger.LogInformation("Adding dependency: {ProjectId} depends on {DependsOn}", projectId, dependsOn);

        if (!_dependencies.ContainsKey(projectId))
        {
            _dependencies[projectId] = new List<string>();
        }
        _dependencies[projectId].Add(dependsOn);

        if (!_dependents.ContainsKey(dependsOn))
        {
            _dependents[dependsOn] = new List<string>();
        }
        _dependents[dependsOn].Add(projectId);
    }

    /// <summary>
    /// Get all dependencies for a project
    /// </summary>
    public IEnumerable<string> GetDependencies(string projectId)
    {
        return _dependencies.ContainsKey(projectId) 
            ? _dependencies[projectId] 
            : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Get all dependents of a project
    /// </summary>
    public IEnumerable<string> GetDependents(string projectId)
    {
        return _dependents.ContainsKey(projectId) 
            ? _dependents[projectId] 
            : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Check for circular dependencies
    /// </summary>
    public bool HasCircularDependency(string projectId)
    {
        var visited = new HashSet<string>();
        return HasCircularDependencyHelper(projectId, visited);
    }

    private bool HasCircularDependencyHelper(string projectId, HashSet<string> visited)
    {
        if (visited.Contains(projectId))
        {
            return true;
        }

        visited.Add(projectId);

        foreach (var dependency in GetDependencies(projectId))
        {
            if (HasCircularDependencyHelper(dependency, new HashSet<string>(visited)))
            {
                return true;
            }
        }

        return false;
    }
}
