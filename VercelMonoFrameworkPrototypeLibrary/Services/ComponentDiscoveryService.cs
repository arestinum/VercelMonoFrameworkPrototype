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
    public HtmlDocument? Content { get; set; }
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

}

public class ComponentDiscoveryService
{

}