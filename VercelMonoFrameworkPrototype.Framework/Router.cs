using System.Reflection;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using System.Web.UI.WebControls;
using VercelMonoFrameworkPrototype.Framework.Routing;

namespace VercelMonoFrameworkPrototype.Framework;

public enum RouteNodeType
{
    Static,
    Dynamic,
    CatchAll,
    Optional
}

public class VercelFrameworkRouteNode
{
    public string DirectoryPath { get; set; } = string.Empty;
    public List<VercelFrameworkRouteNode> Children { get; set; } = [];
    public VercelFrameworkRouteNode? ParentRouteNode { get; set; }
    public RouteNodeType Type { get; set; } = RouteNodeType.Static;

    public VercelFrameworkRouteNode()
    { }
}

public class VercelFrameworkRouter
{
    public HttpApplication Application { get; set; }
    public RouteNode RootNode { get; set; }
    public Assembly ApplicationAssembly { get; set; }

    public VercelFrameworkRouter(HttpApplication application)
    {
        Application = application;
        var routerRoot = Application.Server.MapPath("~/Application/Routes");

        Application.Application["Framework::Router"] = this;
        Application.Application["Framework::Router::Root"] = routerRoot;
        Application.Application["Framework::Assembly"] = Assembly.GetAssembly(typeof(IVercelFrameworkPage));

        RootNode = new()
        {
            Name = "",
            DirectoryPath = routerRoot,
            Children = Discover(routerRoot),
            Metadata = new()
            {
                hasDefault = File.Exists(
                    Path.Combine(routerRoot, "+page.cshtml")
                ),
                hasEndpoint = File.Exists(
                    Path.Combine(routerRoot, "+server.cshtml")
                ),
                hasErrorChild = File.Exists(
                    Path.Combine(routerRoot, "+error.cshtml")
                ),
                hasDynamicChild = Directory.GetDirectories(routerRoot).Any(directory => directory.StartsWith("[") && directory.EndsWith("]"))
            }
        };

        Application.Application["Framework::Router::Tree"] = RootNode;
        
        RegisterRoutes(RouteTable.Routes);
    }

    private static void RegisterRoutes(RouteCollection routes)
    {
        VercelFrameworkRouteHandler frameworkRouteHandler = new();
        Route frameworkRootRoute = new(string.Empty, frameworkRouteHandler);
        Route frameworkGenericRoute = new("{*route}", frameworkRouteHandler);
        
        routes.Add(string.Empty, frameworkRootRoute);
        routes.Add("Generic", frameworkGenericRoute);
    }

    public List<RouteNode> Discover(string path)
    {
        List<string> directories = [.. Directory.GetDirectories(path)];
        if (directories.Count == 0) return [];

        List<RouteNode> children = [..directories.Select(directory => {
            string pattern = @$"^{directory}\/?$";
            Regex.Replace(pattern, @"\[.{0,}    \]", ".{0,}");

            return new RouteNode() {
                Name = directory.Split('/').Last(),
                Pattern = $"^{directory}\\/?$",
                DirectoryPath = directory,
                Children = Discover(directory),
                Metadata = new() {
                    hasDefault = File.Exists(
                        Path.Combine(directory, "+page.cshtml")
                    ),
                    hasEndpoint = File.Exists(
                        Path.Combine(directory, "+server.cs")
                    ),
                    hasErrorChild = File.Exists(
                        Path.Combine(directory, "+error.cs")
                    ),
                    hasDynamicChild = Directory.GetDirectories(directory).Any(directory => directory.StartsWith("[") && directory.EndsWith("]"))
                }
            };
        })];

        return children;
    }
}