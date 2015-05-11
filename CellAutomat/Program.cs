using System;
using CellAutomat.Data;

namespace CellAutomat
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataLoader = new DataLoader(args[0]);
            var dupa = dataLoader.LoadMatrix();
            Console.ReadLine();
        }
    }
}
