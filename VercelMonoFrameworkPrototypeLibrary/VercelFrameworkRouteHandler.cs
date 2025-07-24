using System.Web;
using System.Web.Routing;

namespace VercelMonoFrameworkPrototypeLibrary.Routing;

public class VercelFrameworkRouteHandler : IRouteHandler
{
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
        return new VercelFrameworkHandler();
    }
}