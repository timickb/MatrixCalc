using System;
using System.IO;
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
                return "Использование: save <matrix_name> <path_to_dir>";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }

            try
            {
                var path = Path.GetFullPath(args[2]);
            }
            catch (ArgumentException)
            {
                return "Некорректный путь к директории.";
            }
            catch (PathTooLongException)
            {
                return "Путь к директории слишком длинный.";
            }

            return "";

        }
        
        
    }
}