using System;
using System.IO;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class LoadFromFile : ICommand
    {
        public LoadFromFile(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        private string ParseFile(string path)
        {
            
            try
            {
                var lines = File.ReadAllLines(path);
                // Проверим, имеют ли все строки одинаковое число слов.
                var n = lines[0].Split().Length;
                var m = lines.Length;
                var matrix = new decimal[m, n];
                var i = 0;
                var j = 0;
                foreach (var line in lines)
                {
                    var words = line.Split();
                    if (words.Length != n)
                    {
                        return "Некорректный формат файла: Строки имеют разное число элементов.";
                    }

                    j = 0;
                    foreach (var word in words)
                    {
                        if (!decimal.TryParse(word, out matrix[i, j]))
                        {
                            return "Некорректный формат файла: Некоторые значения в файле не являются вещественными числами.";
                        }

                        j++;
                    }

                    i++;
                }
                // Кладем матрицу в список матриц, присвоив ей имя, равное имени файла.
                Matrix.Storage.Add(Path.GetFileNameWithoutExtension(path), new Matrix(matrix));
                return $"Матрица {Path.GetFileNameWithoutExtension(path)} успешно создана!";
            }
            catch (IOException)
            {
                return "Ошибка чтения файла.";
            }
        }
        
        public string Run(string[] args)
        {
            if (args.Length < 2)
            {
                return "Использование: load <path_to_file>";
            }

            try
            {
                var path = Path.GetFullPath(args[1]);
                if (!File.Exists(path))
                {
                    return "Этого файла не существует.";
                }

                return ParseFile(path);
            }
            catch (ArgumentException)
            {
                return "Недопустимый формат пути к файлу.";
            }
            catch (PathTooLongException)
            {
                return "Указанный путь слишком длинный.";
            }
        }
    }
}