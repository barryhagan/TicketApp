using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketApi
{
    public class TicketBusinessLogic
    {
        private readonly ITicketStore store;
        private readonly ITicketSearch search;
        private readonly ILogger<TicketBusinessLogic> logger;
        private readonly IDataLoader dataLoader;

        public TicketBusinessLogic(ITicketStore store, ITicketSearch search, IDataLoader dataLoader, ILogger<TicketBusinessLogic> logger)
        {
            this.store = store;
            this.search = search;
            this.logger = logger;
            this.dataLoader = dataLoader;
        }

        public async Task InitializeData()
        {
            if (store.Query<User, int>().Any() || store.Query<Organization, int>().Any() || store.Query<Ticket, Guid>().Any())
            {
                logger.LogDebug("There is already data in the system of record.  Skipping data initialization.");
                return;
            }

            var orgs = dataLoader.LoadObjects<Organization>().ToList();
            await store.StoreAsync<Organization, int>(orgs);
            await search.AddDocuments<Organization, int>(orgs);
            logger.LogInformation($"Loaded {orgs.Count} organizations");

            var users = dataLoader.LoadObjects<User>().ToList();
            await store.StoreAsync<User, int>(users);
            await search.AddDocuments<User, int>(users);
            logger.LogInformation($"Loaded {users.Count} users");

            var tickets = dataLoader.LoadObjects<Ticket>().ToList();
            await store.StoreAsync<Ticket, Guid>(tickets);
            await search.AddDocuments<Ticket, Guid>(tickets);
            logger.LogInformation($"Loaded {tickets.Count} tickets");
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
            var searchHits = await search.Search(input);

            // TODO: optimize for O(n) instead of O(3)
            var userIds = searchHits.Where(h => h.DocType == typeof(User).Name.ToLowerInvariant()).Select(u => Convert.ToInt32(u.DocId)).ToList();
            var ticketIds = searchHits.Where(h => h.DocType == typeof(Ticket).Name.ToLowerInvariant()).Select(u => Guid.Parse(u.DocId)).ToList();
            var orgIds = searchHits.Where(h => h.DocType == typeof(Organization).Name.ToLowerInvariant()).Select(u => Convert.ToInt32(u.DocId)).ToList();

            var dataResult = new GlobalSearchResult
            {
                users = (await store.LoadManyAsync<User, int>(userIds)).ToList(),
                tickets = (await store.LoadManyAsync<Ticket, Guid>(ticketIds)).ToList(),
                organizations = (await store.LoadManyAsync<Organization, int>(orgIds)).ToList()
            };

            return dataResult;
        }
    }
}
