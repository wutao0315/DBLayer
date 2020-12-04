using DBLayer.Core.Interface;
using System;

namespace DBLayer.Persistence.Generator
{

    public class GUIDGenerator : IGenerator
    {
        public object Generate()
        {
            return Guid.NewGuid();
        }
    }
}
