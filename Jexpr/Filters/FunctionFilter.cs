using System;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class FunctionFilter : AbstractFilter
    {
        private readonly FunctionOperator _operator;

        public FunctionFilter(string propertyToVisit, FunctionOperator @operator) : base(propertyToVisit)
        {
            _operator = @operator;
        }

        public override string ToJs(string parameterToChain)
        {
            string result = String.Empty;

            switch (_operator)
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