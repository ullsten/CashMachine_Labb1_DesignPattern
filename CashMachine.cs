using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern
{
    // CashMachine (Observer Pattern)
    public class CashMachine : ICashMachineObserver
    {
        private static readonly object lockObject = new object();
        private static bool isCardInserted = false;
        private double balance = 100000; //Define account balance, same for all validPins
        int[] validPins = { 1234, 5678, 9876 }; // Define the valid PINs

        public void InsertCard()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Welcome to AI ATM bank:");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Do you have your card? [Y/N]");
            var cardAnswer = Console.ReadLine();
            if (cardAnswer != null)
            {
                if(cardAnswer.ToLower() == "y")
                {
                    lock (lockObject)
                    {
                        if (!isCardInserted)
                        {
                            isCardInserted = true;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            CashMachineSubject.NotifyObservers("Card inserted");
                            SneakPeek();
                            Console.ResetColor();
                        }
                    }
                }
                else
                {
                    isCardInserted = false;
                    Console.ForegroundColor= ConsoleColor.Red;
                    CashMachineSubject.NotifyObservers("You are locked out, please contact your bank.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Thread.Sleep(300);
                    Console.ForegroundColor = ConsoleColor.White;
                    CashMachineSubject.NotifyObservers("1..");
                    Thread.Sleep(300);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    CashMachineSubject.NotifyObservers(".2");
                    Thread.Sleep(300);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    CashMachineSubject.NotifyObservers("..3");
                    Thread.Sleep(300);
                    Console.WriteLine();
                    Console.WriteLine("Have you found your card? [Y/N]");
                    var foundCard = Console.ReadLine();
                    if(foundCard.ToLower() == "y")
                    {
                        InsertCard();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        CashMachineSubject.NotifyObservers("BYE");
                        Console.ResetColor();
                        Environment.Exit(0);
                    }        
                }
            }
        }

        public void EnterPin(int pin)
        {
            while (!validPins.Contains(pin))
            {

                Console.ForegroundColor = ConsoleColor.Red;
                CashMachineSubject.NotifyObservers("Invalid PIN. Please try again.");
                Console.ResetColor();
                pin = ReadPinInput();
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            CashMachineSubject.NotifyObservers("PIN entered");
            Console.ResetColor();
        }

        private int ReadPinInput()
        {
            Console.Write("Enter your PIN: ");
            string input = Console.ReadLine();
            int pin;
            while (!int.TryParse(input, out pin))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Invalid input. Please enter a valid PIN: ");
                Console.ResetColor();
                input = Console.ReadLine();
            }
            return pin;
        }

        public void PerformWithdraw()
        {
            double amount = ReadAmountInput("Enter the amount to withdraw: ");
            if (ValidateBalance(amount))
            {
                DeductBalance(amount);
                Console.ForegroundColor = ConsoleColor.Blue;
                CashMachineSubject.NotifyObservers("Withdrawal successful.");
                Console.ForegroundColor= ConsoleColor.Green;
                CashMachineSubject.NotifyObservers($"New balance is: ${balance}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                CashMachineSubject.NotifyObservers("Insufficient balance. Please try again.");
                Console.ResetColor();
            }
        }

        public void PerformCheckBalance()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Your current balance is: ${balance}");
            Console.ResetColor();
        }

        private double ReadAmountInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            double amount;
            while (!double.TryParse(input, out amount))
            {
                CashMachineSubject.NotifyObservers("Invalid input. Please enter a valid amount: ");
               // Console.WriteLine("Invalid input. Please enter a valid amount: ");
                input = Console.ReadLine();
            }
            return amount;
        }

        private bool ValidateBalance(double amount)
        {
            // Checks if the current balance is greater than or equal to the specified amount.
            return balance >= amount;
        }

        private void DeductBalance(double amount)
        {
            // Subtracts the specified amount from the balance.
            balance -= amount;
        }

        public void Update(string message)
        {
            // Displays the provided message with the prefix "CashMachineObserver: ".
            Console.WriteLine("CashMachineObserver: " + message);
        }


        public void SneakPeek()
        {
            CashMachineSubject.NotifyObservers("Valid pins: ");
            foreach (var validpin in validPins)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(validpin);
                Console.ResetColor();
            }
        }
    }
}
