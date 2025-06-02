using VercelMonoFrameworkPrototypeLibrary;

namespace VercelMonoFrameworkPrototype;

public class IndexPage : IVercelFrameworkPage
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