using System.Web;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelMonoFrameworkApplication
{
    public VercelMonoFrameworkApplication(HttpApplication application)
    {
        application.Application["VercelFramework"] = this;
    }

    public string GreetUser(string name)
    {
        return $"Hello {name}!";
    }
}