using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Utilities.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Log<T>(this List<T> messages)
        {
            foreach (T message in messages)
                Debug.Log(message);

            return messages;
        }
        
        public static IEnumerable<T> Log<T>(this IEnumerable<T> messages)
        {
            IEnumerable<T> enumerable = messages.ToList();
            foreach (T message in enumerable)
                Debug.Log(message);

            return enumerable;
        }
        

        /// <summary>
        ///     Determines whether a collection is null or has no elements
        ///     without having to enumerate the entire collection to get a count.
        ///     Uses LINQ's Any() method to determine if the collection is empty,
        ///     so there is some GC overhead.
        /// </summary>
        /// <param name="list">List to evaluate</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || !list.Any();

        /// <summary>
        ///     Creates a new list that is a copy of the original list.
        /// </summary>
        /// <param name="list">The original list to be copied.</param>
        /// <returns>A new list that is a copy of the original list.</returns>
        public static List<T> Clone<T>(this IList<T> list)
        {
            var newList = new List<T>();
            foreach (T item in list) newList.Add(item);

            return newList;
        }

        /// <summary>
        ///     Swaps two elements in the list at the specified indices.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index of the first element.</param>
        /// <param name="indexB">The index of the second element.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) =>
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);

        /// <summary>
        ///     Retrieves a random item from the provided list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list from which to retrieve a random item.</param>
        /// <returns>
        ///     A randomly selected item of type <typeparamref name="T" /> from the list.
        ///     If the list is empty, the default value for type <typeparamref name="T" /> is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the list is empty.</exception>
        public static T GetRandomItem<T>(this List<T> list)
        {
            if (IsNullOrEmpty(list))
                throw new ArgumentException("List is empty or null.");

            int index = new Random().Next(list.Count); // Generates a random index within the range of the list
            T randomItem = list[index];

            return randomItem;
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            list = list.OrderBy(_ => rng.Next()).ToList();
            return list;
        }
    }
}