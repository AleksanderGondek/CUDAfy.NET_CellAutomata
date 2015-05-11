using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CellAutomat.Data
{
    public sealed class DataLoader : IDataLoader
    {
        private bool[,,] _matrix;
        private Regex _regex = new Regex(@"{([0-9]\d*),([0-9]\d*),([0-9]\d*)}=(true|false)", RegexOptions.IgnoreCase);

        public bool[,,] LoadMatrix()
        {
            ReadFile();
            return _matrix;
        }

        private void ReadFile()
        {
            using (var streamReader = new StreamReader(AppConfigHelper.GetValueFromAppSettings(@"CellMatrixDataLocation")))
            {
                string line;
                var isFirstLine = true;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        InitializeMatrix(line);
                        isFirstLine = false;
                        continue;
                    }

                    ParseSingleLine(line);
                }
            }
        }

        private void InitializeMatrix(string matrixSize)
        {
            var number = Convert.ToInt32(matrixSize);
            _matrix = new bool[number, number, number];
        }

        private void ParseSingleLine(string line)
        {
            var matches = _regex.Match(line);

            if (!matches.Success)
            {
                throw new Exception(@"Could not parse data file content!");
            }

            var dimensionX = Convert.ToInt32(matches.Groups[1].Value);
            var dimensionY = Convert.ToInt32(matches.Groups[2].Value);
            var dimensionZ = Convert.ToInt32(matches.Groups[3].Value);
            var isAlive = Convert.ToBoolean(matches.Groups[4].Value);

            _matrix[dimensionX, dimensionY, dimensionZ] = isAlive;
        }

        public void Dispose()
        {
            _regex = null;
        }
    }
}
