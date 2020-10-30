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
            var m1 = new Matrix(3, 3, MatrixType.RandomInt);
            Console.WriteLine(m1.Det);


        }
    }
}