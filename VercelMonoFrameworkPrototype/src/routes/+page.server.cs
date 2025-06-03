using VercelMonoFrameworkPrototypeLibrary;

namespace VercelMonoFrameworkPrototype.Routes;

public class Index : IVercelFrameworkPage
{
    public object PreInit()
    {
        return new { };
    }

    public object Init()
    {
        return new
        {
            FirstName = "Test",
            LastName = "Lyddon"
        };
    }
}