using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class MultiplyMatrixByNumber : ICommand
    {
        public MultiplyMatrixByNumber(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string Run(string[] args)
        {
            if (args.Length < 3)
            {
                return "Использование: mul <matrix1_name> <decimal_number> [output_name]";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }

            if (decimal.TryParse(args[2], out var number))
            {
                var result = Matrix.Storage[args[1]] * number;
                // Если указано имя для сохранения полученной матрицы - обработаем это.
                if (args.Length == 4)
                {
                    if (!Utils.IsMatrixNameCorrect(args[3]))
                    {
                        return "Имя для матрицы-результата некорректно.";
                    }

                    if (Matrix.Storage.ContainsKey(args[3]))
                    {
                        return $"Матрица с именем {args[3]} уже существует.";
                    }
                    // Если все ок - кладем в список матрицу result.
                    Matrix.Storage.Add(args[3], result);
                    return $"Матрица умножена на {number}, результатом является новая матрица {args[3]}";
                }
                result.Display();
                return string.Empty;
            }

            return "Второй аргумент должен быть вещественным числом.";
        }
    }
}