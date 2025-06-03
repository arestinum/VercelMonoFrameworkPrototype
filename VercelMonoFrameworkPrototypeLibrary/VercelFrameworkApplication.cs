using System.Web;
using VercelMonoFrameworkPrototypeLibrary.Services;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelMonoFrameworkApplication
{
    private readonly VercelFrameworkComponentService _componentService = new();
    private readonly VercelFrameworkPageService _pageService = new();
    private readonly VercelFrameworkRouter _router;

    public VercelMonoFrameworkApplication(HttpApplication application)
    {
        application.Application["VercelFramework"] = this;
        _router = new(application);
    }

    public string GreetUser(string name)
    {
        return $"Hello {name}!";
    }
}