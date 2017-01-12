namespace RapidSoft.VTB24.BankConnector.DataSource.Repository
{
    using System;
    using System.Linq;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;

    public class OrderAttemptsRepository : IOrderAttemptsRepository
    {
        private readonly BankConnectorDBContext _context;

        public OrderAttemptsRepository(BankConnectorDBContext context)
        {
            _context = context;
        }

        public void Save(string clientId)
        {
            var attempt = _context.OrderAttempts.FirstOrDefault(a => a.ClientId == clientId);

            if (attempt == null)
            {
                attempt = new OrderAttempt
                {
                    ClientId = clientId
                };
                _context.OrderAttempts.Add(attempt);
            }

            attempt.InsertedDate = DateTime.Now;
        }

        public void Clear(string clientId)
        {
            var attempt = _context.OrderAttempts.FirstOrDefault(a => a.ClientId == clientId);

            if (attempt != null)
            {
                _context.OrderAttempts.Remove(attempt);
            }
        }

        public string[] Get(DateTime from, int skip, int take)
        {
            var clientIds = _context
                .OrderAttempts
                .Where(a => a.InsertedDate >= from)
                .OrderBy(a => a.InsertedDate)
                .Skip(skip)
                .Take(take)
                .Select(a => a.ClientId)
                .ToArray();

            return clientIds;
        }

        public void ClearAll()
        {
            foreach (var attempt in _context.OrderAttempts)
            {
                _context.OrderAttempts.Remove(attempt);
            }
        }
    }
}
