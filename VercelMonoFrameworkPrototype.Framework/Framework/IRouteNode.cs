namespace VercelMonoFrameworkPrototype.Framework;

/// <summary>
/// The theory here is that it combines everything here and can use the start up to identify all of these nodes via reflection.
/// <br/>
/// The properties here thus far: Error, Layout and Page would hold the classes that make up that node.
/// <br/>
/// When dealing with error handling we can use this aspect of the application to create a friendly error.
/// </summary>
public interface IRouteNode
{
    public Type Api { get; set; }
    public Type Error { get; set; }
    public Type Layout { get; set; }
    public Type Page { get; set; }
}