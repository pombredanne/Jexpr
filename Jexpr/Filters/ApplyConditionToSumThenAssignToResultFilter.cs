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
        private readonly object _value;
        private readonly string _assignee;
        private readonly string _assigner;
        private readonly ConditionOperator _op;
        private readonly string _defaultAssigner;

        public ApplyConditionToSumThenAssignToResultFilter(string propertyToVisit, object value, string assignee, string assigner, ConditionOperator op, string defaultAssigner = "[]")
            : base(propertyToVisit)
        {
            _value = value;
            _assignee = assignee;
            _assigner = assigner;
            _op = op;
            _defaultAssigner = defaultAssigner;
        }

        public override string ToJs(string parameterToChain)
        {
            string sumExp = base.ToJs(parameterToChain);

            string assignToParameter = !string.IsNullOrEmpty(_assignee) ? string.Format("p.{0} = p.{1}", _assignee, _assigner) : String.Empty;

            var result = string.Format(@"
                    if({0} {1} {2}){{
                        {3}
                    }}else{{
                       {4}
                    }}
                    ",
                sumExp, _op.ToName(), _value,
                assignToParameter,
                string.Format("p.{0} = {1}", _assignee, _defaultAssigner));

            return result;
        }
    }
}