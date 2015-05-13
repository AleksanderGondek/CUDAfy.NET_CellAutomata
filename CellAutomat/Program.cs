using System;
using System.Reflection;
using CellAutomat.Engine;
using Ninject;

namespace CellAutomat
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            EngineRunner.Run();
            var a = ComputationsEngine.Matrix;
            Console.WriteLine("Press a button, dummy!");
            Console.ReadLine();
        }
    }
}
