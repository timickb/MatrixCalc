namespace MatrixCalc
{
    /// <summary>
    /// Интерфейс, описывающий взаимодействие
    /// обработчика команд с каждой
    /// конкретной командой.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Название команды, которое пользователь будет вводить
        /// в командной строке.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Запускает выполнение данной команды.
        /// </summary>
        /// <param name="args">массив параметров</param>
        /// <returns></returns>
        string Run(string[] args);
    }
}