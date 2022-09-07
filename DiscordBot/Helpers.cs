using System;
using System.Collections;

namespace DiscordBot
{
    public static class Helpers
    {
        public static int GetRandom(int minValue = 0, int maxValue = 101)
        {
            var random = new Random();
            
            return random.Next(minValue, maxValue);
        }

        public static void ShufleArray<T>(T[] array)
        {
            var rnd = new Random();

            for (int i = array.Length - 1; i >= 1; i--)
            {
                int j = rnd.Next() % (i + 1);

                (array[j], array[i]) = (array[i], array[j]);
            }
        }
        
        public static bool CollectionIsNullOrEmpty(IEnumerable collection)
        {
            return collection == null || !collection.GetEnumerator().MoveNext();
        }
    }
}
