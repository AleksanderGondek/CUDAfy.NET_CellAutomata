using System.IO;
using Newtonsoft.Json;

namespace CellAutomat.Data
{
    public static class DataHandler
    {
        public static bool[, ,] LoadMatrix()
        {
            var textContent = File.ReadAllText(AppConfigHelper.GetValueFromAppSettings(@"CellMatrixDataLocation"));
            return JsonConvert.DeserializeObject<bool[, ,]>(textContent);
        }

        public static void SerializeAndSaveMatrix(bool[, ,] matrix, string fileName)
        {
            var serializedMatrix = JsonConvert.SerializeObject(matrix);
            File.WriteAllText(fileName, serializedMatrix);
        }

        public static void SaveMatrix(bool[, ,] matrix, string fileName)
        {
            var serializedMatrix = JsonConvert.SerializeObject(matrix);
            serializedMatrix = ReplaceFirst(serializedMatrix, @"[", "var matrix=[");
            serializedMatrix = ReplaceLastOccurrence(serializedMatrix, @"]", @"];");
            File.WriteAllText(fileName, serializedMatrix);
        }

        public static void PrepareVisualisation(string fileName)
        {
            var template = File.ReadAllText(AppConfigHelper.GetValueFromAppSettings(@"VisualisationTemplateLocation"));
            var replaceMeValue = fileName.Replace(@"Output\", string.Empty);
            replaceMeValue = replaceMeValue.Replace(@"\", @"/");
            template = template.Replace(@"please_replace_me.js", replaceMeValue);
            fileName = fileName.Replace(@".js", @".html");
            fileName = fileName.Replace(@"data", string.Empty);
            fileName = fileName.Replace(@"\", @"/");
            File.WriteAllText(fileName, template);
        }

        private static string ReplaceFirst(string source, string find, string replace)
        {
            var place = source.IndexOf(find, System.StringComparison.Ordinal);
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        private static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            var place = source.LastIndexOf(find, System.StringComparison.Ordinal);
            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}
