using System;
using System.Collections.Generic;

namespace Jexpr.Filters
{
    public class SelectFilter : AbstractFilter
    {
        public List<AbstractFilter> Filters { get; set; }
        public object ValueToSearch { get; set; }

        public string AssignTo { get; set; }

        public override string ToJs(string parameterToChain)
        {
            var innerFilterJsResult = GetJsFrom(Filters);

            string assignToParameter = !string.IsNullOrEmpty(AssignTo) ? string.Format("p.{0} =", AssignTo) : String.Empty;

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
            List<string> listerExps = new List<string>();

            foreach (var filter in filters)
            {
                listerExps.Add(filter.ToJs(String.Format("item.{0}", filter.PropertyToVisit)));
            }

            //HARDCODED AND
            return String.Join(" && ", listerExps);
        }
    }
}