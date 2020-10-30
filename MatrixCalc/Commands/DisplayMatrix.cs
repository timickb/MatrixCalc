using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class DisplayMatrix : ICommand
    {

        public DisplayMatrix(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (args.Length < 2)
            {
                return "Использование: display <matrix_name>";
            }

            var name = args[1];
            if (!Matrix.Matrices.ContainsKey(name))
            {
                return $"Матрицы с именем {name} не существует.";
            }
            Matrix.Matrices[name].Display();
            return String.Empty;
        }
    }
}