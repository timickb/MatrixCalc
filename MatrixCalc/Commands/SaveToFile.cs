using System;
using System.IO;
using System.Linq;
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

            try
            {
                var newFilePath = Path.GetFullPath(args[2]);
                var dirPath = Path.GetFullPath(Path.GetDirectoryName(newFilePath));
                if (!Directory.Exists(dirPath))
                {
                    return "Директории, в которой вы хотите создать файл, не существует.";
                }

                var fileName = Path.GetFileName(newFilePath);
                
                return Directory.GetFiles(dirPath).Contains(fileName)
                    ? "Такой файл уже существует, попробуйте другое имя."
                    : WriteMatrix(newFilePath, args[1]);
            }
            catch (ArgumentException)
            {
                return "Некорректный путь.";
            }
            catch (PathTooLongException)
            {
                return "Путь слишком длинный.";
            }
        }

        private string WriteMatrix(string path, string matrixName)
        {
            try
            {
                var sw = new StreamWriter(Path.GetFullPath(path));
                var matrix = Matrix.Storage[matrixName];
                for (var i = 0; i < matrix.RowsAmount; i++)
                {
                    var row = new decimal[matrix.ColsAmount];
                    for (var j = 0; j < matrix.ColsAmount; j++)
                    {
                        row[j] = matrix.GetValueAt(i, j);
                    }

                    sw.WriteLine(string.Join(" ", row));
                }

                sw.Flush();
                sw.Close();
                return $"Матрица успешно записана в файл {path}";
            }
            catch (IOException)
            {
                return "Не удалось создать файл. Возможно, вы потеряли доступ к директории или" +
                       "она была удалена; в данной директории уже был создан файл с таким имеенем.";
            }
            catch (UnauthorizedAccessException)
            {
                return "Ошибка доступа.";
            }
        }
    }
}