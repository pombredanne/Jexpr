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
        public decimal Value { get; set; }
        public ApplyOperator Operator { get; set; }

        public ApplyToSumFilter(string propertyToVisit, string resultProperty, decimal value, ApplyOperator applyOperator)
            : base(propertyToVisit)
        {
            Value = value;
            Operator = applyOperator;
            ResultProperty = resultProperty;
        }

        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string result = String.Empty;

            var sumExpression = base.ToJs(parameterToChain);

            switch (Operator)
            {
                case ApplyOperator.Exact:
                    result = String.Format(@"( ({0}) - ({1})", sumExpression, Value);
                    break;
                case ApplyOperator.Percent:
                    result = string.Format(@"( ({0}) * ({1} / 100) )", sumExpression, Value);
                    break;
            }

            return result;
        }
    }
}