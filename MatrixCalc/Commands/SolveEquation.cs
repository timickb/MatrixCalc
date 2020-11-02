using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class SolveEquation: ICommand
    
    {
        public SolveEquation(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 2)
            {
                return "Использование: solve <matrix_name>";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }
            Console.WriteLine("Введите вектор свободных коэффицентов в строку через пробел:");
            var userInput = Console.ReadLine().Split();
            if (userInput.Length != Matrix.Storage[args[1]].RowsAmount)
            {
                return "Количество чисел в векторе должно быть равно количеству строк в матрице.";
            }

            var b = new decimal[userInput.Length];
            for (var i = 0; i < userInput.Length; i++)
            {
                if (!decimal.TryParse(Utils.PrepareDecimal(userInput[i]), out b[i]))
                {
                    return "Все элементы вектора должны быть вещественными числами.";
                }
            }

            try
            {
                var kramer = new Kramer(Matrix.Storage[args[1]], b);
                var x0 = kramer.GetSolution();
                return $"Решение системы: {Environment.NewLine} {string.Join(", ", x0)}";
                
            }
            catch (KramerException)
            {
                return "Система не имеет решений.";
            }
            catch (InvalidMatrixSizeException)
            {
                return "Матрица не является квадратной.";
            }
            catch (CellValueException)
            {
                return $"Одно из значений вектора свободных коэффициентов превышает по модулю {Matrix.MaxAbsValue}";
            }
        }
    }
}