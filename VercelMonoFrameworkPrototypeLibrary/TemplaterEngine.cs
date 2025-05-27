using System.Numerics;
using RazorEngine;
using RazorEngine.Templating;
using VercelMonoFrameworkPrototypeLibrary.Enums;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkTemplaterEngine
{
    private readonly DateTime _lastAccessed;
    private readonly string _viewSourceTemplate = string.Empty;
    private readonly VercelFrameworkConfigurator _configuration = new();

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
            result = Engine.Razor.RunCompile(_viewSourceTemplate, _lastAccessed.ToString(), null, new { FirstName = "Patrikas", LastName = "Lyddon" });

        return result;
    }
}