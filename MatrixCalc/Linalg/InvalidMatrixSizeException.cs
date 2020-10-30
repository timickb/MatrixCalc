using System;

namespace MatrixCalc.Linalg
{
    public class InvalidMatrixSizeException : Exception
    {
        public override string Message => "Amount of rows and columns must be a positive integer.";
    }
}