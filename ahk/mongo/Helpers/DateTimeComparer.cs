using System;
using System.Collections.Generic;

namespace adatvez.Helpers
{
    internal class DateTimeComparer : IEqualityComparer<DateTime>
    {
        private readonly TimeSpan leeway;

        public DateTimeComparer(TimeSpan leeway) => this.leeway = leeway;

        public bool Equals(DateTime x, DateTime y)
        {
            if (x == y)
                return true;

            var distance = x - y;
            if (distance.Ticks < 0)
                distance = distance.Negate();

            return distance < leeway;
        }

        public int GetHashCode(DateTime obj) => 0;
    }
}
