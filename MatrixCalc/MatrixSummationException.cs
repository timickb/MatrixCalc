using System;

namespace MatrixCalc
{
    public class MatrixSummationException : Exception
    {
        public override string Message => "Matrices must have the same size.";
    }
}