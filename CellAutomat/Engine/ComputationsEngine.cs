using System;
using CellAutomat.Data;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CellAutomat.Engine
{
    public sealed class ComputationsEngine
    {
        public const int MaximumDimensionSize = 6;

        public static bool[, ,] Matrix { get; set; }

        public static int Generations { get; set; }
        public static int LonelinessDeathNumber { get; set; }
        public static int OvercrowingDeathNumber { get; set; }
        public static int RevivalNumber { get; set; }

        public static long GetMatrixSize()
        {
            return Matrix != null ? Matrix.GetLength(0) : -1;
        }

        public static dim3 GetBlockSize()
        {
            return GetMatrixSize() >= MaximumDimensionSize ? 
                new dim3(MaximumDimensionSize, MaximumDimensionSize, MaximumDimensionSize) 
                : new dim3(GetMatrixSize(), GetMatrixSize(), GetMatrixSize());
        }

        public static dim3 GetGridSize()
        {
            var timesMore = GetMatrixSize()/MaximumDimensionSize;
            if (timesMore == 0)
            {
                timesMore = 1;
            }

            return new dim3(timesMore, timesMore, timesMore);
        }

        public static void Execute()
        {
            var gpu = CudafyHost.GetDevice(CudafyModes.Target);
            var arch = gpu.GetArchitecture();
            var km = CudafyTranslator.Cudafy(arch);

            gpu.LoadModule(km);

            for (var i = 0; i <= Generations; i++)
            {
                // allocate the memory on the GPU
                var deviceMatrix = gpu.Allocate(Matrix);
                var rulesArray = new[] {LonelinessDeathNumber, OvercrowingDeathNumber, RevivalNumber};
                var rules = gpu.Allocate(rulesArray);

                // copy the matrix to the GPU
                gpu.CopyToDevice(Matrix, deviceMatrix);
                gpu.CopyToDevice(rulesArray, rules);

                gpu.Launch(GetGridSize(), GetBlockSize(), @"Simulation", deviceMatrix, rules);

                // copy the array 'c' back from the GPU to the CPU
                gpu.CopyFromDevice(deviceMatrix, Matrix);

                // verify that the GPU did the work we requested

                // free the memory allocated on the GPU
                gpu.Free(deviceMatrix);

                //Save on disk
                DataWriter.SaveMatrixAsJs(Matrix, string.Format("MatrixOutput_Gen{0}.js", i));

                // free the memory we allocated on the CPU
                // Not necessary, this is .NET
            }
        }

        [Cudafy]
        // Lambda expression not supported 0/10
        public static void Simulation(GThread thread, bool[, ,] matrix, int[] rules)
        {
            // Read which cube am I ( + offset for bigger data)
            var threadIdX = thread.threadIdx.x + (thread.blockIdx.x * MaximumDimensionSize);
            var threadIdY = thread.threadIdx.y + (thread.blockIdx.y * MaximumDimensionSize);
            var threadIdZ = thread.threadIdx.z + (thread.blockIdx.z * MaximumDimensionSize);        
 
            // Read rules
            int lonelinessDeathNumber = rules[0];
            int overcrowingDeathNumber = rules[1];
            int revivalNumber = rules[2];

            //Check neighbour status
            var upperNeighbour = threadIdY + 1 <= matrix.GetLength(0) && matrix[threadIdX, threadIdY + 1, threadIdZ];
            var downNeighbour = threadIdY - 1 >= 0 && matrix[threadIdX, threadIdY - 1, threadIdZ];
            var leftNeighbour = threadIdX + 1 <= matrix.GetLength(0) && matrix[threadIdX - 1, threadIdY, threadIdZ];
            var rigthNeighbour = threadIdX - 1 >= 0 && matrix[threadIdX + 1, threadIdY, threadIdZ];
            var northNeighbour = threadIdZ + 1 <= matrix.GetLength(0) && matrix[threadIdX, threadIdY, threadIdZ + 1];
            var southNeighbour = threadIdZ - 1 >= 0 && matrix[threadIdX, threadIdY, threadIdZ - 1];

            var aliveNeighburs = 0;
            if (upperNeighbour) { aliveNeighburs += 1; }
            if (downNeighbour) { aliveNeighburs += 1; }
            if (leftNeighbour) { aliveNeighburs += 1; }
            if (rigthNeighbour) { aliveNeighburs += 1; }
            if (northNeighbour) { aliveNeighburs += 1; }
            if (southNeighbour) { aliveNeighburs += 1; }

            // Should cell die of loneliness
            if (aliveNeighburs < lonelinessDeathNumber)
            {
                matrix[threadIdX, threadIdY, threadIdZ] = false;
            }
            //Should cell die of overcrowding
            else if (aliveNeighburs > overcrowingDeathNumber)
            {
                matrix[threadIdX, threadIdY, threadIdZ] = false;
            }
            //Should cell come back to live
            else if (aliveNeighburs == revivalNumber)
            {
                matrix[threadIdX, threadIdY, threadIdZ] = true;
            }
            //Should cell stay the same
            else if (aliveNeighburs >= lonelinessDeathNumber && aliveNeighburs <= overcrowingDeathNumber)
            {
                //Not necessary
                //matrix[threadIdX, threadIdY, threadIdZ] = matrix[threadIdX, threadIdY, threadIdZ];
            }
        }
    }
}
