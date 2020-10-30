using System.Collections.Generic;

namespace MatrixCalc
{
    public class CommandHandler
    {
        private List<ICommand> commands;
        public CommandHandler()
        {
            // Инициализируем все доступные нам команды.
            commands.Add(new CreateMatrix("create"));
        }
        public string Execute(string input)
        {
            
        }
    }
}