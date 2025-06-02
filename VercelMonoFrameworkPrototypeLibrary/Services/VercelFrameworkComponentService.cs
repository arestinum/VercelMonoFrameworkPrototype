using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Web;
using HtmlAgilityPack;
using Microsoft.CSharp;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using VercelMonoFrameworkPrototypeLibrary.RazorEngine;

namespace VercelMonoFrameworkPrototypeLibrary.Services;

public class ComponentScriptServerSideFile
{
    public DateTime LastWritten { get; set; }
    public Assembly? Assembly { get; set; }
}

public class ComponentScriptClientSideFile
{
    public DateTime LastWritten { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class ComponentContentFile
{
    public DateTime LastWritten { get; set; }
    public HtmlDocument? Content { get; set; }
}

public class ComponentStyleFile
{
    public DateTime LastWritten { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class VercelMonoFrameworkComponent
{
    public string FolderPath { get; set; } = string.Empty;
    public ComponentScriptServerSideFile? ServerScript { get; set; }
    public ComponentScriptClientSideFile? ClientScript { get; set; }
    public ComponentContentFile? Content { get; set; }

    public VercelMonoFrameworkComponent(string folderPath)
    {
        FolderPath = folderPath;
        if (Content != null && Content.Content != null)
            Content.Content.Load(folderPath + "+page.cshtml");
    }
}

public class ComponentRegistry
{
    public DateTime CreatedAt { get; set; }
    public DateTime LastWritten { get; set; }
    public Assembly Assembly { get; set; }

    public ComponentScriptClientSideFile ClientSide { get; set; }
    public ComponentContentFile Content { get; set; }
    public ComponentStyleFile Style { get; set; }

    public ComponentRegistry(string filePath)
    {
        CSharpCodeProvider provider = new();
        CompilerParameters parameters = new()
        {
            GenerateExecutable = false,
            GenerateInMemory = true,
        };

        var compilerResults = provider.CompileAssemblyFromFile(parameters, filePath);
        Assembly = compilerResults.CompiledAssembly;
    }
}

public class VercelFrameworkComponentService
{
    public Dictionary<string, ComponentRegistry> ComponentRegistry { get; set; }

    public VercelFrameworkComponentService()
    {
        Discover();
    }

    private void Discover()
    {
        Matcher matcher = new();

        matcher.AddInclude("*.component.cs");

        var results = matcher.Execute(
            new DirectoryInfoWrapper(
                new DirectoryInfo(
                    HttpContext.Current.Server.MapPath(
                        "~/"
                    )
                )
            )
        );

        if (results.HasMatches)
        {
            CSharpCodeProvider provider = new() { };
            CompilerParameters parameters = new()
            {
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            foreach (var file in results.Files)
            {
                var result = provider.CompileAssemblyFromFile(parameters, file.Path);
                var assembly = result.CompiledAssembly;

                var componentClasses = assembly.ExportedTypes.Where(p => p.Name.EndsWith("Component"));

                if (componentClasses.Count() > 1)
                    throw new Exception(
                        $"There are more than one component classes in {file.Path}",
                        new($"Please use one class per file, identified classes: {string.Join(",", componentClasses.Select(t => t.Name))}")
                    );

                var componentClass = componentClasses.First();
                var componentInitMethod = componentClass.GetMethod("Init");

                object componentInstance = assembly.CreateInstance(componentClass.Name);
                var initReturn = componentClass.InvokeMember("Init", BindingFlags.InvokeMethod, null, componentInstance, []);

                TemplateServiceConfiguration config = new()
                {
                    BaseTemplateType = typeof(GlobalTemplateBase<>),
                    CachingProvider = new VercelFrameworkCachingProvider(),
                    Debug = true
                };

                var service = RazorEngineService.Create(config);

                service.AddTemplate(Path.GetFileNameWithoutExtension(file.Path).Replace(".component", ""), $"{Path.GetFileNameWithoutExtension(file.Path)}.cshtml");
            }
        }
    }

    public bool IsInRegistry(string keyName)
    {
        var component = ComponentRegistry[keyName];

        return component != null;
    }
}