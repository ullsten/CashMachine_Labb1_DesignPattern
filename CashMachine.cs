﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using CashMachine_Labb1_DesignPattern.Interfaces;
using CashMachine_Labb1_DesignPattern.Model;
using CashMachine_Labb1_DesignPattern.Strategy;
using Spectre.Console;

namespace CashMachine_Labb1_DesignPattern
{
    // CashMachine (Observer Pattern)
    public class CashMachine : ICashMachineObserver
    {
        private CashMachine _cashMachine;
        private static readonly object lockObject = new object();
        private static bool isCardInserted = true;
        private static int[] validPins = { 1234, 5678, 9876 }; // Define the valid PINs
        private static List<Account> accounts = new List<Account>()
        {
            new Account { AccountID = GenerateRandomAccountNumber(), Pin = 1234, Balance = 100000 },
            new Account { AccountID = GenerateRandomAccountNumber(), Pin = 5678, Balance = 58500 },
            new Account { AccountID = GenerateRandomAccountNumber(), Pin = 9876, Balance = 456800 }
        };
        private int enteredPin;
        private string loggedInCustomerName;
        public void InsertCard()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Welcome to AI ATM bank:");
            Console.ResetColor();
            Console.Write("Enter your name: ");
            var name = Console.ReadLine();
            loggedInCustomerName = name;
            Console.WriteLine();

            AnsiConsole.MarkupLine($"Insert your card [yellow]{name}[/]? (Y/N)");
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
            Console.WriteLine();
            Console.WriteLine("Getting account info.");
            Thread.Sleep(550);
            Console.WriteLine("..");
            Thread.Sleep(550);
            Console.WriteLine("...");
            Thread.Sleep(550);
            Console.WriteLine("....Done");
            Thread.Sleep(550);

            Console.Clear();
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
            var account = GetAccountByPin(enteredPin);

            if (account != null)
            {
                if (ValidateBalance(amount, account.Balance))
                {
                    DeductBalance(amount, account);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    CashMachineSubject.NotifyObservers("Withdrawal successful.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    CashMachineSubject.NotifyObservers($"New balance is: ${account.Balance}");
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
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                CashMachineSubject.NotifyObservers("Invalid PIN. Please try again.");
                Console.ResetColor();
            }
        }

        public void PerformCheckBalance()
        {
            var account = GetAccountByPin(enteredPin);
            if (account != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Your current balance is: ${account.Balance}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                CashMachineSubject.NotifyObservers("Invalid PIN. Please try again.");
                Console.ResetColor();
            }
        }

        public void PerformInsertMoney()
        {
            var account = GetAccountByPin(enteredPin);
            if (account != null)
            {
                double amount = ReadAmountInput("Enter the amount to insert: ");
                account.Balance += amount;
                double newBalance = account.Balance;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                CashMachineSubject.NotifyObservers($"New balance is: ${newBalance}");
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                CashMachineSubject.NotifyObservers("Invalid PIN. Please try again.");
                Console.ResetColor();
            }
        }

        public void PerformTransferMoney()
        {
            Console.WriteLine("Available accounts:");
            //filter to only show account != enteredPin account
            var sourceAccount = GetAccountByPin(enteredPin);
            Console.WriteLine(new string('-', 40));
            foreach (var account in accounts.Where(a=>a != sourceAccount))
            {
               AnsiConsole.MarkupLine($"[lightsalmon1]Account ID:[/] [mediumpurple1]{account.AccountID}[/]");
            }
            Console.WriteLine(new string('-', 40));
            string targetAccountId = ReadAccountIdInput("Enter the account ID to transfer money to: ");
            double amount = ReadAmountInput("Enter the amount to transfer: ");

            sourceAccount = GetAccountByPin(enteredPin);
            var targetAccount = GetAccountById(targetAccountId);

            if (sourceAccount != null && targetAccount != null)
            {
                if (ValidateBalance(amount, sourceAccount.Balance))
                {
                    DeductBalance(amount, sourceAccount);
                    AddBalance(amount, targetAccount);

                    Console.WriteLine(new string('-', 70));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    CashMachineSubject.NotifyObservers($"Transfer of ${amount} successful.");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    CashMachineSubject.NotifyObservers($"New balance for your account {sourceAccount.AccountID} is: ${sourceAccount.Balance}");
                    Console.ResetColor();
                    //CashMachineSubject.NotifyObservers($"New balance for account {targetAccount.AccountID} is: ${targetAccount.Balance}");
                    Console.WriteLine(new string('-', 70));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    CashMachineSubject.NotifyObservers("Insufficient balance. Please try again.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                CashMachineSubject.NotifyObservers("Invalid PIN or account ID. Please try again.");
                Console.ResetColor();
            }
        }

        private string ReadAccountIdInput(string prompt)
        {
            Console.Write(prompt);
            string accountId = Console.ReadLine();

            return accountId;
        }


        private Account GetAccountById(string accountId)
        {
            return accounts.FirstOrDefault(a => a.AccountID == accountId);
        }
        private void AddBalance(double amount, Account account)
        {
            account.Balance += amount;
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

        private void DeductBalance(double amount, Account account)
        {
            // Subtracts the specified amount from the balance.
            account.Balance -= amount;
        }

        private Account GetAccountByPin(int pin)
        {
            return accounts.FirstOrDefault(a => a.Pin == pin);
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
        public static string GenerateRandomAccountNumber()
        {
            Random random = new Random();
            string accountNumber = string.Empty;

            // Generate a 5-digit random number
            for (int i = 0; i < 5; i++)
            {
                accountNumber += random.Next(0, 10).ToString();
            }

            return accountNumber;
        }
        public void LoggedInAccount()
        {
            var loggedInAccount = GetAccountByPin(enteredPin);

            Console.WriteLine();
            Console.WriteLine(new string('-', 20));
            AnsiConsole.MarkupLine("[orange1]Logged in[/]");
            AnsiConsole.MarkupLine($"[green3]Account owner:[/] [yellow]{loggedInCustomerName}[/]");
            AnsiConsole.MarkupLine($"[springgreen3]Account ID:[/] [blue]{loggedInAccount.AccountID}[/]");
            AnsiConsole.MarkupLine($"[darkcyan]Balance:[/] [lightpink1]${loggedInAccount.Balance}[/]");
            Console.ResetColor();
            Console.WriteLine(new string('-', 20));
        }
    }
}
