namespace ElementMcpServer.Models;

/// <summary>
/// Represents a foundation concept in the Availity Element Design System.
/// Foundations include design tokens, colors, typography, spacing, etc.
/// </summary>
public record Foundation
{
    /// <summary>
    /// Unique identifier for the foundation.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Display name of the foundation.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Type of foundation (e.g., Color, Typography, Spacing, Elevation).
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Detailed description of the foundation concept.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Usage guidelines and best practices.
    /// </summary>
    public string? Usage { get; init; }

    /// <summary>
    /// Design tokens associated with this foundation.
    /// </summary>
    public Dictionary<string, string>? Tokens { get; init; }

    /// <summary>
    /// Code examples demonstrating the foundation.
    /// </summary>
    public string? Example { get; init; }

    /// <summary>
    /// Related foundations.
    /// </summary>
    public List<string>? RelatedFoundations { get; init; }
}
