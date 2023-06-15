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
        private double balance = 100000;

        public void InsertCard()
        {
            Console.WriteLine("Card inserted.");
        }

        public void EnterPin(int pin)
        {
            Console.WriteLine("PIN entered.");
        }

        public void PerformWithdraw()
        {
            double amount = ReadAmountInput("Enter the amount to withdraw: ");
            if (ValidateBalance(amount))
            {
                DeductBalance(amount);
                Console.WriteLine("Withdrawal successful.");
            }
            else
            {
                Console.WriteLine("Insufficient balance. Please try again.");
            }
        }

        public void PerformCheckBalance()
        {
            Console.WriteLine($"Your current balance is: {balance}");
        }

        private double ReadAmountInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            double amount;
            while (!double.TryParse(input, out amount))
            {
                Console.WriteLine("Invalid input. Please enter a valid amount: ");
                input = Console.ReadLine();
            }
            return amount;
        }

        private bool ValidateBalance(double amount)
        {
            return balance >= amount;
        }

        private void DeductBalance(double amount)
        {
            balance -= amount;
        }

        public void Update(string message)
        {
            Console.WriteLine("CashMachineObserver: " + message);
        }
    }
}
