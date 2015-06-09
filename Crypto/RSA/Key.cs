using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static Crypto.BigIntegerExtension;

namespace Crypto.RSA
{
    public struct Key
    {
        #region Fields
        #endregion
        #region Properties

        public BigInteger Exponent { get; private set; }

        public BigInteger Module { get; private set; }

        #endregion
        #region Constructors

        public Key(BigInteger exponent, BigInteger module)
        {
            Exponent = exponent;
            Module = module;
        }

        #endregion
        #region Methods

        public static void Create(int size, double probability, BigInteger publicExponent, 
            out Key publicKey, out Key privateKey)
        {
            BigInteger p, q;
            p = PrimeNumber.Create(size / 2, probability);
            q = PrimeNumber.Create(size - size / 2, probability);
            var module = p * q;
            // Для простого n: phi(n) = n-1. Мультипликативная.
            var phi = (p - 1) * (q - 1);
            if (publicExponent >= phi)
                throw new ArgumentException("Открытая экспонента должна быть меньше " +
                    "значения функции Эйлера от модуля n.");
            publicKey = new Key(publicExponent, module);
            privateKey = new Key(ModularInverse(publicExponent, phi), module);
        }

        public static void Create(BigInteger p, BigInteger q, BigInteger publicExponent, 
            out Key publicKey, out Key privateKey)
        {
            var module = p * q;
            var phi = (p - 1) * (q - 1);
            if (publicExponent >= phi)
                throw new ArgumentException("Открытая экспонента должна быть меньше " +
                    "значения функции Эйлера от модуля n.");
            publicKey = new Key(publicExponent, module);
            privateKey = new Key(ModularInverse(publicExponent, phi), module);
        }

        public override string ToString()
        {
            return $"Exponent: {Exponent} Module {Module}";
        }

        #endregion              
    }
}
