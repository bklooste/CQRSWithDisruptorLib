using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Util
{
    public static class NumberHelper
    {
        public static float EnsureInRange(float value, float min, float max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        public static decimal EnsureInRange(decimal value, decimal min, decimal max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }


        public static int EnsureInRangeAndInt(float value, float min, float max)
        {
            return Convert.ToInt32(EnsureInRange(value, min, max));

        }
        public static int EnsureInRangeAndInt(float value, int min)
        {
            return Convert.ToInt32(EnsureInRange(value, min, 1000));

        }
    }
}
