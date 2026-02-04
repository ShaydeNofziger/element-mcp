namespace ElementMcpServer.Models;

/// <summary>
/// Represents a component in the Availity Element Design System.
/// </summary>
public record Component
{
    /// <summary>
    /// Unique identifier for the component.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display name of the component.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Category the component belongs to.
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    /// Detailed description of the component's purpose and usage.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Usage instructions and best practices.
    /// </summary>
    public string? Usage { get; init; }

    /// <summary>
    /// Code example demonstrating the component.
    /// </summary>
    public string? Example { get; init; }

    /// <summary>
    /// Available props/properties for the component.
    /// </summary>
    public List<ComponentProp>? Props { get; init; }

    /// <summary>
    /// Related components.
    /// </summary>
    public List<string>? RelatedComponents { get; init; }

    /// <summary>
    /// Accessibility guidelines for the component.
    /// </summary>
    public string? Accessibility { get; init; }

    /// <summary>
    /// URL to Storybook documentation.
    /// </summary>
    public string? StorybookUrl { get; init; }
}

/// <summary>
/// Represents a property/prop of a component.
/// </summary>
public record ComponentProp
{
    /// <summary>
    /// Name of the prop.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Type of the prop (e.g., string, number, boolean).
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Whether the prop is required.
    /// </summary>
    public bool Required { get; init; }

    /// <summary>
    /// Default value if not provided.
    /// </summary>
    public string? DefaultValue { get; init; }

    /// <summary>
    /// Description of what the prop does.
    /// </summary>
    public string? Description { get; init; }
}
