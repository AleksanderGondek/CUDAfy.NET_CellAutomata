using System;

namespace CellAutomat.Data
{
    internal sealed class DataCreator
    {
        private readonly int _matrixSize;
        private readonly int _chanceOfTrue;
        private readonly Random _randomGenerator;
        private bool[,,] _matrix;

        internal DataCreator(int matrixSize, int chanceOfTrue)
        {
            _matrixSize = matrixSize;
            _chanceOfTrue = chanceOfTrue;
            _randomGenerator = new Random((int)DateTime.Now.Ticks); 
        }

        internal bool[,,] CreateStartupMatrix()
        {
            _matrix = new bool[_matrixSize, _matrixSize, _matrixSize];
            for (var z = 0; z < _matrixSize; z++)
            {
                for (var x = 0; x < _matrixSize; x++)
                {
                    for (var y = 0; y < _matrixSize; y++)
                    {
                        _matrix[x, y, z] = GetValue();
                    }
                }
            }

            return _matrix;
        }

        private bool GetValue()
        {
            return _randomGenerator.Next(100) < _chanceOfTrue;
        }
    }
}
