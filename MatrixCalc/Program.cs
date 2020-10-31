using System;

namespace MatrixCalc
{
    class Program
    {
        public static string EXIT_COMMAND = "exit";
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("==> Калькулятор матриц <==");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Введите help для получения инструкции.");
            var handler = new CommandHandler();
            
            string userInput;
            do
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("> ");
                userInput = Console.ReadLine();
                Console.WriteLine(handler.Execute(userInput));
            } while (userInput != EXIT_COMMAND);


        }
    }
}