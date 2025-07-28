using System.Web;
using System.Web.Routing;

namespace VercelMonoFrameworkPrototype.Framework.Routing;

public class VercelFrameworkRouteHandler : IRouteHandler
{
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
        return new VercelFrameworkHandler();
    }
}