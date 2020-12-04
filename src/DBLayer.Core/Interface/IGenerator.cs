using System;
using System.Collections.Generic;
using System.Text;

namespace DBLayer.Core.Interface
{
    public interface IGenerator
    {
        /// <summary>
        /// unique Identity
        /// </summary>
        /// <returns></returns>
        object Generate();
    }
    public interface IDeGenerator 
    {
        string AnalyzeId(long id);
    }
}
