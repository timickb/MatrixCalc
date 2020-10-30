using System;
using MatrixCalc.Linalg;

namespace MatrixCalc.Commands
{
    public class MatrixList : ICommand
    {
        public MatrixList(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Run(string[] args)
        {
            if (Matrix.Matrices.Count == 0)
            {
                return "На данный момент матрицы в системе отсутствуют. Вы можете создать матрицу " +
                       "командой create или загрузить ее из файла командой load.";
            }
            Console.WriteLine($"На данный момент в системе существует {Matrix.Matrices.Count} матриц:");
            // Просто выводим список матриц в формате name [m x n].
            foreach (var m in Matrix.Matrices)
            {
                Console.WriteLine($"-> {m.Key} [{m.Value.RowsAmount} x {m.Value.ColsAmount}]");
            }
            return String.Empty;
        }
    }
}