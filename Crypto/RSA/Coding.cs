using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.RSA
{
    public static class Coding
    {
        public static byte[] Cipher(byte[] message, Key key)
        {
            return BigInteger.ModPow(new BigInteger(message), key.Exponent, key.Module).ToByteArray();
        }
    }
}
