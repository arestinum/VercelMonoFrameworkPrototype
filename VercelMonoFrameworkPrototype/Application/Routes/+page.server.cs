using VercelMonoFrameworkPrototype.Framework;

namespace VercelMonoFrameworkPrototype.Routes;

public class Index
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