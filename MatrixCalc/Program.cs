using System;

namespace MatrixCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Matrix calculator");
            Matrix m = new Matrix(3, 3, MatrixType.Random);
            m.Display();
            

        }
    }
}