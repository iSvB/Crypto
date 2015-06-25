using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Crypto.BigIntegerExtension;

namespace Crypto.RSA
{
    public static class Coding
    {
        public static byte[] Cipher(byte[] message, Key key)
        {
            return ModPow(new BigInteger(message), key.Module, key.Exponent).ToByteArray();
        }
    }
}
