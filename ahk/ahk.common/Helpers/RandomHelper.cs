using System;
using System.Threading;

namespace ahk.common.Helpers
{
    public static class RandomHelper
    {
        private static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(), trackAllValues: false);

        public static int GetRandomValue(int minValue, int maxValue)
            => random.Value.Next(minValue, maxValue);

        public static DateTime GetRandomValue(DateTime minValue, DateTime maxValue)
        {
            var range = maxValue.Ticks - minValue.Ticks;
            var ticks = (long)Math.Round(random.Value.NextDouble() * range) + minValue.Ticks;
            return new DateTime(ticks, DateTimeKind.Utc);
        }
    }
}
