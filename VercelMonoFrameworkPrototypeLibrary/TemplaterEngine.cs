using System.CodeDom.Compiler;
using System.Numerics;
using System.Web;
using HtmlAgilityPack;
using Microsoft.CSharp;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using VercelMonoFrameworkPrototypeLibrary.Enums;
using VercelMonoFrameworkPrototypeLibrary.RazorEngine;

namespace VercelMonoFrameworkPrototypeLibrary;

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
        string templateExtension = "cshtml";
        switch (_configuration.Templater)
        {
            case VercelFrameworkTemplater.Fluid:
                templateExtension = "liquid";
                break;
            case VercelFrameworkTemplater.Handlebars:
                templateExtension = "hbs";
                break;
        }

        bool isFileExisting = !string.IsNullOrEmpty(routePath) && File.Exists(routePath + $"+page.{templateExtension}");
        _lastWritten = File.GetLastWriteTime(routePath + $"+page.{templateExtension}");
        bool isServerFileExisting = !string.IsNullOrEmpty(routePath) && File.Exists(routePath + "+server.cs");

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

    public string Render()
    {
        string result = string.Empty;

        if (_configuration.Templater is VercelFrameworkTemplater.RazorEngine)
        {
            TemplateServiceConfiguration config = new()
            {
                BaseTemplateType = typeof(GlobalTemplateBase<>),
                CachingProvider = new VercelFrameworkCachingProvider(),
                Debug = true
            };

            var service = RazorEngineService.Create(config);

            result = service.RunCompile(_viewSourceTemplate, _lastWritten.ToString(), null, new { FirstName = "Patrikas", LastName = "Lyddon" });
        }

        return result;
    }
}