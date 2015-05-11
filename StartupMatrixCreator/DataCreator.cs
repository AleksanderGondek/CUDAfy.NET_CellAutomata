using System;
using System.IO;

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
            using (var fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(_matrixSize);
                for (var z = 0; z < _matrixSize; z++)
                {
                    for (var x = 0; x < _matrixSize; x++)
                    {
                        for (var y = 0; y < _matrixSize; y++)
                        {
                            streamWriter.WriteLine(GetSingleLine(x,y,z));
                        }
                    }
                }
            }
        }

        private string GetSingleLine(int x, int y, int z)
        {
            return string.Format("{{{0},{1},{2}}}={3}", x, y, z, GetValue().ToString().ToLower());
        }

        private bool GetValue()
        {
            return _randomGenerator.Next(100) < _chanceOfTrue;
        }
    }
}
