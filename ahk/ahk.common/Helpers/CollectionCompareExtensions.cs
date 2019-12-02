using System.Collections.Generic;

namespace ahk.common
{
    public static class CollectionCompareExtensions
    {
        public static bool SetEquals<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            HashSet<T> hashSet = new HashSet<T>(source);
            return hashSet.SetEquals(other);
        }
    }
}
