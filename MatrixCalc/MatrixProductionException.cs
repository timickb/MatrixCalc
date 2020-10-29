using System;

namespace MatrixCalc
{
    public class MatrixProductionException  : Exception
    {
        public override string Message =>
            "Amount of columns in first matrix must be equal to amount of rows in second matrix.";
    }
}