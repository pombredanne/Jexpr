using System;
using System.Collections.Generic;
using System.Linq;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectFilter : AbstractFilter
    {
        public string Assignee { get; private set; }

        public SelectFilter(string propertyToVisit = "", string assignee = "")
            : base(propertyToVisit)
        {
            Assignee = assignee;
        }

        public override string ToJs(string parameterToChain)
        {
            var innerFilterJsResult = GetJsFrom(Conditions);

            string assignToParameter = !string.IsNullOrEmpty(Assignee) ? string.Format("p.{0} =", Assignee) : String.Format("{0} =", parameterToChain);

            string result = string.Format(@"(  
                                    {2} (function () {{
                                            var _pFilterResult= _.chain({0}).filter(function (item) {{

                                              if({1}){{
                                                 return true;
                                              }}
                                              
                                              return false;

                                            }}).value();

                                            return _pFilterResult;
                                        }})()
                                    )",
                parameterToChain,
                innerFilterJsResult,
                assignToParameter);

            return result;
        }

        private string GetJsFrom(List<AbstractFilter> filters)
        {
            List<string> listerExps = filters.Select(filter => filter.ToJs(String.Format("item.{0}", filter.PropertyToVisit))).ToList();

            return String.Join(" && ", listerExps);
        }
    }
}