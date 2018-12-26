using System.Threading.Tasks;

namespace Common
{
    public interface IDataLoader
    {
        Task<T> GetIntValueAsync<T>(bool throwing);
    }
}