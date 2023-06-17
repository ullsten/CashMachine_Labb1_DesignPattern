using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern.Strategy
{
    // Strategy pattern: CheckBalanceOperationStrategy
    public class CheckBalanceOperationStrategy : IOperationStrategy
    {
        private CashMachine _cashMachine;

        public CheckBalanceOperationStrategy(CashMachine cashMachine)
        {
            _cashMachine = cashMachine;
        }

        public void PerformOperation()
        {
            _cashMachine.PerformCheckBalance();
        }
    }
}
