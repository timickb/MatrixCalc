using System;
using System.Linq;

namespace MatrixCalc.Linalg
{
    public class Kramer
    {
        private Matrix matrix;
        private decimal[] b;
        private decimal det;
        
        /// <summary>
        /// Принимает квадратную матрицу и
        /// вектор b - столбец свободных членов.
        /// </summary>
        /// <param name="matrix">матрица</param>
        /// <param name="b">вектор свободных членов</param>
        /// <exception cref="InvalidMatrixSizeException">Исключение выбрасывается, когда
        /// матрица не является квадратной или длина вектора b не равняется размеру матрицы.</exception>
        /// <exception cref="CellValueException">Исключение выбрасывается, когда
        /// одно из значений вектора b по модулю превосходит ограничение.</exception>
        /// <exception cref="KramerException">Исключение выбрасывается, когда
        /// полученная система уравнений не имеет решений.</exception>
        public Kramer(Matrix matrix, decimal[] b)
        {
            // Длина вектора b должна быть равна количеству строк матрицы, матрица должна быть квадратной.
            if (b.Length != matrix.RowsAmount || matrix.RowsAmount != matrix.ColsAmount)
            {
                throw new InvalidMatrixSizeException();
            }
            // Проверка на соответствие значений допустимому диапазону.
            if (b.Any(value => Math.Abs(value) > Matrix.MaxAbsValue))
            {
                throw new CellValueException();
            }
            // Определитель матрицы не должен быть нулем.
            if (matrix.Det.Equals(decimal.Zero))
            {
                throw new KramerException();
            }
            this.matrix = matrix;
            this.b = b;
            this.det = matrix.Det;
        }

        public decimal[] GetSolution()
        {
            var x0 = new decimal[matrix.RowsAmount];
            for (var i = 0; i < matrix.ColsAmount; i++)
            {
                x0[i] = GetMatrixWithBInColumn(i).Det / det;
            }

            return x0;
        }
        
        /// <summary>
        /// Возвращает матрицу, в которой
        /// k-й столбец заменен на вектор-столбец b.
        /// </summary>
        /// <param name="k">индекс столбца</param>
        /// <returns></returns>
        private Matrix GetMatrixWithBInColumn(int k)
        {
            var matrixRaw = new decimal[b.Length, b.Length];
            for (var i = 0; i < b.Length; i++)
            {
                for (var j = 0; j < b.Length; j++)
                {
                    if (j == k)
                    {
                        matrixRaw[i, j] = b[i];
                    }
                    else
                    {
                        matrixRaw[i, j] = matrix.GetValueAt(i, j);
                    }
                }
            }

            return new Matrix(matrixRaw);
        }
        
        
    }
}