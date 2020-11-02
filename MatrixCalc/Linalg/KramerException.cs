using System;

namespace MatrixCalc.Linalg
{
    public class KramerException : Exception
    {
        public override string Message => "Данная система не имеет решений.";
    }
}