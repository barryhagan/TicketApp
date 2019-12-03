using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketCore.Model;

namespace TicketCore.Interfaces
{
    public interface ITicketStore
    {
        Task<T> LoadAsync<T, TKey>(TKey id) where T : ModelBase<TKey>;
        Task<IList<T>> LoadManyAsync<T, TKey>(IEnumerable<TKey> ids) where T : ModelBase<TKey>;
        Task DeleteAsync<T, TKey>(IEnumerable<TKey> id) where T : ModelBase<TKey>;
        Task StoreAsync<T, TKey>(IEnumerable<T> objects) where T : ModelBase<TKey>;
        IQueryable<T> Query<T, TKey>() where T : ModelBase<TKey>;
    }
}
