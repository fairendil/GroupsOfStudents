using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataAccessLayer
{
    class DataLoader: IDataLoader
    {
        public async Task<T> GetIntValueAsync<T>(bool throwing)
        {
            await Task.Delay(1200);
            if (throwing)
            {
                throw new TimeoutException("Database Timeout");
            }

            return (T)Convert.ChangeType(2, typeof(T));
        }
    }
}
