using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace MatrixCalc.Linalg
{
    public class Matrix
    {
        /// <summary>
        /// Хранилище всех матриц, созданных пользователем.
        /// Ключ в этом словаре - это имя матрицы, которое задал пользователь.
        /// </summary>
        public static Dictionary<string, Matrix> Storage = new Dictionary<string, Matrix>();
        
        /// <summary>
        /// Максимально допустимое число строк/столбцов в матрице.
        /// </summary>
        public static int MaxDimensionSize { get; set; } = 20;

        /// <summary>
        /// Максимальное допустимое по модулю число, которое может находиться в ячейке матрицы.
        /// </summary>
        public static int MaxAbsValue { get; set; } = 99999;

        /// <summary>
        /// Нижняя граница для генератора рандомных чисел.
        /// По умолчанию -100.
        /// </summary>
        public static int LowerRandomBound { get; set; } = -100;

        /// <summary>
        /// Верхняя граница для генератора рандомных чисел.
        /// По умолчанию 100
        /// (сама граница в диапазон не включается).
        /// </summary>
        public static int UpperRandomBound { get; set; } = 100;


        // Этот двумерный массив - сама матрица.
        private decimal[,] _values;

        // В этом двумерном массиве лежит матрица в верхнетреугольном виде.
        private decimal[,] _triang;

        public int RowsAmount => _values.GetLength(0);
        public int ColsAmount => _values.GetLength(1);
        public int Rank { get; private set; }

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
        private void FillWithValue(decimal value)
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
                    _values[i, j] =
                        Convert.ToDecimal(r.NextDouble() * (UpperRandomBound - LowerRandomBound) + LowerRandomBound);
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
                    _values[i, j] = r.Next(LowerRandomBound, UpperRandomBound);
                }
            }
        }

        /// <summary>
        /// Создает матрицу из данного двумерного массива
        /// </summary>
        /// <param name="arr">произвольный двумерный массив</param>
        /// <exception cref="InvalidMatrixSizeException">Исключение выбрасывается, когда
        /// число строк или число столбцов матрицы не является положительным числом.</exception>
        public Matrix(decimal[,] arr)
        {
            var m = arr.GetLength(0);
            var n = arr.GetLength(1);
            if (m <= 0 || n <= 0)
            {
                throw new InvalidMatrixSizeException();
            }

            _values = new decimal[m, n];
            for (var i = 0; i < m; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (Math.Abs(arr[i, j]) > MaxAbsValue)
                    {
                        throw new CellValueException();
                    }
                    _values[i, j] = arr[i, j];
                }
            }

            CreateTriangulated();
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
            if (m <= 0 || n <= 0 || m > MaxDimensionSize || n > MaxDimensionSize)
            {
                throw new InvalidMatrixSizeException();
            }

            if (type == MatrixType.Eye && m != n)
            {
                throw new NonSquareMatrixException();
            }

            _values = new decimal[m, n];

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

            CreateTriangulated();
        }

        /// <summary>
        /// Создает матрицу, в каждую ячейку которой кладет число value.
        /// </summary>
        /// <param name="m">количество строк</param>
        /// <param name="n">количество столбцов</param>
        /// <param name="value">значение в каждой ячейке</param>
        /// <exception cref="InvalidMatrixSizeException">Исключение выбрасывается, когда
        /// число строк или число столбцов матрицы не является положительным числом.</exception>
        public Matrix(int m, int n, decimal value)
        {
            if (m <= 0 || n <= 0 || m > MaxDimensionSize || n > MaxDimensionSize)
            {
                throw new InvalidMatrixSizeException();
            }

            if (Math.Abs(value) > MaxAbsValue)
            {
                throw new CellValueException();
            }

            _values = new decimal[m, n];
            FillWithValue(value);
            CreateTriangulated();
        }

        /// <summary>
        /// Красивенько печатает матрицу в консоль.
        /// </summary>
        public void Display()
        {
            Console.WriteLine();
            for (var i = 0; i < RowsAmount; i++)
            {
                Console.Write("| ");
                for (var j = 0; j < ColsAmount; j++)
                {
                    var val = Math.Round(decimal.ToDouble(_values[i, j]), 2);
                    Console.Write("{0,10}", val);
                }

                Console.Write(" |");
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Красивенько печатает верхнетреугольную матрицу в консоль.
        /// </summary>
        public void DisplayTriangulated()
        {
            Console.WriteLine();
            for (var i = 0; i < RowsAmount; i++)
            {
                Console.Write("| ");
                for (var j = 0; j < ColsAmount; j++)
                {
                    var val = Math.Round(decimal.ToDouble(_triang[i, j]), 3);
                    Console.Write("{0,10}", val);
                }

                Console.Write(" |");
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Изменить значение в ячейке с индексом (i, j).
        /// </summary>
        /// <param name="i">номер строки</param>
        /// <param name="j">номер столбца</param>
        /// <param name="value">новое значение</param>
        /// <exception cref="IndexOutOfRangeException">Исключение выбрасывается, когда
        /// в матрице не существует элемента с индексом (i, j).</exception>
        private void SetValueAt(int i, int j, decimal value)
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
        public decimal GetValueAt(int i, int j)
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
        public decimal Trace
        {
            get
            {
                if (RowsAmount != ColsAmount)
                {
                    throw new NonSquareMatrixException();
                }

                decimal trace = 0;
                for (var i = 0; i < RowsAmount; i++)
                {
                    trace += _values[i, i];
                }

                return trace;
            }
        }
        
        /// <summary>
        /// Решает СЛАУ AX = b, где
        /// A - данная матрица, b - вектор
        /// коэффициентов, передающийся в
        /// качестве аргумента.
        /// </summary>
        /// <param name="b">Массив коэффициентов. Его длина должна
        /// равняться количеству столбцов в данной матрице.</param>
        /// <returns>Вектор X0 - решение СЛАУ.</returns>
        public decimal[] SolveEquationSystem(decimal[] b)
        {
            if (b.Length != ColsAmount)
            {
                throw new InvalidMatrixSizeException();
            }

            var x0 = new decimal[ColsAmount];
            try
            {
                // Идем по строкам верхнетреугольной матрицы с конца.
                for (var i = RowsAmount - 1; i >= 0; i--)
                {
                    // Возьмем первый ненулевой элемент
                    for (var j = 0; j < ColsAmount; j++)
                    {
                        if (_triang[i, j] != Decimal.Zero)
                        {
                            var tail = Decimal.Zero;
                            for (var k = j + 1; k < ColsAmount; k++)
                            {
                                tail += b[k] * _triang[i, k];
                            }

                            x0[j] = ((b[j] - tail) / _triang[i, j]);
                        }

                    }
                }
            }
            catch (OverflowException)
            {
                throw new OverflowException();
            }

            return x0;
        }

        /// <summary>
        /// Возвращает транспонированную по отношению к данной матрицу.
        /// </summary>
        /// <returns>транспонированная матрица</returns>
        public Matrix TransposedMatrix
        {
            get
            {
                var m = ColsAmount;
                var n = RowsAmount;
                var transposed = new Matrix(m, n, MatrixType.Zeros);
                for (var i = 0; i < m; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        transposed.SetValueAt(i, j, _values[j, i]);
                    }
                }

                return transposed;
            }
        }

        /// <summary>
        /// Операция сложения двух матриц.
        /// </summary>
        /// <param name="m1">первая матрица</param>
        /// <param name="m2">вторая матрица</param>
        /// <returns>Матрица, являющаяся суммой m1 и m2.</returns>
        /// <exception cref="MatrixSummationException">Исключение выбрасывается, когда
        /// размеры матриц m1 и m2 не совпадают.</exception>
        /// <exception cref="CellValueException">Исключение выбрасывается, когда
        /// в результате сложения в какой-то ячейке оказывается число, по модулю
        /// превышающее MaxAbsValue.</exception>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.ColsAmount != m2.ColsAmount || m1.RowsAmount != m2.RowsAmount)
            {
                throw new MatrixSummationException();
            }

            var sum = new Matrix(m1.RowsAmount, m1.ColsAmount, MatrixType.Zeros);
            for (var i = 0; i < m1.RowsAmount; i++)
            {
                for (var j = 0; j < m1.ColsAmount; j++)
                {
                    var element = m1.GetValueAt(i, j) + m2.GetValueAt(i, j);
                    if (Math.Abs(element) > MaxAbsValue)
                    {
                        throw new CellValueException();
                    }
                    sum.SetValueAt(i, j, element);
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
        /// <exception cref="CellValueException">Исключение выбрасывается, когда
        /// в результате сложения в какой-то ячейке оказывается число, по модулю
        /// превышающее MaxAbsValue.</exception>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.ColsAmount != m2.ColsAmount || m1.RowsAmount != m2.RowsAmount)
            {
                throw new MatrixSummationException();
            }

            var sum = new Matrix(m1.RowsAmount, m1.ColsAmount, MatrixType.Zeros);
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
        /// <exception cref="CellValueException">Исключение выбрасывается, когда
        /// в результате сложения в какой-то ячейке оказывается число, по модулю
        /// превышающее MaxAbsValue.</exception>
        /// <returns>Матрица n, где n(i, j) = c * m(i, j)</returns>
        public static Matrix operator *(Matrix m, decimal c)
        {
            var result = new Matrix(m.RowsAmount, m.ColsAmount, MatrixType.Zeros);
            for (var i = 0; i < m.RowsAmount; i++)
            {
                for (var j = 0; j < m.ColsAmount; j++)
                {
                    if (Math.Abs(m.GetValueAt(i, j) * c) > MaxAbsValue)
                    {
                        throw new CellValueException();
                    }
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
        /// <exception cref="CellValueException">Исключение выбрасывается, когда
        /// в результате сложения в какой-то ячейке оказывается число, по модулю
        /// превышающее MaxAbsValue.</exception>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.ColsAmount != m2.RowsAmount)
            {
                throw new MatrixProductionException();
            }

            var prod = new Matrix(m1.RowsAmount, m2.ColsAmount, MatrixType.Zeros);
            var n = m1.ColsAmount;
            for (var i = 0; i < prod.RowsAmount; i++)
            {
                for (var j = 0; j < prod.ColsAmount; j++)
                {
                    decimal sum = 0;
                    for (var k = 0; k < n; k++)
                    {
                        sum += m1.GetValueAt(i, k) * m2.GetValueAt(k, j);
                    }

                    if (Math.Abs(sum) > MaxAbsValue)
                    {
                        throw new CellValueException();
                    }
                    prod.SetValueAt(i, j, sum);
                }
            }

            return prod;
        }

        // Меняет местами две строки в верхнетреугольной матрице.
        private void SwapRows(int i, int j)
        {
            if (i < 0 || j < 0 || i >= RowsAmount || j >= RowsAmount)
            {
                return;
            }

            var tmpRow = new decimal[ColsAmount];
            // Возьмем i-ю строку и запишем ее в tmpRow.
            for (var k = 0; k < ColsAmount; k++)
            {
                tmpRow[k] = _triang[i, k];
            }

            // На место i-й строки положим j-ю строку.
            for (var k = 0; k < ColsAmount; k++)
            {
                _triang[i, k] = _triang[j, k];
            }

            // На место j-й строки положим tmpRow.
            for (var k = 0; k < ColsAmount; k++)
            {
                _triang[j, k] = tmpRow[k];
            }
        }

        // Умножает строку i на число c в верхнетреугольной матрице.
        private void MultiplyRow(int i, decimal c)
        {
            if (i < 0 || i >= RowsAmount)
            {
                return;
            }

            for (int k = 0; k < ColsAmount; k++)
            {
                _triang[i, k] *= c;
            }
        }

        // Прибавляет к строке i строку j (в верхнетреугольной матрице).
        private void SumRows(int i, int j)
        {
            if (i < 0 || j < 0 || i >= RowsAmount || j >= RowsAmount)
            {
                return;
            }

            for (var k = 0; k < ColsAmount; k++)
            {
                _triang[i, k] += _triang[j, k];
            }
        }

        // Прибавляет к строке i строку j, умноженную на число c (в верхнетреугольной матрице).
        private void SumRows(int i, int j, decimal c)
        {
            if (i < 0 || j < 0 || i >= RowsAmount || j >= RowsAmount)
            {
                return;
            }

            for (var k = 0; k < ColsAmount; k++)
            {
                _triang[i, k] += _triang[j, k] * c;
            }
        }

        /// <summary>
        /// Приводит матрицу к верхнетреугольному (ступенчатому) виду методом Гаусса.
        /// Предполагается, что данный метод будет вызываться только при создании
        /// матрицы, т.е. только в конструкторе, для того, чтобы сразу
        /// инициализировать массив _triang.
        /// </summary>
        private void CreateTriangulated()
        {
            _triang = new decimal[RowsAmount, ColsAmount];
            // Скопируем все значения из исходной матрицы _values в _triang.
            for (var i0 = 0; i0 < RowsAmount; i0++)
            {
                for (var j0 = 0; j0 < ColsAmount; j0++)
                {
                    _triang[i0, j0] = _values[i0, j0];
                }
            }

            var i = 0;
            var j = 0;
            // Будем проходить по всем столбцам и строкам.
            while (i < RowsAmount && j < ColsAmount)
            {
                // Найдем максимальный по модулю элемент в j-м столбце.
                var maxElement = Decimal.Zero;
                // Индекс строки, в которой лежит этот элемент.
                var rowIndex = 0;
                for (var k = i; k < RowsAmount; k++)
                {
                    if (Math.Abs(_triang[k, j]) > maxElement)
                    {
                        rowIndex = k;
                        maxElement = Math.Abs(_triang[k, j]);
                    }
                }

                if (maxElement == Decimal.Zero)
                {
                    /* На всякий случай обнулим остальные элементы в столбце.
                     (вдруг там что-то ненулевое осталось?...) */
                    for (var k = i; k < RowsAmount; k++)
                    {
                        _triang[k, j] = Math.Abs(decimal.Zero);
                    }

                    j++;
                    continue;
                }

                // Свопнем найденную строку с i-й, если они - не одна и та же строка.
                if (rowIndex != i)
                {
                    SwapRows(rowIndex, i);
                    // Поменяем знак одной из них, чтобы компенсировать изменение знака определителя.
                    MultiplyRow(i, -1);
                }

                for (var k = i + 1; k < RowsAmount; k++)
                {
                    // Добавим к строке k строку i, умноженную на c.
                    var c = -(_triang[k, j]) / (_triang[i, j]);
                    SumRows(k, i, c);
                }

                i++;
                j++;
            }
            // Посчитаем ранг матрицы.
            Rank = RowsAmount;
            for (var i1 = 0; i1 < RowsAmount; i1++)
            {
                bool zero = true;
                for (var j1 = 0; j1 < ColsAmount; j1++)
                {
                    if (_triang[i1, j1] != 0)
                    {
                        zero = false;
                        break;
                    }
                }

                if (zero)
                {
                    Rank--;
                }
            }
        }

        /// <summary>
        /// Вычисляет произведение элементов на диагонали в верхнетреугольной матрице.
        /// </summary>
        /// <exception cref="NonSquareMatrixException">Исключение выбрасывается, когда
        /// данная матрица не является квадратной.</exception>
        /// <exception cref="OverflowException">Исключение выбрасывается, когда
        /// произведение на диагонали не вмещается в тип decimal.</exception>
        private decimal MainDiagonalProduction
        {
            get
            {
                if (ColsAmount != RowsAmount)
                {
                    throw new NonSquareMatrixException();
                }

                var prod = Decimal.One;
                try
                {
                    for (var i = 0; i < ColsAmount; i++)
                    {
                        prod *= _triang[i, i];
                    }
                }
                catch (OverflowException)
                {
                    throw new OverflowException();
                }

                return prod;
            }
        }
        
        /// <summary>
        /// Считает произведение элементов на диагонали,
        /// предварительно округлив каждый из них
        /// до целого числа.
        /// </summary>
        /// <exception cref="NonSquareMatrixException">Исключение выбрасывается, когда
        /// данная матрица не является квадратной.</exception>
        private long IntMainDiagonalProduction
        {
            get
            {
                if (ColsAmount != RowsAmount)
                {
                    throw new NonSquareMatrixException();
                }

                var prod = 1L;
                try
                {
                    for (var i = 0; i < ColsAmount; i++)
                    {
                        prod *= Convert.ToInt64(Math.Round(_triang[i, i]));
                    }
                }
                catch (OverflowException)
                {
                    throw new OverflowException();
                }

                return prod;
            }
        }


        /// <summary>
        /// Возвращает определитель данной матрицы.
        /// Определители 1 и 2 порядка считаются с помощью
        /// мнемонического правила, 3-го и выше - приведением
        /// к верхнетреугольному виду и перемножением
        /// элементов на главной диагонали.
        /// </summary>
        /// <exception cref="NonSquareMatrixException">Исключение выбрасывается, когда
        /// данная матрциа не является квадратной.</exception>
        public decimal Det
        {
            get
            {
                if (ColsAmount != RowsAmount)
                {
                    throw new NonSquareMatrixException();
                }

                try
                {
                    return ColsAmount switch
                    {
                        1 => _values[0, 0],
                        2 => _values[0, 0] * _values[1, 1] - _values[0, 1] * _values[1, 0],
                        _ => MainDiagonalProduction
                    };
                }
                catch (OverflowException)
                {
                    return IntMainDiagonalProduction;
                }
            }
        }
    }
}