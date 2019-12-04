using System.Collections.Generic;
using System.Threading.Tasks;
using TicketCore.Dto;
using TicketCore.Model;

namespace TicketCore.Interfaces
{
    public interface ITicketSearch
    {
        Task AddDocumentsAsync<T, TKey>(IEnumerable<T> docs) where T : ModelBase<TKey>;
        Task<List<SearchHit>> SearchAsync(SearchInput search);
        Task<List<string>> GetSearchFieldsAsync<T>();
    }
}
