using System;
using CellAutomat.Input;

namespace CellAutomat
{
    class Program
    {
        static void Main(string[] args)
        {
            var intputManager = new InputManager() {Args = args};
            
            intputManager.HandleInputArgs();

            Console.WriteLine("Finished work!");
            Console.WriteLine("Press any key to quit...");
            
            Console.ReadLine();
        }
    }
}
