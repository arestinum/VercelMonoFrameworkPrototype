using System.Web;
using System.Configuration;
using System.Text.Json;

namespace VercelMonoFrameworkPrototype.Framework;

public class VercelFrameworkHandler : IHttpHandler
{

    public bool IsReusable => false;

    public void ProcessRequest(HttpContext context)
    {
        VercelMonoFrameworkApplication frameworkApplication = (VercelMonoFrameworkApplication)context.Application["VercelFramework"];

        List<string> segments = [.. context.Request.Url.Segments.Select(s => s.Trim('/'))];

        object data = new
        {
            segments,
            RouteTree = context.Application["Framework::Router:Tree"]
        };

        string response = JsonSerializer.Serialize(data, frameworkApplication.Configuration.JsonSerializerOptions);

        context.Response.ContentType = "application/json";
        context.Response.Write(response);
        context.Response.End();
    }
}