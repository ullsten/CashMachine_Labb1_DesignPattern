using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern
{
    // Strategy pattern: CheckBalanceOperationStrategy
    public class CheckBalanceOperationStrategy : IOperationStrategy
    {
        private CashMachine cashMachine;

        public CheckBalanceOperationStrategy(CashMachine cashMachine)
        {
            this.cashMachine = cashMachine;
        }

        public void PerformOperation()
        {
            cashMachine.PerformCheckBalance();
        }
    }
}
