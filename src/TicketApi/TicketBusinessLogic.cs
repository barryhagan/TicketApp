using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketApi
{
    public partial class TicketBusinessLogic
    {
        private readonly ITicketStore store;
        private readonly ITicketSearch search;
        private readonly ILogger<TicketBusinessLogic> logger;

        public TicketBusinessLogic(ITicketStore store, ITicketSearch search, ILogger<TicketBusinessLogic> logger)
        {
            this.store = store;
            this.search = search;
            this.logger = logger;
        }

        public async Task<IList<T>> LoadManyAsync<T, TKey>(IEnumerable<TKey> ids) where T : ModelBase<TKey>
        {
            return await store.LoadManyAsync<T, TKey>(ids);
        }

        public IQueryable<T> Query<T, TKey>() where T : ModelBase<TKey>
        {
            return store.Query<T, TKey>();
        }

        public async Task<List<string>> GetSearchFields<T>()
        {
            return await search.GetSearchFields<T>();
        }

        public async Task<GlobalSearchResult> SearchAsync(SearchInput input)
        {
            var searchResult = await search.Search(input);

            var dataResult = new GlobalSearchResult
            {
                users = (await store.LoadManyAsync<User, int>(searchResult.users.Select(u => u._id))).ToList(),
                tickets = (await store.LoadManyAsync<Ticket, Guid>(searchResult.tickets.Select(u => u._id))).ToList(),
                organizations = (await store.LoadManyAsync<Organization, int>(searchResult.organizations.Select(u => u._id))).ToList()
            };
            return dataResult;
        }
    }
}
