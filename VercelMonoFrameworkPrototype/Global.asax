<%@ Application Language="C#" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="VercelMonoFrameworkPrototypeLibrary" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        var vercelApp = new VercelMonoFrameworkApplication((HttpApplication)sender);
    }
</script>