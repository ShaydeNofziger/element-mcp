namespace ElementMcpServer.Models;

/// <summary>
/// Represents a template in the Availity Element Design System.
/// Templates are pre-built layouts combining patterns and components.
/// </summary>
public record Template
{
    /// <summary>
    /// Unique identifier for the template.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display name of the template.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Type of template (e.g., Page Layout, Dashboard, Form).
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Detailed description of the template and its use case.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// When to use this template.
    /// </summary>
    public string? UseCase { get; init; }

    /// <summary>
    /// Customization guidelines for the template.
    /// </summary>
    public string? Customization { get; init; }

    /// <summary>
    /// Code example for the template.
    /// </summary>
    public string? Example { get; init; }

    /// <summary>
    /// Components used in this template.
    /// </summary>
    public List<string>? ComponentsUsed { get; init; }

    /// <summary>
    /// Patterns used in this template.
    /// </summary>
    public List<string>? PatternsUsed { get; init; }

    /// <summary>
    /// Related templates.
    /// </summary>
    public List<string>? RelatedTemplates { get; init; }
}
