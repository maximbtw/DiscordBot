using System;

namespace DiscordBot
{
    public static class CommandAsset
    {
        public static int GetRandom(int minValue = 0, int maxValue = 101)
        {
            var rnd = new Random();
            return rnd.Next(minValue, maxValue);
        }

        public static void ShufleArray<T>(T[] array)
        {
            var rnd = new Random();

            for (int i = array.Length - 1; i >= 1; i--)
            {
                int j = rnd.Next() % (i + 1);

                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }
    }
}
