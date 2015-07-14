using System;
using Jexpr.Common;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplyConditionToSumThenAssignToResultFilter : SumFilter
    {
        public object Value { get; private set; }
        public string Assignee { get; private set; }
        public string Assigner { get; private set; }
        public ConditionOperator Operator { get; private set; }
        public string DefaultAssigner { get; private set; }

        public ApplyConditionToSumThenAssignToResultFilter(string propertyToVisit, object value, string assignee, string assigner, ConditionOperator op, string defaultAssigner = "[]")
            : base(propertyToVisit)
        {
            Value = value;
            Assignee = assignee;
            Assigner = assigner;
            Operator = op;
            DefaultAssigner = defaultAssigner;
        }

        public override string ToJs(string parameterToChain)
        {
            string sumExp = base.ToJs(parameterToChain);

            string assignToParameter = !string.IsNullOrEmpty(Assignee) ? string.Format("p.{0} = p.{1}", Assignee, Assigner) : String.Empty;

            var result = string.Format(@"
                    if({0} {1} {2}){{
                        {3}
                    }}else{{
                       {4}
                    }}
                    ",
                sumExp, Operator.ToName(), Value,
                assignToParameter,
                string.Format("p.{0} = {1}", Assignee, DefaultAssigner));

            return result;
        }
    }
}