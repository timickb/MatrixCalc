using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class MultiplyMatrix : ICommand
    {
        public MultiplyMatrix(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string Run(string[] args)
        {
            if (args.Length < 3)
            {
                return "Использование: mul <matrix1_name> <matrix2_name>";
            }

            var name1 = args[1];
            var name2 = args[2];
            if (!Matrix.Storage.ContainsKey(name1))
            {
                return $"Матрицы {name1} не существует.";
            }

            if (!Matrix.Storage.ContainsKey(name2))
            {
                return $"Матрицы {name2} не существует.";
            }

            var m1 = Matrix.Storage[name1];
            var m2 = Matrix.Storage[name2];
            try
            {
                var result = m1 * m2;
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
                    return $"Матрицы перемножены, результатом является новая матрица {args[3]}";
                }

                result.Display();
                return string.Empty;
            }
            catch (MatrixProductionException)
            {
                return "Количество столбцов первой матрицы не совпадает с количеством строк второй.";
            }
            catch (CellValueException)
            {
                return "К сожалению, данная операция не может быть выполнена в силу того, что в ее" +
                       $" результате получится число, превышающее по модулю {Matrix.MaxAbsValue}";
            }
        }
    }
}