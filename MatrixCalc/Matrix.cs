using System;

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

        // Приведена ли на данный момент матрица к ступенчатому (верхнетреугольному) виду.
        private bool _isTriangulated = false;

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

            _isTriangulated = false;
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

            _isTriangulated = false;
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

            _isTriangulated = false;
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

            _isTriangulated = false;
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
                _isTriangulated = false;
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
        public Matrix TransposedMatrix
        {
            get
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
                        try
                        {
                            sum += m1.GetValueAt(i, k) * m2.GetValueAt(k, j);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine($"i = {i}, j = {j}, k = {k}");
                        }
                    }

                    prod.SetValueAt(i, j, sum);
                }
            }

            return prod;
        }

        private void SwapRows(int i, int j)
        {
            if (i < 0 || j < 0 || i >= RowsAmount || j >= RowsAmount)
            {
                return;
            }

            var tmpRow = new double[ColsAmount];
            // Возьмем i-ю строку и запишем ее в tmpRow.
            for (var k = 0; k < ColsAmount; k++)
            {
                tmpRow[k] = _values[i, k];
            }

            // На место i-й строки положим j-ю строку.
            for (var k = 0; k < ColsAmount; k++)
            {
                _values[i, k] = _values[j, k];
            }

            // На место j-й строки положим tmpRow.
            for (var k = 0; k < ColsAmount; k++)
            {
                _values[j, k] = tmpRow[k];
            }
        }

        /// <summary>
        /// Умножает строку i на число c.
        /// </summary>
        /// <param name="i">индекс строки</param>
        /// <param name="c">вещественное число</param>
        private void MultiplyRow(int i, double c)
        {
            if (i < 0 || i >= RowsAmount)
            {
                return;
            }

            for (int k = 0; k < ColsAmount; k++)
            {
                _values[i, k] *= c;
            }
        }

        /// <summary>
        /// Вычисляет сумму строк i и j, результат кладет
        /// в строку i.
        /// </summary>
        /// <param name="i">индекс первой строки</param>
        /// <param name="j">индекс второй строки</param>
        private void SumRows(int i, int j)
        {
            if (i < 0 || j < 0 || i >= RowsAmount || j >= RowsAmount)
            {
                return;
            }

            for (var k = 0; k < ColsAmount; k++)
            {
                _values[i, k] += _values[j, k];
            }
        }

        /// <summary>
        /// Приводит матрицу к верхнетреугольному виду методом Гаусса.
        /// </summary>
        private void Triangulate()
        {


            _isTriangulated = true;
        }

        private double MainDiagonalProduction
        {
            get
            {
                if (!_isTriangulated)
                {
                    Triangulate();
                }

                if (ColsAmount != RowsAmount)
                {
                    throw new NonSquareMatrixException();
                }

                var prod = 1.0;
                for (var i = 0; i < ColsAmount; i++)
                {
                    prod *= _values[i, i];
                }

                return prod;
            }
        }


        /// <summary>
        /// Возвращает определитель данной матрицы.
        /// Определители 1, 2 и 3-го порядка считаются с помощью
        /// мнемонического правила, 4-го и выше - приведением
        /// к верхнетреугольному виду и перемножением
        /// элементов на главной диагонали.
        /// </summary>
        /// <exception cref="NonSquareMatrixException">Исключение выбрасывается, когда
        /// данная матрциа не является квадратной.</exception>
        public double Det
        {
            get
            {
                if (ColsAmount != RowsAmount)
                {
                    throw new NonSquareMatrixException();
                }
                
                return ColsAmount switch
                {
                    1 => _values[0, 0],
                    2 => _values[0, 0] * _values[1, 1] - _values[0, 1] * _values[1, 0],
                    3 => _values[0, 0] * _values[1, 1] * _values[2, 2] + _values[0, 1] * _values[1, 2] * _values[2, 0] +
                         _values[1, 0] * _values[2, 1] * _values[0, 2] - _values[0, 2] * _values[1, 1] * _values[2, 0] -
                         _values[1, 0] * _values[0, 1] * _values[2, 2] - _values[1, 2] * _values[2, 1] * _values[0, 0],
                    _ => MainDiagonalProduction
                };
            }
        }
    }
}