using System;

namespace MatrixCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix.UpperRandomBound = 8;
            Matrix.LowerRandomBound = -4;
            Console.WriteLine("Matrix calculator");
            Matrix m1 = new Matrix(3, 3, MatrixType.RandomInt);
            Matrix m2 = new Matrix(3, 1, MatrixType.RandomInt);
            Matrix m3 = m1 * m2;
            m1.Display();
            m2.Display();
            Console.WriteLine();
            m3.Display();

        }
    }
}