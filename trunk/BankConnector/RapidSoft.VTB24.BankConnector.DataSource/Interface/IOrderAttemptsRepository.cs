using System;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IOrderAttemptsRepository
    {
        void Save(string clientId);

        void Clear(string clientId);

        string[] Get(DateTime from, int skip, int take);

        void ClearAll();
    }
}
