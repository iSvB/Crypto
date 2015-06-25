using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
    /// <summary>
    /// Big endian.
    /// </summary>
    public class BitArray : IEnumerable<bool>
    {
        #region Fields

        private bool[] bitArray;      

        #endregion
        #region Properties

        public bool this[int index]
        {
            get
            {
                return bitArray[index];
            }

            set
            {
                bitArray[index] = value;
            }
        }

        public int Length
        {
            get
            {
                return bitArray.Length;
            }
        }

        #endregion
        #region Constructors

        public BitArray(int length)
        {
            bitArray = new bool[length];
        }

        public BitArray(Int64 bitArray)
        {
            this.bitArray = new bool[64];          
            for (int i = 0; i < 64; ++i)
                this[64 - i - 1] = (bitArray >> i) % 2 == 1 ? true : false;      
        }

        public BitArray(IList<bool> bitArray)
        {
            this.bitArray = new bool[bitArray.Count];
            for (int i = 0; i < bitArray.Count; ++i)
                this[i] = bitArray[i];
        }
        
        public BitArray(IList<int> bitArray)
        {
            this.bitArray = new bool[bitArray.Count];
            for (int i = 0; i < bitArray.Count; ++i)
                this[i] = (bitArray[i] == 0) ? false : true;
        }               

        public BitArray(BitArray big, BitArray little)
        {
            bitArray = new bool[big.Length + little.Length];
            for (int i = 0; i < big.Length; ++i)
                this[i] = big[i];
            for (int i = 0; i < little.Length; ++i)
                this[big.Length + i] = little[i];
        }

        #endregion
        #region Methods
        #region Private        

        private static void Fit(ref BitArray left, ref BitArray right)
        {
            int size = Math.Max(left.Length, right.Length);
            if (left.Length < size)
                left = new BitArray(new BitArray(Enumerable.Repeat(false, size - left.Length).ToList()), left);
            else if (right.Length < size)
                right = new BitArray(new BitArray(Enumerable.Repeat(false, size - right.Length).ToList()), right);
        }

        #endregion
        #region Public
        #region Static

        public static BitArray BitwiseOperation(BitArray left, BitArray right, Func<bool, bool, bool> operation)
        {
            Fit(ref left, ref right);
            int size = left.Length;
            var result = new BitArray(size);
            for (int i = 0; i < size; ++i)
                result[i] = operation(left[i], right[i]);
            return result;
        }

        public static BitArray Or(BitArray left, BitArray right)
        {
            return BitwiseOperation(left, right, (l, r) => l || r);
        }

        public static BitArray And(BitArray left, BitArray right)
        {
            return BitwiseOperation(left, right, (l, r) => l && r);
        }

        public static BitArray Xor(BitArray left, BitArray right)
        {
            return BitwiseOperation(left, right, (l, r) => l ^ r);
        }

        public static BitArray Not(BitArray x)
        {
            var result = new BitArray(x.Length);
            for (int i = 0; i < result.Length; ++i)
                result[i] = !x[i];
            return result;
        }

        #endregion

        public override string ToString()
        {
            var result = new StringBuilder(bitArray.Length);
            foreach (var bit in bitArray)
                result.Append(bit ? "1" : "0");
            return result.ToString();            
        }
        
        #endregion
        #endregion
        #region Operators

        public static BitArray operator |(BitArray left, BitArray right)
        {
            return Or(left, right);
        }

        public static BitArray operator &(BitArray left, BitArray right)
        {
            return And(left, right);
        }

        public static BitArray operator ^(BitArray left, BitArray right)
        {
            return Xor(left, right);
        }

        public static BitArray operator !(BitArray x)
        {
            return Not(x);
        }

        #endregion
        #region Interface implementations
        #region IEnumerable<bool>

        public IEnumerator<bool> GetEnumerator()
        {
            return ((IEnumerable<bool>)this.bitArray).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<bool>)this.bitArray).GetEnumerator();
        }

        #endregion
        #endregion
    }
}
