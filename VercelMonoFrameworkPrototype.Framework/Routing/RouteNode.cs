namespace VercelMonoFrameworkPrototype.Framework.Routing;

/// <summary>
/// A data structure describing the nodes in a tree
/// </summary>
public class RouteNode
{
    public string Name { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string DirectoryPath { get; set; } = string.Empty;
    public List<RouteNode> Children { get; set; } = [];
    public RouteNodeMetadata Metadata { get; set; } = new() { };
}