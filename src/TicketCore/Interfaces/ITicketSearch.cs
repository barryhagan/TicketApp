using System.Collections.Generic;
using System.Threading.Tasks;
using TicketCore.Model;

namespace TicketCore.Interfaces
{
    public interface ITicketSearch
    {
        Task AddDocuments<T, TKey>(IEnumerable<T> docs) where T : ModelBase<TKey>;
        Task<List<SearchHit>> Search(SearchInput search);
        Task<List<string>> GetSearchFields<T>();
    }
}
