using System;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class FunctionFilter : AbstractFilter
    {
        public FunctionFilter(string propertyToVisit, FunctionOperator op)
        {
            PropertyToVisit = propertyToVisit;
            Operator = op;
        }

        public FunctionOperator Operator { get; private set; }

        public override string ToJs(string parameterToChain)
        {
            string result = String.Empty;

            switch (Operator)
            {
                case FunctionOperator.Min:
                    result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder(false)
                                                .min()
                                                .value() )", parameterToChain, PropertyToVisit);
                    break;
                case FunctionOperator.Max:
                    result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder()
                                                .max()
                                                .value() )",
                parameterToChain, PropertyToVisit);
                    break;
            }

            return result;
        }
    }
}