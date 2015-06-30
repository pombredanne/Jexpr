using System;
using Jexpr.Common;

namespace Jexpr.Filters
{
    public class AssignApplyArithmeticToSumToResultFilter : SumFilter
    {
        public AssignApplyArithmeticToSumToResultFilter()
        {
            DefaultAssignmentValue = "[]";
        }
        public ExpressionOp ExpressionOp { get; set; }
        public decimal Value { get; set; }
        public string AssignTo { get; set; }
        public string AssignmentValue { get; set; }
        public string DefaultAssignmentValue { get; set; }
        public override string ToJs(string parameterToChain)
        {
            string sumExp = base.ToJs(parameterToChain);

            string assignToParameter = !string.IsNullOrEmpty(AssignTo) ? string.Format("p.{0} = p.{1}", AssignTo, AssignmentValue) : String.Empty;

            var result = string.Format(@"
                    if({0} {1} {2}){{
                        {3}
                    }}else{{
                       {4}
                    }}
                    ",
                sumExp, ExpressionOp.ToName(), Value,
                assignToParameter,
                string.Format("p.{0} = {1}", AssignTo, DefaultAssignmentValue));

            return result;
        }
    }
}