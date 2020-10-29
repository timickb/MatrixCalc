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
            if (RowsAmount != ColsAmount)
            {
                return;
            }

            for (var i = 0; i < RowsAmount; i++)
            {
                for (var j = 0; j < ColsAmount; j++)
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

        // Кладет во все ячейки матрицы число value.
        private void FillWithValue(double value)
        {
            for (var i = 0; i < RowsAmount; i++)
            {
                for (var j = 0; j < ColsAmount; j++)
                {
                    _values[i, j] = value;
                }
            }
        }

        private void FillWithRandomValues()
        {
            var r = new Random();
            for (var i = 0; i < RowsAmount; i++)
            {
                for (var j = 0; j < ColsAmount; j++)
                {
                    _values[i, j] = r.NextDouble() * (UpperRandomBound - LowerRandomBound) + LowerRandomBound;
                }
            }
        }

        private void FillWithRandomIntValues()
        {
            var r = new Random();
            for (var i = 0; i < RowsAmount; i++)
            {
                for (var j = 0; j < ColsAmount; j++)
                {
                    _values[i, j] = r.Next((int) LowerRandomBound, (int) UpperRandomBound);
                }
            }
        }

        /// <summary>
        /// Создает нулевую матрицу.
        /// </summary>
        /// <param name="m">количество строк</param>
        /// <param name="n">количество столбцов</param>
        /// <exception cref="InvalidMatrixSizeException">Исключение выбрасывается, когда
        /// число строк или число столбцов матрицы не является положительным числом.</exception>
        public Matrix(int m, int n)
        {
            if (m <= 0 || n <= 0)
            {
                throw new InvalidMatrixSizeException();
            }

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
        /// <exception cref="InvalidMatrixSizeException">Исключение выбрасывается, когда
        /// число строк или число столбцов матрицы не является положительным числом.</exception>
        /// <exception cref="NonSquareMatrixException">Исключение выбрасывается, когда матрица
        /// не является квадратной, а запрашиваемый тип требует, чтобы она ей являлась.</exception>
        public Matrix(int m, int n, MatrixType type)
        {
            if (m <= 0 || n <= 0)
            {
                throw new InvalidMatrixSizeException();
            }

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
                case MatrixType.RandomInt:
                    FillWithRandomIntValues();
                    break;
                default:
                    FillWithValue(0);
                    break;
            }
        }

        /// <summary>
        /// Создает матрицу, в каждую ячейку которой кладет число value.
        /// </summary>
        /// <param name="m">количество строк</param>
        /// <param name="n">количество столбцов</param>
        /// <param name="value">значение в каждой ячейке</param>
        /// <exception cref="InvalidMatrixSizeException">Исключение выбрасывается, когда
        /// число строк или число столбцов матрицы не является положительным числом.</exception>
        public Matrix(int m, int n, int value)
        {
            if (m <= 0 || n <= 0)
            {
                throw new InvalidMatrixSizeException();
            }

            _values = new double[m, n];
            FillWithValue(value);
        }

        /// <summary>
        /// Красивенько печатает матрицу в консоль.
        /// </summary>
        public void Display()
        {
            for (var i = 0; i < RowsAmount; i++)
            {
                for (var j = 0; j < ColsAmount; j++)
                {
                    Console.Write(_values[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Изменить значение в ячейке с индексом (i, j).
        /// </summary>
        /// <param name="i">номер строки</param>
        /// <param name="j">номер столбца</param>
        /// <param name="value">новое значение</param>
        /// <exception cref="IndexOutOfRangeException">Исключение выбрасывается, когда
        /// в матрице не существует элемента с индексом (i, j).</exception>
        public void SetValueAt(int i, int j, double value)
        {
            try
            {
                _values[i, j] = value;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Получить значение из ячейки с индексом (i, j).
        /// </summary>
        /// <param name="i">номер строки</param>
        /// <param name="j">номер столбца</param>
        /// <returns>значение элемента с индексом (i, j)</returns>
        /// <exception cref="IndexOutOfRangeException">Исключение выбрасывается, когда
        /// в матрице не существует элемента с индексом (i, j).</exception>
        public double GetValueAt(int i, int j)
        {
            try
            {
                return _values[i, j];
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Возвращает вещественное число - след матрицы.
        /// </summary>
        /// <exception cref="NonSquareMatrixException">Исключение выбрасывается,
        /// когда матрица не является квадратной.</exception>
        public double Trace
        {
            get
            {
                if (RowsAmount != ColsAmount)
                {
                    throw new NonSquareMatrixException();
                }

                double trace = 0;
                for (var i = 0; i < RowsAmount; i++)
                {
                    trace += _values[i, i];
                }

                return trace;
            }
        }

        /// <summary>
        /// Возвращает транспонированную по отношению к данной матрицу.
        /// </summary>
        /// <returns>транспонированная матрица</returns>
        public Matrix GetTransposedMatrix()
        {
            var m = ColsAmount;
            var n = RowsAmount;
            var transposed = new Matrix(m, n);
            for (var i = 0; i < m; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    transposed.SetValueAt(i, j, _values[j, i]);
                }
            }

            return transposed;
        }

        /// <summary>
        /// Операция сложения двух матриц.
        /// </summary>
        /// <param name="m1">первая матрица</param>
        /// <param name="m2">вторая матрица</param>
        /// <returns>Матрица, являющаяся суммой m1 и m2.</returns>
        /// <exception cref="MatrixSummationException">Исключение выбрасывается, когда
        /// размеры матриц m1 и m2 не совпадают.</exception>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.ColsAmount != m2.ColsAmount || m1.RowsAmount != m2.RowsAmount)
            {
                throw new MatrixSummationException();
            }

            var sum = new Matrix(m1.RowsAmount, m1.ColsAmount);
            for (var i = 0; i < m1.RowsAmount; i++)
            {
                for (var j = 0; j < m1.ColsAmount; j++)
                {
                    sum.SetValueAt(i, j, m1.GetValueAt(i, j) + m2.GetValueAt(i, j));
                }
            }

            return sum;
        }

        /// <summary>
        /// Операция вычитания двух матриц.
        /// </summary>
        /// <param name="m1">первая матрица</param>
        /// <param name="m2">вторая матрица</param>
        /// <returns>Матрица, являющаяся разностью m1 и m2.</returns>
        /// <exception cref="MatrixSummationException">Исключение выбрасывается, когда
        /// размеры матриц m1 и m2 не совпадают.</exception>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.ColsAmount != m2.ColsAmount || m1.RowsAmount != m2.RowsAmount)
            {
                throw new MatrixSummationException();
            }

            var sum = new Matrix(m1.RowsAmount, m1.ColsAmount);
            for (var i = 0; i < m1.RowsAmount; i++)
            {
                for (var j = 0; j < m1.ColsAmount; j++)
                {
                    sum.SetValueAt(i, j, m1.GetValueAt(i, j) - m2.GetValueAt(i, j));
                }
            }

            return sum;
        }

        /// <summary>
        /// Операция умножение матрицы на число.
        /// </summary>
        /// <param name="m">произвольная матрица</param>
        /// <param name="c">вещественное число</param>
        /// <returns>Матрица n, где n(i, j) = c * m(i, j)</returns>
        public static Matrix operator *(Matrix m, double c)
        {
            var result = new Matrix(m.RowsAmount, m.ColsAmount);
            for (var i = 0; i < m.RowsAmount; i++)
            {
                for (var j = 0; j < m.ColsAmount; j++)
                {
                    result.SetValueAt(i, j, m.GetValueAt(i, j) * c);
                }
            }

            return result;
        }

        /// <summary>
        /// Операция умножения двух матриц.
        /// </summary>
        /// <param name="m1">первая матрица</param>
        /// <param name="m2">вторая матрица</param>
        /// <returns>Матрица размера m1.RowsAmount, m2.ColsAmount</returns>
        /// <exception cref="MatrixProductionException">Исключение выбрасывается, когда
        /// количество столбцов матрицы m1 не совпадает с количеством строк матрицы m2.</exception>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.ColsAmount != m2.RowsAmount)
            {
                throw new MatrixProductionException();
            }

            var prod = new Matrix(m1.RowsAmount, m2.ColsAmount);
            var n = m1.ColsAmount;
            for (var i = 0; i < prod.RowsAmount; i++)
            {
                for (var j = 0; j < prod.ColsAmount; j++)
                {
                    double sum = 0;
                    for (var k = 0; k < n; k++)
                    {
                        sum += m1.GetValueAt(i, k) * m2.GetValueAt(k, j);
                    }

                    prod.SetValueAt(i, j, sum);
                }
            }

            return prod;
        }
    }
}