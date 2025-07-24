using System.Web;
using System.Configuration;
using System.Text.Json;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkHandler : IHttpHandler
{
    private readonly VercelFrameworkConfigurator _configuration = new();

    public bool IsReusable => false;

    public void ProcessRequest(HttpContext context)
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        List<string> segments = [.. context.Request.Url.Segments.Select(segment => {
            bool isRoot = segment == "/";

            if (isRoot)
            {
                return "~/src/routes";
            }

            return segment.Trim('/');
        })];

        string response = JsonSerializer.Serialize(new
        {
            segments,
            RouteTree = context.Application["Framework::Router::Tree"]
        }, jsonSerializerOptions);


        context.Response.ContentType = "application/json";
        context.Response.Write(response);
        context.Response.End();


        // string absoluteFilePath = context.Server.MapPath(
        //     $"{_configuration.RootPath}{context.Request.Path.Replace("Default.aspx", "")}"
        // );

        // VercelFrameworkTemplaterEngine engine = new(absoluteFilePath);

        // context.Response.Clear();
        // context.Response.ContentType = "text/html";
        // context.Response.Write(engine.Render());
    }
}