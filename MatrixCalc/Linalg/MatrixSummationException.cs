using System;

namespace MatrixCalc.Linalg
{
    public class MatrixSummationException : Exception
    {
        public override string Message => "Matrices must have the same size.";
    }
}