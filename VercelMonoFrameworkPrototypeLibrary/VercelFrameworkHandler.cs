using System.Web;
using System.Configuration;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkHandler : IHttpHandler
{
    private readonly VercelFrameworkConfigurator _configuration = new();

    public bool IsReusable => false;

    public void ProcessRequest(HttpContext context)
    {
        string? absoluteFilePath = context.Server.MapPath(
            $"{_configuration.RootPath}{context.Request.Path.Replace("Default.aspx", "")}"
        );

        VercelFrameworkTemplaterEngine engine = new(absoluteFilePath);

        context.Response.Clear();
        context.Response.ContentType = "text/html";
        context.Response.Write(engine.Render());
    }
}