using System;

namespace MatrixCalc.Linalg
{
    public class MatrixSummationException : Exception
    {
        public override string Message => "Storage must have the same size.";
    }
}