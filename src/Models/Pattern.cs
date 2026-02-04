namespace ElementMcpServer.Models;

/// <summary>
/// Represents a design pattern in the Availity Element Design System.
/// Patterns are reusable solutions to common design problems.
/// </summary>
public record Pattern
{
    /// <summary>
    /// Unique identifier for the pattern.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display name of the pattern.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Category the pattern belongs to.
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    /// Detailed description of the pattern and when to use it.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Problem this pattern solves.
    /// </summary>
    public string? Problem { get; init; }

    /// <summary>
    /// Solution provided by this pattern.
    /// </summary>
    public string? Solution { get; init; }

    /// <summary>
    /// Best practices and usage guidelines.
    /// </summary>
    public string? BestPractices { get; init; }

    /// <summary>
    /// Code example demonstrating the pattern.
    /// </summary>
    public string? Example { get; init; }

    /// <summary>
    /// Components used in this pattern.
    /// </summary>
    public List<string>? ComponentsUsed { get; init; }

    /// <summary>
    /// Related patterns.
    /// </summary>
    public List<string>? RelatedPatterns { get; init; }
}
