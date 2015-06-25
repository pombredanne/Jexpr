using System.Collections.Generic;
using System.Linq;

namespace Jexpr.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            HashSet<T> hashSet = new HashSet<T>(lists.First());

            foreach (var list in lists.Skip(1))
            {
                hashSet.IntersectWith(list);
            }

            return hashSet.ToList();
        }

        public static bool IsSubSet<T>(this IEnumerable<T> set, IEnumerable<T> toCheck)
        {
            return set.Count() == (toCheck.Intersect(set)).Count();
        }
    }
}