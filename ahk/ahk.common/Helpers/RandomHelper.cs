using System;
using System.Threading;

namespace ahk.common.Helpers
{
    public static class RandomHelper
    {
        private static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(), trackAllValues: false);

        public static int GetRandomValue(int minValue, int maxValue)
            => random.Value.Next(minValue, maxValue);
    }
}
