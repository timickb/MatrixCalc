using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class SaveToFile : ICommand
    {

        public SaveToFile(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 3)
            {
                return "Использование: save <matrix_name> <path_to_file>";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }

            return "";

        }
        
        
    }
}