using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PYDFramework
{
    public static class CollectionsExtention
    {
        public static T RandomElement<T>(this IList<T> list)
        {
            if (list.Count == 0)
                return default(T);

            var index = Random.Range(0, list.Count);
            return list[index];
        }

        public static T RandomElementByWeight<T>(this IEnumerable<T> list, System.Func<T, int> weights )
        {
            var totalWeight = list.Sum(weights);
            var value = Random.Range(0, totalWeight);
            var sum = 0;

            foreach(var item in list)
            {
                var weight = weights(item);
                sum += weight;

                if (value < sum)
                    return item;
            }    

            return default(T);
        }
    }
}
