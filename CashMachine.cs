using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern
{
    // CashMachine (Observer Pattern)
    public class CashMachine : ICashMachineObserver
    {
        private static readonly object lockObject = new object();
        private static bool isCardInserted = true;
        private static int[] validPins = { 1234, 5678, 9876 }; // Define the valid PINs
        private static Dictionary<int, double> accountBalances = new Dictionary<int, double>()
        {
            {1234, 100000 },
            {5678, 58500 },
            {9876, 456800 },

        };
        private int enteredPin;
        private double balance; // Define account balance, same for all validPins

        public void InsertCard()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Welcome to AI ATM bank:");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Have you inserted the card? [Y/N]");
            var cardAnswer = Console.ReadLine();
            if (cardAnswer != null)
            {
                if (cardAnswer.ToLower() == "y")
                {
                    lock (lockObject)
                    {
                        if (isCardInserted)
                        {
                            isCardInserted = true;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            CashMachineSubject.NotifyObservers("Card inserted successfully");
                            Console.ResetColor();
                            Console.WriteLine();
                            SneakPeek();

                        }
                    }
                }
                else
                {
                    isCardInserted = false;
                    Console.ForegroundColor = ConsoleColor.Red;
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
                    if (foundCard.ToLower() == "y")
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

        public void EnterPin()
        {
            do
            {
                enteredPin = ReadPinInput();

                if (!validPins.Contains(enteredPin))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CashMachineSubject.NotifyObservers("Invalid PIN. Please try again.");
                    Console.ResetColor();
                }
            }
            while (!validPins.Contains(enteredPin));

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine();
            CashMachineSubject.NotifyObservers("PIN entered successfully");
            Console.ResetColor();
        }

        private int ReadPinInput()
        {
            Console.Write("Enter your PIN: ");
            string input = Console.ReadLine();

            while (!int.TryParse(input, out enteredPin))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                CashMachineSubject.NotifyObservers("Invalid input. Please enter a valid PIN in numbers: ");
                Console.ResetColor();
                input = Console.ReadLine();
            }
            return enteredPin;
        }

        public void PerformWithdraw()
        {
            double amount = ReadAmountInput("Enter the amount to withdraw: ");
            double currentBalance = accountBalances[enteredPin]; // Get current balance from entered pin
            if (ValidateBalance(amount, currentBalance))
            {
                DeductBalance(amount, enteredPin); // Update the current balance
                currentBalance = accountBalances[enteredPin]; // Update the current balance after deduction

                Console.ForegroundColor = ConsoleColor.Blue;
                CashMachineSubject.NotifyObservers("Withdrawal successful.");
                Console.ForegroundColor = ConsoleColor.Green;
                CashMachineSubject.NotifyObservers($"New balance is: ${currentBalance}");
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
            double currentBalance = accountBalances[enteredPin];
            Console.ForegroundColor = ConsoleColor.Green;
            CashMachineSubject.NotifyObservers($"Your current balance is: ${currentBalance}");
            Console.ResetColor();
        }

        public void PerformInsertMoney()
        {
            double amount = ReadAmountInput("Enter the amount to insert: ");
            accountBalances[enteredPin] += amount;
            double newBalance = accountBalances[enteredPin];
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            CashMachineSubject.NotifyObservers("Insert successfully");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            CashMachineSubject.NotifyObservers($"New balance is: ${newBalance}");
            Console.ResetColor();
            Console.WriteLine();
        }

        private double ReadAmountInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            double amount;
            while (!double.TryParse(input, out amount))
            {
                CashMachineSubject.NotifyObservers("Invalid input. Please enter a valid amount: ");
                input = Console.ReadLine();
            }
            return amount;
        }

        private bool ValidateBalance(double amount, double currentBalance)
        {
            // Checks if the current balance is greater than or equal to the specified amount.
            return currentBalance >= amount;
        }

        private void DeductBalance(double amount, int pin)
        {
            // Subtracts the specified amount from the balance.
            accountBalances[pin] -= amount;
        }

        public void Update(string message)
        {
            // Displays the provided message with the prefix "CashMachineObserver: ".
            Console.WriteLine("CashMachineObserver: " + message);
        }

        public void SneakPeek()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
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
