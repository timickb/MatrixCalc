using System.Diagnostics;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class TransposeMatrix : ICommand
    {
        public TransposeMatrix(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 2)
            {
                return "Использование: trans <matrix_name> [output_name]";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }

            var transposed = Matrix.Storage[args[1]].TransposedMatrix;
            // Если указано имя для матрицы-результата.
            if (args.Length >= 3)
            {
                if (!Utils.IsMatrixNameCorrect(args[2]))
                {
                    return "Имя для матрицы-результата некорректно.";
                }
                Matrix.Storage.Add(args[2], transposed);
                return $"Результат операции успешно записан в матрицу {args[2]}";
            }
            // Если нет.
            transposed.Display();
            return string.Empty;
        }
    }
}