using System.Text.Json;
using System.Web;
using VercelMonoFrameworkPrototype.Framework.Services;

namespace VercelMonoFrameworkPrototype.Framework;

public class VercelMonoFrameworkApplication
{
    private readonly VercelFrameworkComponentService _componentService = new();
    private readonly VercelFrameworkPageService _pageService = new();
    private readonly VercelFrameworkRouter _router;
    private readonly VercelFrameworkConfigurator _configuration = new();

    public VercelFrameworkConfigurator Configuration { get => _configuration; }

    public VercelMonoFrameworkApplication(HttpApplication application)
    {
        application.Application["VercelFramework"] = this;
        _router = new(application);
    }
}