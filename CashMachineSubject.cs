using System.Collections.Generic;
using CashMachine_Labb1_DesignPattern.Interfaces;

namespace CashMachine_Labb1_DesignPattern
{
    // Observer pattern: CashMachineSubject
    public static class CashMachineSubject
    {
        private static List<ICashMachineObserver> observers = new List<ICashMachineObserver>();

        public static void AttachObserver(ICashMachineObserver observer)
        {
            observers.Add(observer);
        }

        public static void DetachObserver(ICashMachineObserver observer)
        {
            observers.Remove(observer);
        }

        public static void NotifyObservers(string message)
        {
            foreach (var observer in observers)
            {
                observer.Update(message);
            }
        }
    }
}
