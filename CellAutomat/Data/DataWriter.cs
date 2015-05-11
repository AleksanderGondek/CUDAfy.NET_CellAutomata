using System.IO;

namespace CellAutomat.Data
{
    internal static class DataWriter
    {
        internal static void SaveMatrix(bool[,,] matrix, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                var matrixSize = matrix.GetLength(0);
                streamWriter.WriteLine(matrixSize);
                for (var z = 0; z < matrixSize; z++)
                {
                    for (var x = 0; x < matrixSize; x++)
                    {
                        for (var y = 0; y < matrixSize; y++)
                        {
                            streamWriter.WriteLine(GetSingleLine(x, y, z, matrix[x,y,z]));
                        }
                    }
                }
            }
        }

        private static string GetSingleLine(int x, int y, int z, bool value)
        {
            return string.Format("{{{0},{1},{2}}}={3}", x, y, z, value.ToString().ToLower());
        }
    }
}
