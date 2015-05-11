using System;

namespace CellAutomat.Data
{
    public sealed class DataCreator : IDataCreator
    {
        public int MatrixSize { get; set; }
        public int ChanceOfTrue { get; set; }

        private Random _randomGenerator = new Random((int)DateTime.Now.Ticks);
        private bool[,,] _matrix;

        public bool[,,] CreateStartupMatrix()
        {
            if (MatrixSize <= 0 || ChanceOfTrue <= 0)
            {
                throw new Exception("No valid parameters for matrix size / chance of cell being alive were supplied!");
            }

            _matrix = new bool[MatrixSize, MatrixSize, MatrixSize];
            for (var z = 0; z < MatrixSize; z++)
            {
                for (var x = 0; x < MatrixSize; x++)
                {
                    for (var y = 0; y < MatrixSize; y++)
                    {
                        _matrix[x, y, z] = GetValue();
                    }
                }
            }

            return _matrix;
        }

        private bool GetValue()
        {
            return _randomGenerator.Next(100) < ChanceOfTrue;
        }

        public void Dispose()
        {
            _randomGenerator = null;
        }
    }
}
