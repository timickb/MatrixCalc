using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class DisplayTrace : ICommand
    {
        public DisplayTrace(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 2)
            {
                return "Использование: trace <matrix_name>";
            }

            if (!Matrix.Storage.ContainsKey(args[1]))
            {
                return $"Матрицы {args[1]} не существует.";
            }

            var m = Matrix.Storage[args[1]];
            try
            {
                return $"След данной матрицы равен {m.Trace}";
            }
            catch (NonSquareMatrixException)
            {
                return "Данная матрица не является квадратной.";
            }
        }
    }
}