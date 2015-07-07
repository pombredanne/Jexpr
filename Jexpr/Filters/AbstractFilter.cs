using System.Collections.Generic;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractFilter
    {
        public string PropertyToVisit { get; set; }
        public List<AbstractFilter> Conditions { get; set; }

        public abstract string ToJs(string parameterToChain);
    }
}