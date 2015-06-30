using System;

namespace Jexpr.Filters
{
    public class AssignTakeToResultFilter : TakeFilter, IHasResultProperty
    {
        public AssignTakeToResultFilter()
        {
            Take = 1;
        }
        public string ResultProperty { get; set; }

        public override string ToJs(string parameterToChain)
        {
            string sumExpression = base.ToJs(parameterToChain);

            string result = String.Format(@"( {0} )", sumExpression);

            return result;
        }
    }
}