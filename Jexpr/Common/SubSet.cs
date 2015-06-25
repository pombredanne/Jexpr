using System;
using System.Collections.Generic;

namespace Jexpr.Common
{
    public class SubSet<T> : IEnumerable<IEnumerable<T>>
    {
        private readonly IList<T> _list;
        private readonly int _length;
        private readonly int _max;
        private int _count;

        public SubSet(IEnumerable<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            _list = new List<T>(list);
            _length = _list.Count;
            _count = 0;
            _max = (int)Math.Pow(2, _length);
        }

        public int Count
        {
            get { return _max; }
        }

        private IList<T> Next()
        {
            if (_count == _max)
            {
                return null;
            }

            uint uIndex = 0;

            IList<T> result = new List<T>();

            while (uIndex < _length)
            {
                if ((_count & (1u << (int)uIndex)) > 0)
                {
                    result.Add(_list[(int)uIndex]);
                }

                uIndex++;
            }

            _count++;

            return result;
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            IList<T> subset;
            while ((subset = Next()) != null)
            {
                yield return subset;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}