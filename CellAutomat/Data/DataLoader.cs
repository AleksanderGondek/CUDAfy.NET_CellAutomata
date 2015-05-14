using System.IO;
using Newtonsoft.Json;

namespace CellAutomat.Data
{
    public sealed class DataLoader : IDataLoader
    {
        public bool[,,] LoadMatrix()
        {
            var textContent = File.ReadAllText(AppConfigHelper.GetValueFromAppSettings(@"CellMatrixDataLocation"));
            return JsonConvert.DeserializeObject<bool[,,]>(textContent);
        }

        public void Dispose()
        {
            //
        }
    }
}
