using System;
using System.Threading.Tasks;

namespace tsc.backend.lib
{
    public interface IBaseCrudHandler<TId, TList, TCreate, TUpdate, TRemove, TResult>
        where TResult : class
        where TCreate : class
        where TUpdate : class
        where TRemove : class
    {
        Task<TResult[]> ListAsync(TList userId);

        Task<TResult> GetDetailsAsync(TList model);

        Task<TResult> CreateAsync(TCreate model);

        Task<TResult> UpdateAsync(TUpdate model);

        Task<Guid> RemoveAsync(TRemove model);
    }
}
