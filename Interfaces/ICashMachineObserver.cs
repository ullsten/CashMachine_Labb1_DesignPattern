﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMachine_Labb1_DesignPattern.Interfaces
{
    // Observer pattern: ICashMachineObserver
    public interface ICashMachineObserver
    {
        void Update(string message);
    }
}
