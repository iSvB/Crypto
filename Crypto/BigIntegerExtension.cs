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
            int maxLength = (int)BigInteger.Log(max, 16);
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
    }
}
