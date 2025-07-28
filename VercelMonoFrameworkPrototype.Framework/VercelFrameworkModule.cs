using System.Web;

namespace VercelMonoFrameworkPrototype.Framework;

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