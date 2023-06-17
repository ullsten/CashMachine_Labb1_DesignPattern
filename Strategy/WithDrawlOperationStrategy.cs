using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern.Strategy
{
    // Strategy pattern: WithdrawOperationStrategy
    public class WithdrawOperationStrategy : IOperationStrategy
    {
        private CashMachine _cashMachine;

        public WithdrawOperationStrategy(CashMachine cashMachine)
        {
            _cashMachine = cashMachine;
        }

        public void PerformOperation()
        {
            _cashMachine.PerformWithdraw();
        }
    }
}
