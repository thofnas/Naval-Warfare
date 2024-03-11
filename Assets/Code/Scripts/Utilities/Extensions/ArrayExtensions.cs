using System;

namespace Utilities.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty<T>(T[] array) => array == null || array.Length == 0;

        /// <summary>
        ///     Retrieves a random item from the provided list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="array">The list from which to retrieve a random item.</param>
        /// <returns>
        ///     A randomly selected item of type <typeparamref name="T" /> from the list.
        ///     If the list is empty, the default value for type <typeparamref name="T" /> is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the list is empty.</exception>
        public static T GetRandomItem<T>(this T[] array)
        {
            if (IsNullOrEmpty(array))
                throw new ArgumentException("List is empty or null.");

            var random = new Random();
            int index = random.Next(array.Length); // Generates a random index within the range of the list
            T randomItem = array[index];

            return randomItem;
        }
    }
}