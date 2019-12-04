using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Exceptions;
using TicketCore.Model;
using Xunit;

namespace TicketApi.IntegrationTests.Search
{
    public class SearchTests : TicketIntegrationTest
    {
        private const string SEARCH_QUERY_FORMAT = "{ globalSearch(input) { organizations { score item {id details external_id name tags } } tickets { score item { id subject via status external_id tags via priority type organization { name } } }  users { score item { id external_id name alias email created_at tags } } } }";

        public SearchTests(TicketTestHost testHost) : base(testHost) { }

        [Fact]
        public async Task throws_on_missing_input()
        {
            await Assert.ThrowsAsync<TicketAppException>(async () => await GraphRequestAsync<SearchData>("{globalSearch}"));
        }

        [Fact]
        public async Task can_search_user_by_id()
        {
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ search:\"_id:11\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Single(results.globalSearch.users);
            Assert.Equal(11, results.globalSearch.users.Single().item._id);
            Assert.Empty(results.globalSearch.tickets);
            Assert.Empty(results.globalSearch.organizations);
        }

        [Fact]
        public async Task can_search_by_empty_email()
        {
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ search:\"email:ISNULL\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Equal(2, results.globalSearch.users.Count);
            foreach (var user in results.globalSearch.users)
            {
                Assert.Null(user.item.email);
            }
        }

        private class SearchData
        {
            public Search globalSearch { get; set; }
        }

        private class Search
        {
            public List<ScoredResult<GraphUser>> users { get; set; }
            public List<ScoredResult<GraphTicket>> tickets { get; set; }
            public List<ScoredResult<GraphOrganization>> organizations { get; set; }
        }

        private class ScoredResult<T>
        {
            public float score { get; set; }
            public T item { get; set; }
        }

        public class GraphUser : User
        {
            public int id { get { return _id; } set { _id = value; } }
        }

        public class GraphOrganization : Organization
        {
            public int id { get { return _id; } set { _id = value; } }
        }

        public class GraphTicket : Ticket
        {
            public Guid id { get { return _id; } set { _id = value; } }
        }
    }
}
