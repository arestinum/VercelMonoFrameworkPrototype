using System.Web;
using System.Web.Routing;
using VercelMonoFrameworkPrototype.Framework.Routing;
using System.Text.Json;

namespace VercelMonoFrameworkPrototype.Framework;

public class VercelFrameworkGlobalApplication : HttpApplication
{
    private VercelMonoFrameworkApplication? _frameworkApplication;

    protected void Application_Start(object sender, EventArgs e)
    {
        _frameworkApplication = new((HttpApplication)sender);
    }
}