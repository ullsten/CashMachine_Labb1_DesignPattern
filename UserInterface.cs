using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;
using CashMachine_Labb1_DesignPattern.Strategy;

namespace CashMachine_Labb1_DesignPattern
{
    // User interface (Factory Method, Observer, Strategy patterns)
    public class UserInterface
    {

        private CashMachine cashMachine;
        private UserInputHandler userInputHandler;

        // UserInputHandler
        public class UserInputHandler
        {
            public int ReadPinInput()
            {
                Console.WriteLine();
                Console.WriteLine("Enter your PIN: ");
                string input = Console.ReadLine();
                int pin;
                while (!int.TryParse(input, out pin))
                {
                    Console.ForegroundColor = ConsoleColor.Red; 
                    CashMachineSubject.NotifyObservers("Invalid input. Please enter a valid PIN in numbers: ");
                    input = Console.ReadLine();
                    Console.ResetColor();
                }
                return pin;
            }
        }

        public UserInterface()
        {
            cashMachine = CashMachineFactory.CreateCashMachine();
            userInputHandler = new UserInputHandler();
        }

        public void RunProgram()
        {
            var transactionLogger = new TransactionLogger();
            CashMachineSubject.AttachObserver(transactionLogger);

            cashMachine.InsertCard();
            cashMachine.EnterPin();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("Available operations");
                Console.WriteLine(new string('-', 20));
                Console.ForegroundColor= ConsoleColor.Magenta;
                Console.WriteLine("1. Check Balance");
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine("2. Insert Cash");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("3. Withdraw Cash");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("4. Enter new pin");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("5. Log out");
                Console.ResetColor();
                Console.WriteLine(new string('-', 20));
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        PerformOperation(new CheckBalanceOperationStrategy(cashMachine));
                        break;
                    case "2":
                        Console.Clear();
                        PerformOperation(new InsertMoneyOperationStrategy(cashMachine));
                        
                        break;
                    case "3":
                        Console.Clear();
                        PerformOperation(new WithdrawOperationStrategy(cashMachine));
                        break;
                    case "4":
                        Console.Clear();
                        cashMachine.SneakPeek();
                        cashMachine.EnterPin();

                        break; 
                    case "5":
                        ExitUI();
                        exit = true;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        CashMachineSubject.NotifyObservers("Invalid choice. Please try again");
                        Console.ResetColor();
                        break;
                }
            }

            CashMachineSubject.DetachObserver(transactionLogger);
        }

        private void PerformOperation(IOperationStrategy operationStrategy)
        {
            operationStrategy.PerformOperation();
        }

        private void ExitUI()
        {
            Console.WriteLine();
            CashMachineSubject.NotifyObservers("The door is closing...");
            Thread.Sleep(500);

            Console.WriteLine("     ___________");
            Thread.Sleep(100);
            Console.WriteLine("   //           \\|");
            Thread.Sleep(100);
            Console.WriteLine("  //_____________\\|");
            Thread.Sleep(100);
            Console.WriteLine("  |   -     -   ||");
            Thread.Sleep(100);
            Console.WriteLine("  |             ||");
            Thread.Sleep(100);
            Console.WriteLine("  |   _______   ||");
            Thread.Sleep(100);
            Console.WriteLine("  |  |       |  ||");
            Thread.Sleep(100);
            Console.WriteLine("  |  |       |  ||");
            Thread.Sleep(100);
            Console.WriteLine("  |  |       |  ||");
            Thread.Sleep(100);
            Console.WriteLine("  |  |       |  ||");
            Thread.Sleep(100);
            Console.WriteLine("  |  |_______|  ||");
            Thread.Sleep(100);
            Console.WriteLine("  |             ||");
            Thread.Sleep(100);
            Console.WriteLine("  |             ||");
            Thread.Sleep(100);
            Console.WriteLine("  |_____________||");

            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("The door is closed. Goodbye!");
        }
    }
}
