using System.Configuration;
using VercelMonoFrameworkPrototypeLibrary.Enums;

namespace VercelMonoFrameworkPrototypeLibrary;

public class VercelFrameworkConfigurator
{
    private VercelFrameworkTemplater _templater = VercelFrameworkTemplater.RazorEngine;
    private string _definedRootPath = string.Empty;

    private string SetRootPath()
    {
        var rootPath = ConfigurationManager.AppSettings["VercelFrameworkTemplater.TemplaterRootPath"];

        // if (!string.IsNullOrEmpty(rootPath))
        // {
        //     _definedRootPath = rootPath;
        // }

        return rootPath;
    }

    private VercelFrameworkTemplater SetTemplateConfiguration()
    {
        var templater = ConfigurationManager.AppSettings["VercelFrameworkTemplater.Templater"];
        VercelFrameworkTemplater templaterEnum = VercelFrameworkTemplater.RazorEngine;

        if (!string.IsNullOrEmpty(templater))
        {
            switch (templater.ToLower())
            {
                // Skip over RazorEngine, as it is defaulted.
                case "fluid":
                    templaterEnum = VercelFrameworkTemplater.Fluid;
                    break;
                case "handlebars":
                    templaterEnum = VercelFrameworkTemplater.Handlebars;
                    break;
                case "custom":
                    templaterEnum = VercelFrameworkTemplater.Custom;
                    break;
            }
        }

        return templaterEnum;
    }

    public VercelFrameworkConfigurator()
    {
        SetTemplateConfiguration();
        SetRootPath();
    }

    public VercelFrameworkTemplater Templater
    {
        get
        {
            return SetTemplateConfiguration();
        }
    }

    public string RootPath
    {
        get { return SetRootPath(); }
    }
}