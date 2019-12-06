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

        [Theory]
        [InlineData("\\\"\\\"")]
        [InlineData("ISNULL")]
        public async Task can_search_empty_email_field(string emptySearch)
        {
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ search:\"email:" + emptySearch + "\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Equal(2, results.globalSearch.users.Count);
            foreach (var user in results.globalSearch.users)
            {
                Assert.Null(user.item.email);
            }
        }

        [Fact]
        public async Task can_perform_global_empty_field_search()
        {
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ search:\"ISNULL\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Equal(3, results.globalSearch.users.Count);
            Assert.Single(results.globalSearch.tickets);
            Assert.Single(results.globalSearch.organizations);
        }


        [Theory]
        [InlineData(1, "_id", "1")]
        [InlineData(1, "url", @"\""http:\/\/initech.zendesk.com\/api\/v2\/users\/7.json\""")]
        [InlineData(1, "external_id", "bce94e82-b4f4-438f-bc0b-2440e8265705")]
        [InlineData(1, "name", "Loraine")]
        [InlineData(1, "alias", "\\\"Miss Coffey\\\"")]
        [InlineData(2, "created_at", "[20160415 TO 20160416]")]
        [InlineData(39, "active", "true")]
        [InlineData(26, "verified", "true")]
        [InlineData(47, "shared", "false")]
        [InlineData(32, "locale", "en-AU")]
        [InlineData(1, "timezone", "Monaco")]
        [InlineData(1, "last_login_at", "20130703*")]
        [InlineData(1, "email", "olapittman@flotonic.com")]
        [InlineData(1, "phone", "9365-482-943")]
        [InlineData(75, "signature", "happy")]
        [InlineData(5, "organization_id", "110")]
        [InlineData(1, "tags", "Hartley")]
        [InlineData(36, "suspended", "true")]
        [InlineData(26, "role", "end-user")]
        public async Task can_search_user_field(int expected_count, string field, string search)
        {
            var fieldSearch = $"{field}:{search}";
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ docType:\"user\" search:\"" + fieldSearch + "\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Equal(expected_count, results.globalSearch.users.Count());
        }

        [Theory]
        [InlineData(1, "_id", "2217c7dc-7371-4401-8738-0a8a8aedc08d")]
        [InlineData(1, "url", @"\""http:\/\/initech.zendesk.com\/api\/v2\/tickets\/1a227508-9f39-427c-8f57-1b72f3fab87c.json\""")]
        [InlineData(1, "external_id", "4a70394c-9b1a-4766-9d41-ef7f61a01a1c")]
        [InlineData(25, "created_at", "[201606 TO 201607]")]
        [InlineData(50, "type", "question")]
        [InlineData(1, "subject", @"\""A Nuisance in Equatorial Guinea\""")]
        [InlineData(1, "description", @"\""Incididunt exercitation voluptate eu laborum proident Lorem minim pariatur. Lorem culpa amet Lorem Lorem commodo anim deserunt do consectetur sunt.\""")]
        [InlineData(64, "priority", "high")]
        [InlineData(37, "status", "hold")]        
        [InlineData(4, "submitter_id", "73")]
        [InlineData(3, "assignee_id", "34")]        
        [InlineData(13, "organization_id", "122")]
        [InlineData(14, "tags", "MAINE")]
        [InlineData(99, "has_incidents", "true")]
        [InlineData(70, "via", "chat")]
        [InlineData(4, "due_at", "20160816*")]
        public async Task can_search_ticket_field(int expected_count, string field, string search)
        {
            var fieldSearch = $"{field}:{search}";
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ docType:\"ticket\" search:\"" + fieldSearch + "\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Equal(expected_count, results.globalSearch.tickets.Count());
        }


        [Theory]
        [InlineData(1, "_id", "125")]
        [InlineData(1, "url", @"\""http:\/\/initech.zendesk.com\/api\/v2\/organizations\/125.json\""")]
        [InlineData(1, "external_id", "33c4e38d-bfa3-4b12-9bb6-6f547524cf33")]
        [InlineData(1, "name", "Hotcâkes")]
        [InlineData(1, "domain_names", "fishland.com")]
        [InlineData(1, "created_at", "[20160410 TO 20160420]")]
        [InlineData(9, "details", "MegaCorp")]
        [InlineData(16, "shared_tickets", "false")]
        [InlineData(1, "tags", "Poole")]
        public async Task can_search_org_field(int expected_count, string field, string search)
        {
            var fieldSearch = $"{field}:{search}";
            var query = SEARCH_QUERY_FORMAT.Replace("input", "input:{ docType:\"organization\" search:\"" + fieldSearch + "\" }");
            var results = await GraphRequestAsync<SearchData>(query);
            Assert.Equal(expected_count, results.globalSearch.organizations.Count());
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

        private class GraphUser : User
        {
            public int id { get { return _id; } set { _id = value; } }
        }

        private class GraphOrganization : Organization
        {
            public int id { get { return _id; } set { _id = value; } }
        }

        private class GraphTicket : Ticket
        {
            public Guid id { get { return _id; } set { _id = value; } }
        }
    }
}
