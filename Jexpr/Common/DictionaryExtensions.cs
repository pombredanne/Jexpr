using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Jexpr.Common
{
    public static class DictionaryExtensions
    {
        public static dynamic ToDynamic(this IDictionary<string, object> dict)
        {
            IDictionary<string, object> result = new ExpandoObject() as IDictionary<string, object>;

            foreach (KeyValuePair<string, object> kvp in dict)
            {
                result.Add(kvp);
            }

            return result;
        }
    }
}