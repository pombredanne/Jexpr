using System.Collections.Generic;

namespace Jexpr.Core
{
    internal interface IScriptLoader
    {
        string Load(Dictionary<string, string> pathList);
    }
}