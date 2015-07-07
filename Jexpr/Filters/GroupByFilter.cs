namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupByFilter : AbstractFilter
    {
        private readonly string _key;
        private readonly string _groupSet;

        public GroupByFilter(string propertyToVisit, string key, string groupSet)
            : base(propertyToVisit)
        {
            _key = key;
            _groupSet = groupSet;
        }


        public override string ToJs(string parameterToChain)
        {
            string result = string.Format(@"( _.chain({0})
                                                    .groupBy(function(item){{
                                                          var parts= '{1}'.split('.')
                                                          return item[parts[0]][parts[1]]        
                                                    }})
                                                    .pairs()
                                                    .map(function (currentItem) {{
                                                        return _.object(_.zip(['{2}', '{3}'], currentItem))
                                                    }})
                                                    .value()
                                                )", parameterToChain, PropertyToVisit, _key, _groupSet);

            return result;
        }
    }
}