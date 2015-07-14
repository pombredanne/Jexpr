using System.Collections.Generic;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractFilter
    {
        protected AbstractFilter(string propertyToVisit)
        {
            PropertyToVisit = propertyToVisit;
        }

        public string PropertyToVisit { get; private set; }
        public List<AbstractFilter> Conditions { get; set; }

        public abstract string ToJs(string parameterToChain);
    }
}