using System;

namespace Jexpr.Filters
{
    public class SetTakeToResultFilter : TakeFilter, IHasResultProperty
    {
        public SetTakeToResultFilter()
        {
            Take = 1;
        }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            var sumExpression = base.ToJs(parameterToChain);

            var result = String.Format(@"( {0} )", sumExpression);

            return result;
        }
    }
}