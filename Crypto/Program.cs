using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Math;
using static Crypto.PrimeNumber;

namespace Crypto
{
    class Program
    {
        static BigInteger sourceMessage, decodedMessage;
        static byte[] srcMsg, encMsg, decMsg;
        static RSA.Key publicKey, privateKey;

        static void Main(string[] args)
        {
            WriteLine("Wiki example:");
            // https://ru.wikipedia.org/wiki/RSA
            BigInteger p = 3557;
            BigInteger q = 2579;
            BigInteger e = 3;
            RSA.Key.Create(p, q, e, out publicKey, out privateKey);
            sourceMessage = 111111;
            CalcRSA();        
            OutputRSA();

            int bytes = 8;
            int bits = bytes * 8;
            int pubExp = 157;
            WriteLine("\n\n\nRandom example:");
            WriteLine($"\t{bits} bits keys");
            WriteLine($"\t{(bytes - 1) * 8} bits message");
            WriteLine($"\tPublic exponent: { pubExp}");
            RSA.Key.Create(bits, 1e-6, pubExp, out publicKey, out privateKey);
            var buf = new byte[bytes-1];
            new Random().NextBytes(buf);
            sourceMessage = new BigInteger(buf);
            CalcRSA();
            OutputRSA();

            ReadKey(true);
        }

        private static void CalcRSA()
        {            
            srcMsg = sourceMessage.ToByteArray();
            encMsg = RSA.Coding.Cipher(srcMsg, publicKey);
            decMsg = RSA.Coding.Cipher(encMsg, privateKey);
            decodedMessage = new BigInteger(decMsg);
        }

        private static void OutputRSA()
        {
            WriteLine($"Public key:\n\t{publicKey}");
            WriteLine($"Private key:\n\t{privateKey}");
            WriteLine();
            WriteLine("Source message:");
            WriteLine(sourceMessage);
            WriteLine(srcMsg.ToStr());
            WriteLine();
            WriteLine("Encoded message:");
            WriteLine(encMsg.ToStr());
            WriteLine();
            WriteLine("Decoded message:");
            WriteLine(decodedMessage);
            WriteLine(decMsg.ToStr());
            WriteLine();
            WriteLine($"Is correct: {decodedMessage == sourceMessage}");
        }
    }

    public static class ByteEnumerableExtension
    {
        public static string ToStr(this IEnumerable<byte> collection)
        {
            var strs = new List<string>(collection.Count());
            foreach (var value in collection)
                strs.Add($"{value:X2}");
            return string.Join(" ", strs);
        }
    }
}
