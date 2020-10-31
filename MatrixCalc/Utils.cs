using System;
using System.Globalization;

namespace MatrixCalc
{
    public static class Utils
    {
        /// <summary>
        /// Проверяет имя матрицы на соответствие правилам.
        /// </summary>
        /// <param name="name">имя матрицы</param>
        /// <returns>статус соответствия правилам</returns>
        public static bool IsMatrixNameCorrect(string name)
        {
            char firstSymbol = name[0];
            
            // Имя не должно начинаться с цифры.
            if (firstSymbol >= 48 && firstSymbol <= 57)
            {
                return false;
            }
            
            // Имя не должно содержать символов, отличных от цифр и букв.
            foreach (var t in name)
            {
                if (!char.IsDigit(t) && !char.IsLetter(t))
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// В зависимости от региональных настроек заменяет в строковом
        /// представлении вещественного числа точку на запятую или наоборот.
        /// </summary>
        /// <param name="word">вещественное число в строковом представлении</param>
        /// <returns>пропатченная строка</returns>
        public static string PrepareDecimal(string word)
        {
            var sep = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            if (sep == ".")
            {
                return word.Replace(",", ".");
            }

            return word.Replace(".", ",");
        }
    }
}