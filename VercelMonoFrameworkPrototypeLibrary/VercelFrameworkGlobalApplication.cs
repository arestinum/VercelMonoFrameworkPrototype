using System.Web;
using System.Web.Routing;
using VercelMonoFrameworkPrototypeLibrary.Routing;
using System.Text.Json;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkGlobalApplication : HttpApplication
{
    private VercelMonoFrameworkApplication? _frameworkApplication;

    protected void Application_Start(object sender, EventArgs e)
    {
        _frameworkApplication = new((HttpApplication)sender);
        RouteTable.Routes.Add(new Route(string.Empty, new VercelFrameworkRouteHandler()));
        RouteTable.Routes.Add(new Route("{*route}", new VercelFrameworkRouteHandler()));

        // RouteTable.Routes.Add(new Route(string.Empty, new VercelFrameworkRouteHandler()));
    }
}