using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedRPG
{
    // рандом
    public static class RandomHelper
    {
        private static readonly Random rand = new Random();
    
        public static int Next(int max)
        {
            return rand.Next(max);
        }
    
        public static int Next(int min, int max)
        {
            return rand.Next(min, max);
        }
    
        public static double NextDouble()
        {
            return rand.NextDouble();
        }
    }
}
