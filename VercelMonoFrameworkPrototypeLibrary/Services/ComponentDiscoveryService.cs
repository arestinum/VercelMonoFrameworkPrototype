using System.Reflection;
using HtmlAgilityPack;

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
    public string Content { get; set; } = string.Empty;
}

public class VercelMonoFrameworkComponent
{
    public string FolderPath { get; set; } = string.Empty;
    public ComponentScriptServerSideFile? serverScript;
    public ComponentScriptClientSideFile? clientScript;
    public ComponentContentFile? content;

    public VercelMonoFrameworkComponent(string folderPath)
    {
        FolderPath = folderPath;

        var doc = new HtmlDocument();
        doc.Load(folderPath + "+page.cshtml");
    }
}

public class ComponentRegistry
{

}

public class ComponentDiscoveryService
{

}