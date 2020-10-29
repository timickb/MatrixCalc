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
            Matrix m2 = new Matrix(3, 3, MatrixType.RandomInt);
            Matrix m3 = m1 + m2;
            m1.Display();
            Console.WriteLine("+");
            m2.Display();
            Console.WriteLine("=");
            m3.Display();
            Console.WriteLine();
            Console.WriteLine();
            Matrix m4 = m3.GetTransposedMatrix();
            m4.Display();
            m4 *= 2;
            Console.WriteLine();
            m4.Display();

        }
    }
}