using System.Security;
using System.Web;
using System.Web.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace VercelMonoFrameworkPrototypeLibrary;

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
    {
    }
}

public class VercelFrameworkRouter
{
    public VercelFrameworkRouteNode RootNode { get; set; }

    public VercelFrameworkRouter()
    {
        RootNode = new();
        Discover(HttpContext.Current.Server.MapPath("~/src/routes"));
        HttpContext.Current.Application["VercelFrameworkRouter"] = this;
        RootNode.Children.Add(new());
    }

    public void Discover(string filePath)
    {
        var files = Directory.GetFiles(filePath);

        if (File.Exists(filePath + "+page.cshtml") || File.Exists(filePath + "server.cs"))
        {
            RootNode.Children.Add(new());

            foreach (var file in files)
            {
                Discover(file);
            }
        }

        // Matcher matcher = new();
        // matcher.AddInclude("+page.cshtml");
        // matcher.AddInclude("+server.cs");

        // var results = matcher.Execute(
        // new DirectoryInfoWrapper(
        //         new DirectoryInfo(
        //             HttpContext.Current.Server.MapPath("~/src/routes")
        //         )
        //     )
        // );

        // if (results == null) return;

        // var apiRouteFiles = results.Files.Where(file => file.Path.EndsWith("+server.cs"));
        // var pageRouteFiles = results.Files.Where(file => file.Path.EndsWith("+page.cshtml"));
    }
}