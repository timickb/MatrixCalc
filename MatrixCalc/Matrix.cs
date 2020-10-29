using System;
using System.Security.Cryptography;

namespace MatrixCalc
{
    public class Matrix
    {
        /// <summary>
        /// Нижняя граница для генератора рандомных чисел.
        /// По умолчанию -1000.
        /// </summary>
        public static double LowerRandomBound { get; set; } = -1000;

        /// <summary>
        /// Верхняя граница для генератора рандомных чисел.
        /// По умолчанию 1000
        /// (сама граница в диапазон не включается).
        /// </summary>
        public static double UpperRandomBound { get; set; } = 1000;


        // Этот двумерный массив - сама матрица.
        private readonly double[,] _values;

        public int RowsAmount => _values.GetLength(0);
        public int ColsAmount => _values.GetLength(1);

        /* Делает матрицу единичной. Если матрица не квадратная, ничего не делает.
         (обработка исключительной ситуации происходит в конструкторе). */
        private void FillAsEye()
        {
            if (RowsAmount == ColsAmount)
            {
                for (int i = 0; i < RowsAmount; i++)
                {
                    for (int j = 0; j < ColsAmount; j++)
                    {
                        if (i == j)
                        {
                            _values[i, j] = 1;
                        }
                        else
                        {
                            _values[i, j] = 0;
                        }
                    }
                }
            }
        }

        // Кладет во все ячейки матрицы число value
        private void FillWithValue(double value)
        {
            for (int i = 0; i < RowsAmount; i++)
            {
                for (int j = 0; j < ColsAmount; j++)
                {
                    _values[i, j] = value;
                }
            }
        }

        private void FillWithRandomValues()
        {
            Random r = new Random();
            for (int i = 0; i < RowsAmount; i++)
            {
                for (int j = 0; j < ColsAmount; j++)
                {
                    _values[i, j] = LowerRandomBound + r.NextDouble() * UpperRandomBound;
                }
            }
        }

        /// <summary>
        /// Создает нулевую матрицу
        /// </summary>
        /// <param name="m">количество строк</param>
        /// <param name="n">количество столбцов</param>
        public Matrix(int m, int n)
        {
            _values = new double[n, m];
            FillWithValue(0);
        }

        /// <summary>
        /// Создает матрицу определенного типа из перечисления MatrixType.
        /// Имейте в виду, что для использования некоторых типов
        /// матрица должна быть квадратной.
        /// </summary>
        /// <param name="m">количество строк</param>
        /// <param name="n">количество столбцов</param>
        /// <param name="type">тип матрицы</param>
        public Matrix(int m, int n, MatrixType type)
        {
            if (type == MatrixType.Eye && m != n)
            {
                throw new NonSquareMatrixException();
            }

            _values = new double[m, n];

            switch (type)
            {
                case MatrixType.Eye:
                    FillAsEye();
                    break;
                case MatrixType.Ones:
                    FillWithValue(1);
                    break;
                case MatrixType.Zeros:
                    FillWithValue(0);
                    break;
                case MatrixType.Random:
                    FillWithRandomValues();
                    break;
                default:
                    FillWithValue(0);
                    break;
            }
        }

        /// <summary>
        /// Создает матрицу, в каждую ячейку которой кладет число value
        /// </summary>
        /// <param name="m">количество строк</param>
        /// <param name="n">количество столбцов</param>
        /// <param name="value">значение в каждой ячейке</param>
        public Matrix(int m, int n, int value)
        {
            _values = new double[m, n];
            FillWithValue(value);
        }

        public void Display()
        {
            for (int i = 0; i < RowsAmount; i++)
            {
                for (int j = 0; j < ColsAmount; j++)
                {
                    Console.Write(_values[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
    }
}