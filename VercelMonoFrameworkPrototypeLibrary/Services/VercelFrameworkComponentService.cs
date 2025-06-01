using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using HtmlAgilityPack;
using Microsoft.CSharp;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

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

        // Path.GetFileNameWithoutExtension

        results.Files.Select(file =>
        {
            CSharpCodeProvider provider = new();
            provider.CompileAssemblyFromFile(file.Path);

            return new ComponentRegistry()
            {
                CreatedAt = DateTime.Now,
                LastWritten = File.GetLastWriteTime(file.Path),
            };
        });
    }

    public bool IsInRegistry(string keyName)
    {
        var component = ComponentRegistry[keyName];

        return component != null;
    }
}