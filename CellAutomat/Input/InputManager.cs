using System;
using System.Linq;
using System.Reflection;
using CellAutomat.Data;
using CellAutomat.Engine;
using Cudafy;
using Cudafy.Host;

namespace CellAutomat.Input
{
    public sealed class InputManager
    {
        public string[] Args { get; set; }

        public void HandleInputArgs()
        {
            if (Args == null || !Args.Any())
            {
                Console.WriteLine("You need to specify desired action - create, simulate or launch interactive, to run this app");
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
                case @"interactive":
                    Interactive();
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
                Console.WriteLine("Exception name: {0}", exception);
                Console.WriteLine("Exception details: {0}", exception.Message);
                Console.WriteLine("I am sorry dave, I am afraid I cannot allow you to do that!");
                Console.ReadLine();
            }
        }

        private void Interactive()
        {
            Console.WriteLine("Running in interactive mode.");

            PrintOutAviableDevices();

            bool[,,] matrix;

            var chosenDeviceId = 0;
            var matrixSize = 10;
            var chanceOfAlive = 20;
            
            var generationsToSimulate = 1;
            var lonelinessDeathNumber = 2;
            var overcrowingDeathNumber = 3;
            var revivalNumber = 3;

            Console.WriteLine("Please enter id of device chosen for computations");
            chosenDeviceId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter desired matrix size (single number,matrix is a cube)");
            matrixSize = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter % chance of cell being alive at the start");
            chanceOfAlive = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter number of generations to calculate");
            generationsToSimulate = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter LonelinessDeathNumber - cells with fewer than this number alive neighbours will die");
            lonelinessDeathNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter OvercrowingDeathNumber - cells with more than this number alive neighbours will die");
            overcrowingDeathNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter RevivalNumber - cells with exactly this number of alive neighbours will come to live");
            revivalNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Creating an matrix of size {0}, with chance of cell being alive equal to {1}", matrixSize, chanceOfAlive);

            using (var dataCreator = new DataCreator())
            {
                dataCreator.MatrixSize = matrixSize;
                dataCreator.ChanceOfTrue = chanceOfAlive;

                matrix = dataCreator.CreateStartupMatrix();
            }

            Console.WriteLine("{0} matrix bytes", matrix.LongLength*sizeof(bool));

            Console.WriteLine("Running a simulation on device {0} of {1} generations, with LonelinessDeathNumber {2}, OvercrowdingDeathNumber {3} and revivalNumber {4}",
                chosenDeviceId, generationsToSimulate, lonelinessDeathNumber, overcrowingDeathNumber, revivalNumber);

            try
            {
                ComputationsEngine.Matrix = matrix;
                ComputationsEngine.Generations = generationsToSimulate;
                ComputationsEngine.LonelinessDeathNumber = lonelinessDeathNumber;
                ComputationsEngine.OvercrowingDeathNumber = overcrowingDeathNumber;
                ComputationsEngine.RevivalNumber = revivalNumber;
                ComputationsEngine.Execute();
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception has occured!");
                Console.WriteLine("Exception name: {0}", exception);
                Console.WriteLine("Exception details: {0}", exception.Message);
                Console.WriteLine("I am sorry dave, I am afraid I cannot allow you to do that!");
                Console.ReadLine();
            }
        }

        private void PrintOutAviableDevices()
        {
            Console.WriteLine("Printing out avaiable devices...");
            Console.WriteLine("For now this code will work only with Cuda devs...");

            var numberOfCudaDevices = CudafyHost.GetDeviceCount(eGPUType.Cuda);
            var numberOfOpenClDevices = CudafyHost.GetDeviceCount(eGPUType.OpenCL);
            var numberOfEmulatorDevices = CudafyHost.GetDeviceCount(eGPUType.Emulator);

            Console.WriteLine("{0} devices of type Cuda found", numberOfCudaDevices);
            Console.WriteLine("{0} devices of type OpenCl found", numberOfOpenClDevices);
            Console.WriteLine("{0} devices of type Emulator found", numberOfEmulatorDevices);

            Console.WriteLine("Attempting to print out detailed info about Cuda devices..");
            var cudaDevicesProperties = CudafyHost.GetDeviceProperties(eGPUType.Cuda);
            if (cudaDevicesProperties.Count() != numberOfCudaDevices)
            {
                Console.WriteLine("Something is terribly off! Number of cuda devices differ from received properites");
            }

            foreach (var cudaDeviceProperties in cudaDevicesProperties)
            {
                Console.WriteLine(@"---");
                PrintOutObjectPublicProperties(cudaDeviceProperties);
                Console.WriteLine(@"---");
            }

            Console.WriteLine("Attempting to print out detailed info about openCl devices..");
            var openClDevicesProperties = CudafyHost.GetDeviceProperties(eGPUType.OpenCL);
            if (openClDevicesProperties.Count() != numberOfOpenClDevices)
            {
                Console.WriteLine("Something is terribly off! Number of openCl devices differ from received properites");
            }

            foreach (var openClDeviceProperties in openClDevicesProperties)
            {
                Console.WriteLine(@"---");
                PrintOutObjectPublicProperties(openClDeviceProperties);
                Console.WriteLine(@"---");
            }

            Console.WriteLine("Attempting to print out detailed info about emulator devices..");
            var emulatorDevicesProperties = CudafyHost.GetDeviceProperties(eGPUType.Emulator);
            if (emulatorDevicesProperties.Count() != numberOfEmulatorDevices)
            {
                Console.WriteLine("Something is terribly off! Number of emulator devices differ from received properites");
            }

            foreach (var emulatorDeviceProperties in emulatorDevicesProperties)
            {
                Console.WriteLine(@"---");
                PrintOutObjectPublicProperties(emulatorDeviceProperties);
                Console.WriteLine(@"---");
            }            
        }

        private void PrintOutObjectPublicProperties(object thing)
        {
            foreach (var propertyInfo in thing.GetType().GetProperties().Where(propertyInfo => propertyInfo.CanRead))
            {
                Console.WriteLine("{0}: {1}", propertyInfo.Name, propertyInfo.GetValue(thing, null));
            }
        }
    }
}
