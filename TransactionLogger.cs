using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern
{
    // Observer pattern: TransactionLogger

    public class TransactionLogger : ICashMachineObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"[Transaction Log] {message}");
        }
    }
}
