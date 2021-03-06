using System;
using System.Collections.Generic;
using System.Linq;
using MatrixCalc.Commands;

namespace MatrixCalc
{
    public class CommandHandler
    {
        // Список инстансов доступных приложений (команд).
        private List<ICommand> commands;
        public CommandHandler()
        {
            commands = new List<ICommand>();
            // Инициализируем все доступные пользователю команды.
            commands.Add(new CreateMatrix("create"));
            commands.Add(new DisplayMatrix("display"));
            commands.Add(new MatrixList("list"));
            commands.Add(new DisplayDet("det"));
            commands.Add(new DisplayTrace("trace"));
            commands.Add(new MultiplyMatrix("mul"));
            commands.Add(new MultiplyMatrixByNumber("muln"));
            commands.Add(new SumMatrix("add"));
            commands.Add(new SubMatrix("sub"));
            commands.Add(new LoadFromFile("load"));
            commands.Add(new SaveToFile("save"));
            commands.Add(new DisplayRank("rank"));
            commands.Add(new Help("help"));
            commands.Add(new ChangeRandomBounds("setrnd"));
            commands.Add(new TransposeMatrix("trans"));
            commands.Add(new SolveEquation("solve"));
        }
        
        /// <summary>
        /// Запускает выполнение команды.
        /// </summary>
        /// <param name="input">команда, введенная пользователем в консоль</param>
        /// <returns>результат выполнения команды</returns>
        public string Execute(string input)
        {
            var args = input.Split(' ');
            var appName = args[0];
            
            // Находим команду с заданным именем и выполняем ее.
            foreach (var app in commands.Where(app => app.Name == appName))
            {
                return app.Run(args);
            }

            if (args[0] == Program.EXIT_COMMAND)
            {
                return "Bye!" + Environment.NewLine;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            return "Unknown command. Type \"help\" to get a list of available commands.";
        }
    }
}