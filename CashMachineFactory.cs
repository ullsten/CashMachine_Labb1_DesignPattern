using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMachine_Labb1_DesignPattern
{
    // Factory pattern: CashMachineFactory
    public static class CashMachineFactory
    {
        public static CashMachine CreateCashMachine()
        {
            return new CashMachine();
        }
    }
}
