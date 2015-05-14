using System;
using System.Linq;
using CellAutomat.Data;
using CellAutomat.Engine;

namespace CellAutomat.Input
{
    public sealed class InputManager
    {
        public string[] Args { get; set; }

        public void HandleInputArgs()
        {
            if (Args == null || !Args.Any())
            {
                Console.WriteLine("You need to specify desired action - create or simulate, to run this app");
                Console.ReadLine();
                Environment.Exit(0);
            }

            switch (Args[0])
            {
                case @"create":
                    Create();
                    break;
                case @"simulate":
                    Simulate();
                    break;
                default:
                    Console.WriteLine("You have not specified valid action - create or simulate");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
            }
        }

        private void Create()
        {
            if (Args.Length < 3)
            {
                Console.WriteLine(@"Result matrix size and % chance of true value of single cell required!");
                Console.WriteLine(@"I.e. create 10 20, will create cube of 10x10x10 with 20% chance of cube being alive");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("Creating an matrix of size {0}, with chance of cell being alive equal to {1}", Args[1], Args[2]);
            using (var dataCreator = new DataCreator())
            {
                dataCreator.MatrixSize = Convert.ToInt32(Args[1]);
                dataCreator.ChanceOfTrue = Convert.ToInt32(Args[2]);

                DataHandler.SerializeAndSaveMatrix(dataCreator.CreateStartupMatrix(), AppConfigHelper.GetValueFromAppSettings(@"CellMatrixDataLocation"));
            }
        }

        private void Simulate()
        {
            if (Args.Length < 3)
            {
                Console.WriteLine(@"You need to specify the following:");
                Console.WriteLine(@"* Number of generations of the simulation to take place (iterations)");
                Console.WriteLine(@"* LonelinessDeathNumber - cells with fewer than this number alive neighbours will die ");
                Console.WriteLine(@"* OvercrowingDeathNumber - cells with more than this number alive neighbours will die");
                Console.WriteLine(@"* RevivalNumber - cells with exactly this number of alive neighbours will come to live");
                Console.WriteLine("I.e: simulate 100 2 3 3, will simulate basic Conways Game of Life");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("Running a simulation of {0} generations, with LonelinessDeathNumber {1}, OvercrowdingDeathNumber {2} and revivalNumber {3}",
                Args[1], Args[2], Args[3], Args[4]);

            try
            {
                ComputationsEngine.Matrix = DataHandler.LoadMatrix();
                ComputationsEngine.Generations = Convert.ToInt32(Args[1]);
                ComputationsEngine.LonelinessDeathNumber = Convert.ToInt32(Args[2]);
                ComputationsEngine.OvercrowingDeathNumber = Convert.ToInt32(Args[3]);
                ComputationsEngine.RevivalNumber = Convert.ToInt32(Args[4]);
                ComputationsEngine.Execute();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception has occured!");
                Console.WriteLine("Exception name: {0}", exception.ToString());
                Console.WriteLine("Exception details: {0}", exception.Message);
                Console.WriteLine("I am sorry dave, I am afraid I cannot allow you to do that!");
                Console.ReadLine();
            }
        }
    }
}
