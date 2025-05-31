using System.CodeDom.Compiler;
using System.Numerics;
using Microsoft.CSharp;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using VercelMonoFrameworkPrototypeLibrary.Enums;
using VercelMonoFrameworkPrototypeLibrary.RazorEngine;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkTemplaterEngine
{
    private readonly DateTime _lastAccessed;
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
        _lastAccessed = File.GetLastWriteTime(routePath + $"+page.{templateExtension}");
        bool isServerFileExisting = !string.IsNullOrEmpty(routePath) && File.Exists(routePath + "+server.cs");

        if (isFileExisting)
            _viewSourceTemplate = File.ReadAllText(routePath + $"+page.{templateExtension}");

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
                BaseTemplateType = typeof(GlobalTemplateBase<>)
            };

            var service = RazorEngineService.Create(config);

            result = service.RunCompile(_viewSourceTemplate, _lastAccessed.ToString(), null, new { FirstName = "Patrikas", LastName = "Lyddon" });
        }

        return result;
    }
}