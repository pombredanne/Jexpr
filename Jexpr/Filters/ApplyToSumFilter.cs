using System;
using Jexpr.Interface;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// Applies exact/percent value to sum of items
    /// </summary>
    public class ApplyToSumFilter : SumFilter, IHasResultProperty
    {
        private readonly decimal _value;
        private readonly ApplyOperator _applyOperator;

        public ApplyToSumFilter(string resultProperty, decimal value, ApplyOperator applyOperator)
        {
            _value = value;
            _applyOperator = applyOperator;
            ResultProperty = resultProperty;
        }

        public string ResultProperty { get; protected set; }

        public override string ToJs(string parameterToChain)
        {
            string result = String.Empty;

            var sumExpression = base.ToJs(parameterToChain);

            switch (_applyOperator)
            {
                case ApplyOperator.Exact:
                    result = String.Format(@"( ({0}) - ({1})", sumExpression, _value);
                    break;
                case ApplyOperator.Percent:
                    result = string.Format(@"( ({0}) * ({1} / 100) )", sumExpression, _value);
                    break;
            }

            return result;
        }
    }
}