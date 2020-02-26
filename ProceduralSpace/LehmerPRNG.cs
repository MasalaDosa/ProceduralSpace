using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralSpace
{
    class LehmerPRNG
    {
        private uint seed;

        public LehmerPRNG(uint seed)
        {
            this.seed = seed;
        }

        public uint NextUInt()
        {
            this.seed += 0xe120fc15;
            ulong tmp;
            tmp = (ulong)this.seed * 0x4a39b70d;
            uint m1 = (uint)((tmp >> 32) ^ tmp);
            tmp = (ulong)m1 * 0x12fad5c9;
            uint m2 = (uint)((tmp >> 32) ^ tmp);
            return m2;
        }

        /// <summary>
        /// Returns a double greater than or equal to min and less than max.
        /// </summary>
        /// <returns></returns>
        public double NextDouble(double min, double max)
        {
            return ((double)NextUInt() / (double)(uint.MaxValue)) * (max - min) + min;
        }

        
        /// <summary>
        /// Returns an int greater than or equal to min and less than max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int NextInt(int min, int max)
        {
            return (int)(NextUInt() % (max - min)) + min;
        }

        /// <summary>
        /// Returns a boolean
        /// </summary>
        /// <returns></returns>
        public bool NextBool()
        {
            return NextInt(0, 2) == 1;
        }

        public bool NextBool(double chance)
        {
            if (chance <= 0 || chance > 1.0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(chance)}");
            }
            return NextDouble(0, 1) < chance;
        }

        /// <summary>
        /// Chooses one of the choices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="choices"></param>
        /// <returns></returns>
        public T NextChoice<T>(IEnumerable<T> choices)
        {
            var tmp = choices.ToArray();
            return tmp[NextInt(0, tmp.Length)];
        }

        public T NextChoice<T>(IEnumerable<T> choices, IEnumerable<double> weights)
        {
            var tmp = choices.ToArray();
            var tmpWeights = weights.ToArray();
            if (tmp.Length != tmpWeights.Length)
            {
                throw new ArgumentException($"{nameof(weights)}");
            }
            
            var d = NextDouble(0, weights.Sum());
            var runningTotal = 0d;
            for(int i = 0; i < tmp.Length; i++)
            {
                runningTotal += tmpWeights[i];
                if (d <= runningTotal)
                    return tmp[i];
            }
            return choices.Last();
        }
    }
}
