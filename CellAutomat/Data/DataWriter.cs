using System.IO;
using Newtonsoft.Json;

namespace CellAutomat.Data
{
    internal static class DataWriter
    {
        internal static void SaveMatrix(bool[,,] matrix, string fileName)
        {
            var serializedMatrix = JsonConvert.SerializeObject(matrix);
            File.WriteAllText(fileName, serializedMatrix);
        }

        internal static void SaveMatrixAsJs(bool[,,] matrix, string fileName)
        {
            var serializedMatrix = JsonConvert.SerializeObject(matrix);
            serializedMatrix = ReplaceFirst(serializedMatrix, @"[", "var matrix=[");
            serializedMatrix = ReplaceLastOccurrence(serializedMatrix, @"]", @"];");
            File.WriteAllText(fileName, serializedMatrix);
        }

        public static string ReplaceFirst(string source, string find, string replace)
        {
            var place = source.IndexOf(find, System.StringComparison.Ordinal);
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            var place = source.LastIndexOf(find, System.StringComparison.Ordinal);
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}
