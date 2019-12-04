using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TicketCore.Dto;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketBusinessLogic
{
    public class BusinessLogic
    {
        private readonly ITicketStore store;
        private readonly ITicketSearch search;
        private readonly ILogger<BusinessLogic> logger;
        private readonly IDataLoader dataLoader;

        public BusinessLogic(ITicketStore store, ITicketSearch search, IDataLoader dataLoader, ILogger<BusinessLogic> logger)
        {
            this.store = store;
            this.search = search;
            this.logger = logger;
            this.dataLoader = dataLoader;
        }

        public async Task InitializeDataAsync()
        {
            if (store.Query<User, int>().Any() || store.Query<Organization, int>().Any() || store.Query<Ticket, Guid>().Any())
            {
                logger.LogDebug("There is already data in the system of record.  Skipping data initialization.");
                return;
            }

            var orgs = dataLoader.LoadObjects<Organization>().ToList();
            await store.StoreAsync<Organization, int>(orgs);
            await search.AddDocumentsAsync<Organization, int>(orgs);
            logger.LogInformation($"Loaded {orgs.Count} organizations");

            var users = dataLoader.LoadObjects<User>().ToList();
            await store.StoreAsync<User, int>(users);
            await search.AddDocumentsAsync<User, int>(users);
            logger.LogInformation($"Loaded {users.Count} users");

            var tickets = dataLoader.LoadObjects<Ticket>().ToList();
            await store.StoreAsync<Ticket, Guid>(tickets);
            await search.AddDocumentsAsync<Ticket, Guid>(tickets);
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

        public async Task<List<string>> GetSearchFieldsAsync<T>()
        {
            return await search.GetSearchFieldsAsync<T>();
        }

        public async Task<SearchResultSet> SearchAsync(SearchInput input)
        {
            var searchHits = await search.SearchAsync(input);
            var searchResults = new SearchResultSet();

            foreach (var hit in searchHits)
            {
                switch (hit.DocType)
                {
                    case "user":
                        searchResults.Users.Add(Convert.ToInt32(hit.DocId), hit.Score);
                        break;
                    case "organization":
                        searchResults.Organizations.Add(Convert.ToInt32(hit.DocId), hit.Score);
                        break;
                    case "ticket":
                        searchResults.Tickets.Add(Guid.Parse(hit.DocId), hit.Score);
                        break;
                }
            }

            return searchResults;
        }
    }
}
