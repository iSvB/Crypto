using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static System.Math;

namespace Crypto
{
    /// <summary>
    /// Простое число.
    /// </summary>
    public class PrimeNumber
    {
        #region Fields
        
        /// <summary>
        /// Значение простого числа. 
        /// </summary>
        private BigInteger value;

        #endregion
        #region Constructors

        /// <summary>
        /// Создаёт случайное новое простое число принадлежащее указанному интервалу.
        /// </summary>
        /// <param name="minValue">Нижняя граница интервала (включённая).</param>
        /// <param name="maxValue">Верхняя граница интервала (включённая).</param>
        /// <param name="probability">Вероятность того, что число не простое.</param>
        public PrimeNumber(BigInteger minValue, BigInteger maxValue, double probability)
        {
            // Количество требуемых раундов теста Миллера-Рабина.
            int r = (int)Ceiling(-Log(probability, 4));
        }

        #endregion
        #region Methods
        #region Private

        

        #endregion
        #region Public

        public override string ToString()
        {
            return value.ToString();
        }

        #endregion
        #endregion
        #region Operators
        #endregion
    }
}
