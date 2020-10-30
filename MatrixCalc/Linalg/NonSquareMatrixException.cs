using System;

namespace MatrixCalc.Linalg
{
    public class NonSquareMatrixException : Exception
    {
        public override string Message => "Can't perform this operation with non-square matrix.";
    }
}