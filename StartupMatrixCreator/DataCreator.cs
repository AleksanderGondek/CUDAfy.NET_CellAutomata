using System;
using System.IO;
using Newtonsoft.Json;

namespace StartupMatrixCreator
{
    internal sealed class DataCreator
    {
        private readonly int _matrixSize;
        private readonly string _fileName;
        private readonly int _chanceOfTrue;
        private readonly Random _randomGenerator;

        internal DataCreator(int matrixSize, string fileName, int chanceOfTrue)
        {
            _matrixSize = matrixSize;
            _fileName = fileName;
            _chanceOfTrue = chanceOfTrue;
            _randomGenerator = new Random((int)DateTime.Now.Ticks); 
        }

        internal void CreateStartupMatrix()
        {
            var matrix = new bool[_matrixSize, _matrixSize, _matrixSize];
            for (var z = 0; z < _matrixSize; z++)
            {
                for (var x = 0; x < _matrixSize; x++)
                {
                    for (var y = 0; y < _matrixSize; y++)
                    {
                        matrix[x, y, z] = GetValue();
                    }
                }
            }

            SaveMatrix(matrix);
        }

        internal void SaveMatrix(bool[,,] matrix)
        {
            var serializedMatrix = JsonConvert.SerializeObject(matrix);
            File.WriteAllText(_fileName, serializedMatrix);
        }

        private bool GetValue()
        {
            return _randomGenerator.Next(100) < _chanceOfTrue;
        }
    }
}
