using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class DisplayRank : ICommand
    {
        public DisplayRank(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 2)
            {
                return "Использование: rank <matrix_name>";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }

            return Matrix.Storage[args[1]].Rank.ToString();
        }
        
    }
}