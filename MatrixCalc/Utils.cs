using System;

namespace MatrixCalc
{
    public static class Utils
    {
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
    }
}