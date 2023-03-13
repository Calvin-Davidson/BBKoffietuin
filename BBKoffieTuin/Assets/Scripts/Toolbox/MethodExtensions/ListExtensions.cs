using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Toolbox.MethodExtensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Checks if the list is empty 
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IList<T> target)
        {
            return target.Count == 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(this IList<T> list, int index)
        {
            if (index < 0) index = list.Count + index;
            else if (index > list.Count - 1) index %= list.Count;

            return list[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static void SetAt<T>(this IList<T> list, int index, T item)
        {
            if (index < 0) index = list.Count + index;
            else if (index > list.Count - 1) index %= list.Count;

            list.Insert(index, item);
        }

        /// <summary>
        /// ContainsSlot checks if the given number is between the list size.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ContainsSlot<T>(this IList<T> list, int index)
        {
            return index >= 0 && list.Count - 1 >= index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldList"></param>
        /// <typeparam name="TY"></typeparam>
        /// <typeparam name="TU"></typeparam>
        public static List<TU> ConvertListItemsTo<TY, TU>(this IList<TY> oldList) where TU : class
        {
            return oldList.Select(oldItem => oldItem as TU).ToList();
        }

        public static List<T> Shuffle<T>(this IList<T> oldList)
        {
            Random random = new Random();
            int n = oldList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (oldList[k], oldList[n]) = (oldList[n], oldList[k]);
            }
            return oldList.ToList();
        }
    }
}