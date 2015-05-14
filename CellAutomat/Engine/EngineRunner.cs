using CellAutomat.Data;
using Cudafy;
using Cudafy.Translator;

namespace CellAutomat.Engine
{
    public sealed class EngineRunner
    {
        public static void Run()
        {
            CudafyModes.Target = eGPUType.Cuda;
            CudafyModes.DeviceId = 0;
            CudafyTranslator.Language = CudafyModes.Target == eGPUType.OpenCL ? eLanguage.OpenCL : eLanguage.Cuda;

            ComputationsEngine.Matrix = DataHandler.LoadMatrix();
            ComputationsEngine.Generations = 100;
            ComputationsEngine.LonelinessDeathNumber = 2;
            ComputationsEngine.OvercrowingDeathNumber = 3;
            ComputationsEngine.RevivalNumber = 3;
            ComputationsEngine.Execute();
        }
    }
}
