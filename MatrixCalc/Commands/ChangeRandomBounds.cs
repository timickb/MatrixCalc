using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class ChangeRandomBounds : ICommand
    {
        public ChangeRandomBounds(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 3)
            {
                return "Использование: setrnd <lower_bound_int> <upper_bound_int>";
            }

            int lowerBound, upperBound;
            if (!int.TryParse(args[1], out lowerBound) || !int.TryParse(args[2], out upperBound))
            {
                return "Значения должны быть целыми числами.";
            }

            if (Math.Abs(lowerBound) > Matrix.MaxAbsValue || Math.Abs(upperBound) > Matrix.MaxAbsValue)
            {
                return $"Значения по модулю не должны превышать {Matrix.MaxAbsValue}";
            }

            if (upperBound <= lowerBound)
            {
                return "Второе значение должно быть строго больше первого.";
            }

            Matrix.LowerRandomBound = lowerBound;
            Matrix.UpperRandomBound = upperBound;
            return $"Значения в генераторе случайных матриц теперь будут лежать в [{lowerBound}; {upperBound})";
        }
    }
}