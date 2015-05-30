using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Math;

namespace Crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var bits = 1024;
            var min = BigInteger.Pow(2, bits);
            var max = BigInteger.Pow(2, bits+1);
            var prime = PrimeNumber.Create(min, max, 1e-6);
            sw.Stop();
            WriteLine(prime);
            WriteLine(sw.Elapsed);
            ReadKey(true);
        }
    }
}
