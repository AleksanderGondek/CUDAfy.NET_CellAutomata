using System;

namespace StartupMatrixCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                throw new Exception(@"Result file name, matrix size and % chance of true value of single cell required!");
            }

            Console.WriteLine("Creating an matrix of size {0}, with chance of cell being alive equal to {1}, in file {2}...", args[1], args[2], args[0]);
            new DataCreator(Convert.ToInt32(args[1]), args[0], Convert.ToInt32(args[2])).CreateStartupMatrix();
            
            Console.WriteLine("Matrix created. Press any key to exit...");
            Console.ReadLine();
        }
    }
}
