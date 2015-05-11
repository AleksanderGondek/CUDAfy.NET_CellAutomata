using System;

namespace CellAutomat.Data
{
    public interface IDataLoader : IDisposable
    {
        bool[,,] LoadMatrix();
    }
}
