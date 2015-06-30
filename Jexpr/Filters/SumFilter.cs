namespace Jexpr.Filters
{
    public class SumFilter : AbstractFilter
    {
        public string MultiplierPropertyToVisit { get; set; }
        public override string ToJs(string parameterToChain)
        {
            string result;

            if (!string.IsNullOrEmpty(MultiplierPropertyToVisit))
            {
                result = string.Format(@"  
                                  (function () {{
                                        var _pTotal=0;
                                        _.chain({0}).each(function (item) {{
                                            var _p{1} = item.{1};
                                            var _p{2} = item.{2};

                                            _pTotal += (_p{1} * _p{2});
                                        }}).value();
            
                                        return _pTotal;
                                    }})()
                                    ", parameterToChain, PropertyToVisit, MultiplierPropertyToVisit);
            }
            else
            {
                result = string.Format(@"  
                                  (function () {{
                                        var _pTotal=0;
                                        _.chain({0}).each(function (item) {{
                                            var _p{1} = item.{1};

                                            _pTotal += _p{1};
                                        }}).value();
            
                                        return _pTotal;
                                    }})()
                                    ",
                               parameterToChain, PropertyToVisit);
            }

            return result;
        }
    }
}