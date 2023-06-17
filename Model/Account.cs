using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMachine_Labb1_DesignPattern.Model
{
    internal class Account
    {
        public string AccountID { get; set; }
        public int Pin { get; set; }
        public double Balance { get; set; }
    }
}
