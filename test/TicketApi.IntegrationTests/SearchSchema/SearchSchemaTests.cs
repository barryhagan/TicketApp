using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketCore.Exceptions;
using Xunit;

namespace TicketApi.IntegrationTests.SearchSchema
{   
    public class SearchSchemaTests : TicketIntegrationTest
    {
        private const string SCHEMA_QUERY_ALL = "{searchSchema { user ticket organization } }";
        private const string SCHEMA_QUERY_USER = "{searchSchema { user  } }";
        private const string SCHEMA_QUERY_TICKET = "{searchSchema { ticket } }";
        private const string SCHEMA_QUERY_ORG = "{searchSchema {  organization } }";

        public SearchSchemaTests(TicketTestHost testHost) : base(testHost) { }

        [Fact]
        public async Task throws_on_bad_query_field()
        {
            await Assert.ThrowsAsync<TicketAppException>(async () => await GraphRequestAsync<SearchSchemaData>("{searchSchema { other }}"));
        }

        [Fact]
        public async Task can_load_all_search_schema()
        {
            var schema = await GraphRequestAsync<SearchSchemaData>(SCHEMA_QUERY_ALL);
            Assert.Equal(19, schema.searchSchema.user.Count);
            Assert.Equal(16, schema.searchSchema.ticket.Count);
            Assert.Equal(9, schema.searchSchema.organization.Count);
        }

        [Fact]
        public async Task can_load_user_search_schema()
        {
            var schema = await GraphRequestAsync<SearchSchemaData>(SCHEMA_QUERY_USER);
            Assert.Equal(19, schema.searchSchema.user.Count);
            Assert.Contains("email", schema.searchSchema.user);
            Assert.Null(schema.searchSchema.ticket);
            Assert.Null(schema.searchSchema.organization);
        }

        [Fact]
        public async Task can_load_org_search_schema()
        {
            var schema = await GraphRequestAsync<SearchSchemaData>(SCHEMA_QUERY_ORG);
            Assert.Equal(9, schema.searchSchema.organization.Count);
            Assert.Contains("domain_names", schema.searchSchema.organization);
            Assert.Null(schema.searchSchema.ticket);
            Assert.Null(schema.searchSchema.user);
        }

        [Fact]
        public async Task can_load_ticket_search_schema()
        {
            var schema = await GraphRequestAsync<SearchSchemaData>(SCHEMA_QUERY_TICKET);
            Assert.Equal(16, schema.searchSchema.ticket.Count);
            Assert.Contains("subject", schema.searchSchema.ticket);
            Assert.Null(schema.searchSchema.organization);
            Assert.Null(schema.searchSchema.user);
        }

        private class SearchSchemaData
        {
            public SearchSchema searchSchema { get; set; }
        }

        private class SearchSchema
        {
            public List<string> user { get; set; }
            public List<string> ticket { get; set; }
            public List<string> organization { get; set; }
        }


    }
}
