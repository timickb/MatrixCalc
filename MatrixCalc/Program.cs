using System;

namespace MatrixCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix.UpperRandomBound = 10;
            Matrix.LowerRandomBound = 1;
            Console.WriteLine("Matrix calculator");
            var m1 = new Matrix(4, 4, MatrixType.RandomInt);
            m1.Display();
            m1.DisplayTriangulated();
            Console.WriteLine(m1.Det);


        }
    }
}