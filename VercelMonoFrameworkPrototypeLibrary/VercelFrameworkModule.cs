using System.Web;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.BeginRequest += (sender, e) =>
        {

        };
    }

    public void Dispose() { }
}