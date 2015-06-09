using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Crypto
{
    public static class BigIntegerExtension
    {
        private static Random prng = new Random();

        /// <summary>
        /// Генерирует случайное число на заданном интервале.
        /// </summary>
        /// <param name="min">Инклюзивная нижняя граница интервала.</param>
        /// <param name="max">Инклюзивная верхняя граница интервала.</param>       
        public static BigInteger Generate(BigInteger min, BigInteger max)
        {
            // Длина заданного интервала.            
            BigInteger interval = max - min + 1;
            // Генерируем число не превосходящее разницу между заданными границами интервала.
            BigInteger diff = Generate(interval);
            // Добавляем к нижней границе интервала сгенерированное число.
            return min + diff;
        }

        /// <summary>
        /// Генерирует случайное число не превосходящее заданное.
        /// </summary>
        /// <param name="max">Инклюзивная верхняя граница интервала.</param>
        public static BigInteger Generate(BigInteger max)
        {
            BigInteger result;
            // Максимальная длина записи генерируемого числа в системе счисления с основанием 16.
            int maxLength = (int)BigInteger.Log(max, 256);
            // Генерируемое число представленное байтами, в порядке от младшего к старшему.
            // { 3, 2, 1 } => 0x10203
            var buffer = new byte[maxLength + 1];
            do
            {
                prng.NextBytes(buffer);
                buffer[maxLength] = 0x00;
                result = new BigInteger(buffer);
            } while (result > max);
            return result;
        }

        /// <summary>
        /// Поиск мультипликативно обратного к числу х по модулю m.
        /// </summary>
        public static BigInteger ModularInverse(BigInteger x, BigInteger m)
        {
            BigInteger a0 = x;
            BigInteger a1 = m;
            BigInteger x0 = 1;
            BigInteger x1 = 0;
            BigInteger y0 = 0;
            BigInteger y1 = 1;
            while (a1 != 0)
            {
                var q = a0 / a1;
                var temp = a0;
                a0 = a1;
                a1 = temp - a1 * q;
                temp = x0;
                x0 = x1;
                x1 = temp - x1 * q;
                temp = y0;
                y0 = y1;
                y1 = temp - y1 * q;
            }
            return (x0 < 0) ? x0 + m : x0;
        }
    }
}
