using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;

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
                Console.Write("Enter your PIN: ");
                string input = Console.ReadLine();
                int pin;
                while (!int.TryParse(input, out pin))
                {
                    Console.WriteLine("Invalid input. Please enter a valid PIN: ");
                    input = Console.ReadLine();
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
            cashMachine.EnterPin(userInputHandler.ReadPinInput());

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Available operations:");
                Console.WriteLine("1. Withdraw Cash");
                Console.WriteLine("2. Check Balance");
                Console.WriteLine("3. Exit");

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        PerformOperation(new WithdrawOperationStrategy(cashMachine));
                        break;
                    case "2":
                        PerformOperation(new CheckBalanceOperationStrategy(cashMachine));
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            CashMachineSubject.DetachObserver(transactionLogger);
        }

        private void PerformOperation(IOperationStrategy operationStrategy)
        {
            operationStrategy.PerformOperation();
        }
    }
}
