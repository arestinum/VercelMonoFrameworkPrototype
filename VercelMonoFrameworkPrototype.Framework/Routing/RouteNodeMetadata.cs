namespace VercelMonoFrameworkPrototype.Framework;

public class RouteNodeMetadata
{
    public bool hasDynamicChild { get; set; }
    public bool hasErrorChild { get; set; }
    public bool hasDefault { get; set; }
    public bool hasEndpoint { get; set; }
}