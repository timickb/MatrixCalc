using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class CreateMatrix : ICommand
    {
        public string Name { get; set; }

        public CreateMatrix(string name)
        {
            Name = name;
        }

        // "Мастер создания матрицы".
        private string CreationMaster(int m, int n, string name)
        {
            string userInput;
            while (true)
            {
                Console.Write("Введите номер выбранной опции: ");
                userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "0":
                        return String.Empty;
                    case "1":
                        try
                        {
                            Matrix.Matrices.Add(name, new Matrix(m, n, MatrixType.Eye));
                            return $"Матрица {name} успешно создана!";
                        }
                        catch (NonSquareMatrixException)
                        {
                            Console.WriteLine("Матрица должна быть квадратной. Попробуйте другую опцию, " +
                                              "либо выйдите и введите команду заново с корректными данными.");
                            continue;
                        } 
                    case "2":
                        Matrix.Matrices.Add(name, new Matrix(m, n, MatrixType.Zeros));
                        return $"Матрица {name} успешно создана!";
                    case "3":
                        Console.Write("Введите вещественное или целое число: ");
                        var inputNumber = Console.ReadLine();
                        decimal value;
                        if (!decimal.TryParse(inputNumber, out value))
                        {
                            Console.WriteLine("Это не число. Попробуйте еще раз.");
                            continue;
                        }
                        Matrix.Matrices.Add(name, new Matrix(m, n, value));
                        return $"Матрица {name} успешно создана!";
                    case "4":
                        Matrix.Matrices.Add(name, new Matrix(m, n, MatrixType.RandomInt));
                        return $"Матрица {name} успешно создана!";
                    case "5":
                        Matrix.Matrices.Add(name, new Matrix(m, n, MatrixType.Random));
                        return $"Матрица {name} успешно создана!";
                    case "6":
                        return RequestMatrix(m, n, name);
                }
            }
        }

        private string RequestMatrix(int m, int n, string name)
        {
            var matrix = new decimal[m, n];
            Console.WriteLine($"Введите {m} строк, в каждой из них - по {n} вещественных чисел через пробел:");
            for (var i = 0; i < m; i++)
            {
                // Считаем очередную строку.
                var line = Console.ReadLine()?.Split(" ");
                // Проверим ее размер.
                if (line?.Length != n)
                {
                    return "Не удалось создать матрицу: неверное количество чисел в строке.";
                }
                // Проверим, что каждая сущность ней - вещественное число.
                for (var j = 0; j < n; j++)
                {
                    if (!decimal.TryParse(line[j], out matrix[i, j]))
                    {
                        return "Не удалось создать матрицу: одно из значений не является вещественным числом.";
                    }
                }
            }
            // Если все успешно - создаем матрицу.
            Matrix.Matrices.Add(name, new Matrix(matrix));
            return $"Матрица {name} успешно создана!";
        }

        public string Run(string[] args)
        {
            if (args.Length < 3)
            {
                return "Использование: create <rows_amount> <cols_amount> [matrix_name]";
            }

            // Размеры матрицы.
            int m, n;

            if (!int.TryParse(args[1], out m) || !int.TryParse(args[2], out n) || m <= 0 || n <= 0)
            {
                return "Размеры матрицы должны быть целыми положительными числами!";
            }

            // Имя матрицы.
            var name = String.Empty;
            if (args.Length == 4)
            {
                if (!Utils.IsMatrixNameCorrect(args[3]))
                {
                    return
                        "Заданное имя матрицы некорректно. Оно не должно начинаться с цифры и не должно " +
                        "содержать никаких символов помимо цифр и латинких букв.";
                }

                if (Matrix.Matrices.ContainsKey(args[3]))
                {
                    return $"Матрица с именем {args[3]} уже существует. Попробуйте другие имя.";
                }

                name = args[3];
            }
            else
            {
                // Если пользователь не указал имя, задаем имя по умолчанию.
                name = "matrix" + Convert.ToString(Matrix.Matrices.Count + 1);
            }
            Console.WriteLine("Выберите опцию для создания матрицы: ");
            Console.WriteLine("1). Единичная матрица");
            Console.WriteLine("2). Нулевая матрица");
            Console.WriteLine("3). Матрица, заполненная одним определенным числом.");
            Console.WriteLine("4). Матрица из случайных целых чисел");
            Console.WriteLine("5). Матрица из случайных вещественных чисел");
            Console.WriteLine("6). Ввести матрицу вручную");
            Console.WriteLine("0). Я передумал(а), отстаньте.");
            return CreationMaster(m, n, name);
        }
    }
}