using System.Web;
using RazorEngine.Templating;

namespace VercelMonoFrameworkPrototypeLibrary.RazorEngine;

public abstract class GlobalTemplateBase<T> : TemplateBase<T>
{
    public HttpContext Context => HttpContext.Current;
    public HttpRequest Request => Context.Request;
    public HttpResponse Response => Context.Response;
    public VercelMonoFrameworkApplication VercelFramework => (VercelMonoFrameworkApplication)Context.Application["VercelFramework"];
}