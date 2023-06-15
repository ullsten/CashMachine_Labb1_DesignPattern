namespace CashMachine_Labb1_DesignPattern
{
    // Factory Method, Observer, Strategy patterns
    internal class Program
    {
        static void Main(string[] args)
        {
            UserInterface userInterface = new UserInterface();
            userInterface.RunProgram();
        }
    }
}
