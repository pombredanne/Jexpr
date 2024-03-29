using System.Collections.Generic;
using System.Linq;
using Jexpr.Common;
using Jexpr.Operators;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AssignConditionalToResultFilter : AbstractFilter
    {
        public AssignConditionalToResultFilter(string propertyToVisit="")
            : base(propertyToVisit)
        {
            DefaultAssigner = "[]";
        }
        public ConditionOperator ConditionOperator { get; set; }
        public bool Value { get; set; }
        public string Assignee { get; set; }
        public string Assigner { get; set; }
        public string DefaultAssigner { get; set; }
        public override string ToJs(string parameterToChain)
        {
            List<string> innerFiltersExps = Conditions.Select(filter => filter.ToJs(parameterToChain)).ToList();

            string body = string.Join(string.Format(" {0} ", ConditionOperator.ToName()), innerFiltersExps);

            string assignToParameter = !string.IsNullOrEmpty(Assignee)
                ? string.Format("p.{0} = p.{1}", Assignee, Assigner)
                : string.Empty;

            var result = string.Format(@"
                    if({0} === true){{
                        {1}
                    }}else{{
                       {2}
                    }}
                    ",
                body, assignToParameter, string.Format("p.{0} = {1}", Assignee, DefaultAssigner)
                );

            return result;
        }
    }
}