using Newtonsoft.Json;

namespace Jexpr.Filters
{
    public class ExistsFilter : JexprFilter
    {

        public object ValueToSearch { get; set; }

        public override string ToJs(string parameterToChain)
        {

            string result = string.Format(@"(  
                                    (function () {{
                                            var _pFound=false;
                                            _.chain({0}).pluck('{1}').each(function (key) {{
                                                if ({2}.indexOf(key) !== -1) {{
                                                   _pFound = true;
                                                    return true;
                                                }}

                                                return false;

                                            }}).value();

                                            return _pFound;
                                        }})()
                                    )",
                parameterToChain, PropertyToVisit, JsonConvert.SerializeObject(ValueToSearch));

            return result;
        }
    }
}