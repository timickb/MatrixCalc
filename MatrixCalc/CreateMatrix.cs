namespace MatrixCalc
{
    public class CreateMatrix : ICommand
    {
        public string Name { get; set; }

        public CreateMatrix(string name)
        {
            Name = name;
        }
        public string Run(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}