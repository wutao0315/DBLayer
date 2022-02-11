using DBLayer.Core.Interface;

namespace DBLayer.Persistence.Generator;

public class GUIDGenerator : IGenerator
{
    public object Generate()
    {
        return Guid.NewGuid();
    }
}
