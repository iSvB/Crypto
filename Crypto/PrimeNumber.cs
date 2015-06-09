using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static System.Math;
using static Crypto.BigIntegerExtension;
using System.Threading;

namespace Crypto
{
    /// <summary>
    /// Простое число.
    /// </summary>
    public static class PrimeNumber
    {
        #region Constants

        /// <summary>
        /// Верхняя граница интервала простых чисел, определяемых для проверки числа на простоту.
        /// </summary>
        private const int smallPrimesBorder = 250;

        #endregion
        #region Fields

        private static Random prng = new Random();

        /// <summary>
        /// Простые числа, не превосходящие заданную границу.
        /// </summary>
        private static IEnumerable<int> smallPrimes = 
            Enumerable.Range(3, smallPrimesBorder - 3).Where(x => IsPrimeNaive(x));

        #endregion
        #region Methods
        #region Private        

        /// <summary>
        /// Определяет, является ли число простым, путём деления на числа,
        /// не превосходящие квадратного корня из данного числа.
        /// </summary>
        private static bool IsPrimeNaive(int value)
        {
            if (value < 0)
                throw new ArgumentException("Требуется положительное число!", nameof(value));
            if (value == 1)
                return true;
            int border = (int)Floor(Sqrt(value));
            if (value % 2 == 0)
                return false;
            // Если проверяемое число делится нацело, то оно составное.
            for (int i = 3; i < border; i += 2)
                if (value % i == 0)
                    return false;
            // Делителей не найдено => проверяемое число простое.
            return true;
        }

        /// <summary>
        /// Вероятностный тест простоты числа Миллера-Рабина.
        /// </summary>
        /// <param name="value">Проверяемое на простоту число.</param>
        /// <param name="r">Количество раундов теста, которые требуется провести.</param>
        /// <returns>Истина - если число вероятно простое, в противном случае - ложь.</returns>
        private static bool MillerRabinTest(BigInteger value, int r)
        {
            BigInteger decrValue = value - 1;
            // Представим value-1 в виде (2^s) * t, где t - нечётно.
            int s = 0;
            BigInteger t;
            do
                t = decrValue / BigInteger.Pow(2, ++s);
            while (t % 2 == 0);            
            // Выполним r раундов теста.
            for (int i = 0; i < r; ++i)
            {
                BigInteger a = Generate(2, value - 2);
                BigInteger x = BigInteger.ModPow(a, t, value);
                if (x == 1 || x == decrValue)
                    continue;
                for (int j = 0; j < s - 1; ++j)
                {
                    x = BigInteger.ModPow(x, 2, value);
                    if (x == 1)
                        return false;
                    if (x == decrValue)
                        goto continueLabel;
                }
                return false;
                continueLabel: continue;                
            }
            return true;
        }

        /// <summary>
        /// Определяет количество раундов теста Миллера-Рабина, которые необходимо провести 
        /// для достижения требуемой вероятности.
        /// </summary>
        /// <param name="probability">Требуемая вероятность того, что число составное.</param>
        /// <returns>Количество раундов теста Миллера-Рабина, которые требуется провести.</returns>
        private static int CalculateMillerRabinTestRounds(double probability)
        {
            if (probability < 0)
                throw new ArgumentException("Требуется положительное число!", nameof(probability));
            return (int)Ceiling(-Log(probability, 4));
        }

        /// <summary>
        /// Осуществляет поиск простого числа, путём перебора всех числел, начиная с <code>value</code> до 
        /// <code>border</code> с шагом равным 2 по модулю.
        /// </summary>
        /// <param name="value">Начальное значение.</param>
        /// <param name="border">Граница интервала, в сторону которой будем двигатся.</param>
        /// <param name="probability">Требуемая вероятность того, что данное число составное.</param>
        /// <returns>Простое число, либо -1, если на заданном интервале нет простых чисел.</returns>
        private static BigInteger MoveToPrime(BigInteger value, BigInteger border, double probability)
        {
            int summand = (border >= value) ? 2 : -2;
            int compareResult = (summand > 0) ? -1 : 1;
            while (value.CompareTo(border) == compareResult)
            {
                value += summand;
                if (IsPrime(value, probability))
                    return value;
            }
            return BigInteger.MinusOne;
        }

        /// <summary>
        /// Генерирует случайное нечётное число указанной длины (длина указана в битах).
        /// </summary>
        /// <param name="length">Длина генерируемого числа в битах.</param>
        /// <returns>Нечётное случайное число.</returns>
        public static BigInteger GenerateOdd(int bitsLength)
        {
            byte[] buffer = GenerateBits(bitsLength);
            // Число должно быть нечётным => должно оканчиваться на единицу.
            buffer[0] |= 0x01; 
            return new BigInteger(buffer);
        }
        
        /// <summary>
        /// Генерирует число представленное в виде массива байтов в порядке от младшего к старшему.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        private static byte[] GenerateBits(int bits)
        {
            // Число значащих бит в последнем байте. 
            int lastByteBits = (bits - 1) % 8;
            // Для выполнения битовой операции ИЛИ, с целью установить значение последнего значащего бита в 1.
            byte orMask = (byte)(1 << lastByteBits);
            // Для выполнения битовой операции И, с целью установить значения битов после последнего значащего в 0.            
            byte andMask = (byte)(255 >> 8 - lastByteBits - 1);
            // Количество байт в числе.
            int bytes = (int)Ceiling(bits / 8d);
            // Генерируем случайные байты.
            var buffer = new byte[bytes+1];
            prng.NextBytes(buffer);
            buffer[bytes] = 0;
            // Применяем битовые операции к последнему байту (старшему).
            buffer[bytes - 1] |= orMask;
            buffer[bytes - 1] &= andMask;
            return buffer;
        }        

        #endregion
        #region Public

        /// <summary>
        /// Определяет, является ли данное число простым, либо составным.
        /// </summary>
        /// <param name="value">Проверяемое на простоту число.</param>
        /// <param name="probability">Вероятность того, что число составное.</param>
        /// <returns>Истина - число вероятно простое, в противном случае - ложь.</returns>
        public static bool IsPrime(BigInteger value, double probability)
        {
            if (value < 0)
                throw new ArgumentException("Требуется положительное число!", nameof(value));
            // Если проверяемое число не превосходит верхнюю границу интервала, на котором найдены простые числа, 
            // то просто проверяем вхождение данного числа в множество найденных простых.
            if (value <= smallPrimesBorder)
                return smallPrimes.Contains((int)value);
            // Чётные числа являются составными.
            if (value.IsEven)
                return false;
            // Проверим делимость проверяемого числа на все найденные на заданном интервале простые числа.
            // Если делится без остатка - составное.
            if (smallPrimes.Any(x => value % x == 0))
                return false;
            // Определим количество требуемых раундов теста Миллера-Рабина.
            int r = CalculateMillerRabinTestRounds(probability);
            // Выполним вероятностный тест простоты Миллера-Рабина.
            return MillerRabinTest(value, r);
        }
        
        /// <summary>
        /// Создаёт вероятно простое число указанной длины.
        /// </summary>
        /// <param name="length">Длина числа в битах.</param>
        /// <param name="probability">Вероятность того, что число составное.</param>
        /// <returns>Вероятно простое число.</returns>
        public static BigInteger Create(int length, double probability)
        {
            if (length <= Log(smallPrimesBorder, 2))
            {
                int max = 1 << length;
                if (max <= smallPrimesBorder)
                    return smallPrimes.ElementAt(prng.Next(smallPrimes.Count(x => x <= max)));
            }
            Func<BigInteger> func = () => {
                BigInteger result;
                while (true)                                    
                    if (IsPrime((result = GenerateOdd(length)), probability))
                        return result;
            };
            var cts = new CancellationTokenSource();
            var tasks = new Task<BigInteger>[Environment.ProcessorCount];
            for (int i = 0; i < tasks.Length; ++i)
                tasks[i] = Task.Factory.StartNew(func, cts.Token);
            int index = Task.WaitAny(tasks);
            cts.Cancel();
            return tasks[index].Result;
        }

        #endregion
        #endregion
    }
}
