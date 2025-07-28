using System.CodeDom.Compiler;
using System.Text;
using System.Web;
using System.Web.UI;
using HtmlAgilityPack;
using Microsoft.CSharp;
using VercelMonoFrameworkPrototype.Framework.Enums;

namespace VercelMonoFrameworkPrototype.Framework;

public class VercelFrameworkTemplaterEngine
{
    private readonly DateTime _lastWritten;
    private readonly string _viewSourceTemplate = string.Empty;
    private readonly VercelFrameworkConfigurator _configuration = new();

    public void GenerateServerSideScript(string filePath)
    {
        CSharpCodeProvider provider = new();

        CompilerParameters parameters = new()
        {
            GenerateExecutable = false,
            GenerateInMemory = true,
        };

        var compilerResult = provider.CompileAssemblyFromFile(parameters, filePath);
    }

    public VercelFrameworkTemplaterEngine(string? routePath)
    {
        var templateExtension = "cshtml";
        switch (_configuration.Templater)
        {
            case VercelFrameworkTemplater.Fluid:
                templateExtension = "liquid";
                break;
            case VercelFrameworkTemplater.Handlebars:
                templateExtension = "hbs";
                break;
        }

        var isFileExisting = !string.IsNullOrEmpty(routePath) && File.Exists(routePath + $"+page.{templateExtension}");
        _lastWritten = File.GetLastWriteTime(routePath + $"+page.{templateExtension}");
        var isServerFileExisting = !string.IsNullOrEmpty(routePath) && File.Exists(routePath + "+server.cs");

        if (isFileExisting)
        {
            HtmlDocument doc = new();
            doc.Load(routePath + $"+layout.{templateExtension}");

            var node = doc.DocumentNode.SelectSingleNode("//slot");

            if (node != null)
            {
                _viewSourceTemplate = File.ReadAllText(routePath + $"+page.{templateExtension}");

                var pageContentNode = HtmlNode.CreateNode("<div></div>");
                pageContentNode.InnerHtml = _viewSourceTemplate;
                node.ParentNode.ReplaceChild(pageContentNode, node);
                _viewSourceTemplate = doc.DocumentNode.WriteTo();
            }

            if (File.Exists(HttpContext.Current.Server.MapPath("~/src/index.html")))
            {
                HtmlDocument document = new();
                document.Load(HttpContext.Current.Server.MapPath("~/src/index.html"));

                var layoutContentNode = HtmlNode.CreateNode("<div></div>");
                layoutContentNode.InnerHtml = _viewSourceTemplate;
                var slotNode = document.DocumentNode.SelectSingleNode("//slot");
                slotNode.ParentNode.ReplaceChild(layoutContentNode, slotNode);
                _viewSourceTemplate = document.DocumentNode.WriteTo();
            }
        }

        if (string.IsNullOrEmpty(_viewSourceTemplate))
        {
            throw new Exception($"The file was empty or does not exist?: {routePath}+page.{templateExtension}");
        }
    }

    public string NativeRenderer()
    {
        StringBuilder stringBuilder = new();

        using var stringWriter = new StringWriter(stringBuilder);
        var fakeRequest = new HttpRequest("+page.aspx", "http://tempuri.org", string.Empty);
        var fakeResponse = new HttpResponse(stringWriter);
        var fakeContext = new HttpContext(fakeRequest, fakeResponse);

        var pageInstance = PageParser.GetCompiledPageInstance("~/src/routes/+page.aspx", HttpContext.Current.Server.MapPath("~/src/routes/+page.aspx"), HttpContext.Current);
        pageInstance.ProcessRequest(fakeContext);

        return stringBuilder.ToString();
    }
    
    // public string RazorLightRenderer()
    // {
    //     throw new NotImplementedException();
    // }

    public string Render(HttpContext context)
    {
        var result = string.Empty;

        switch (_configuration.Templater)
        {
            case VercelFrameworkTemplater.Native:
                result = NativeRenderer();
                break;
            case VercelFrameworkTemplater.RazorEngine:
            case VercelFrameworkTemplater.Handlebars:
            case VercelFrameworkTemplater.Fluid:
            case VercelFrameworkTemplater.Custom:
            default:
                result = string.Empty;
                break;
        };

        return result;
    }
}