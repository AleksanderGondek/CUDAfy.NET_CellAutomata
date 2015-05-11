using System;
using System.Reflection;
using Ninject;

namespace CellAutomat
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            Console.ReadLine();
        }
    }
}
