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
using System.Collections;

namespace Crypto
{
    class Program
    {
        // For RSA.
        static int bytes = 8;
        static int pubExp = 157;
        static BigInteger sourceMessage, decodedMessage;
        static byte[] srcMsg, encMsg, decMsg;
        static RSA.Key publicKey, privateKey;

        static void Main(string[] args)
        {
            DemonstrateRSA();
            ReadKey(true);
        }

        public static void DemonstrateRSA()
        {
            WriteLine("Wiki example:");
            BigInteger p = 3557;
            BigInteger q = 2579;
            BigInteger e = 3;
            RSA.Key.Create(p, q, e, out publicKey, out privateKey);
            sourceMessage = 111111;
            CalcRSA();
            OutputRSA();
            
            int bits = bytes * 8;            
            WriteLine("\n\n\nRandom example:");
            WriteLine($"\t{bits} bits keys");
            WriteLine($"\t{(bytes - 1) * 8} bits message");
            WriteLine($"\tPublic exponent: { pubExp}");
            RSA.Key.Create(bits, 1e-6, pubExp, out publicKey, out privateKey);
            var buf = new byte[bytes - 1];
            new Random().NextBytes(buf);
            sourceMessage = new BigInteger(buf);
            CalcRSA();
            OutputRSA();
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
            WriteLine(ToStr(srcMsg));
            WriteLine();
            WriteLine("Encoded message:");
            WriteLine(ToStr(encMsg));
            WriteLine();
            WriteLine("Decoded message:");
            WriteLine(decodedMessage);
            WriteLine(ToStr(decMsg));
            WriteLine();
            WriteLine($"Is correct: {decodedMessage == sourceMessage}");
        }

        private static string ToStr(IEnumerable<byte> collection)
        {
            var strs = new List<string>(collection.Count());
            foreach (var value in collection)
                strs.Add($"{value:X2}");
            return string.Join(" ", strs);
        }
    }
}
