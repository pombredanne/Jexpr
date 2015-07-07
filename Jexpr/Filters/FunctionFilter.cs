using System;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    public class FunctionFilter : AbstractFilter
    {
        public FunctionFilter(string property, FunctionOperator op)
        {
            Property = property;
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
                                                .value() )", parameterToChain, Property);
                    break;
                case FunctionOperator.Max:
                    result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder()
                                                .max()
                                                .value() )",
                parameterToChain, Property);
                    break;
            }

            return result;
        }
    }
}