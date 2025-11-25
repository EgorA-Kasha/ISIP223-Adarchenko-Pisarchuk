using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class RandomProvider
{
    private static readonly Random rand = new Random();

    public static int Next(int maxValue) => rand.Next(maxValue);
}

