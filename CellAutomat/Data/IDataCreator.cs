using System;

namespace CellAutomat.Data
{
    public interface IDataCreator : IDisposable
    {
        int MatrixSize { get; set; }
        int ChanceOfTrue { get; set; }

        bool[,,] CreateStartupMatrix();
    }
}
