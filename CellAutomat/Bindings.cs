using CellAutomat.Data;
using Ninject.Modules;

namespace CellAutomat
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            // Bind stuff right here
            Bind<IDataCreator>().To<DataCreator>();
            Bind<IDataLoader>().To<DataLoader>();
        }
    }
}
