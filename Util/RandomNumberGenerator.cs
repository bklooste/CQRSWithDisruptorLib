using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L6.Infrastructure.Util
{
    public sealed class RandomNumberGenerator // : Random
    {
        private static Random _global = new Random();

        public static int Seed { get; set; }

        [ThreadStatic]
        private static Random _localInstance;

        static RandomNumberGenerator()
        {

            Seed = -1; 
        }

        public static Random Instance
        {
            get
            {
                Random inst = _localInstance;
                if (inst == null)
                {
                    if (Seed == -1)
                    {
                        lock (_global) Seed = _global.Next();
                    }
                    _localInstance = inst = new Random(Seed);
                }
                return _localInstance;
            }
        }
    }

}
