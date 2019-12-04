using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Dto;
using TicketCore.Exceptions;
using TicketCore.Model;
using Xunit;

namespace TicketSearch.Lucene.UnitTests
{
    public class InMemoryLuceneSearchTests
    {
        private InMemoryLuceneSearch search = new InMemoryLuceneSearch();

        [Fact]
        public async Task can_get_search_fields_for_user()
        {
            var userFields = await search.GetSearchFieldsAsync<User>();
            Assert.Equal(19, userFields.Count());
        }

        [Fact]
        public async Task can_get_search_fields_for_org()
        {
            var orgFields = await search.GetSearchFieldsAsync<Organization>();
            Assert.Equal(9, orgFields.Count());
        }

        [Fact]
        public async Task can_get_search_fields_for_ticket()
        {
            var ticketFields = await search.GetSearchFieldsAsync<Ticket>();
            Assert.Equal(16, ticketFields.Count());
        }

        [Fact]
        public async Task can_add_and_search_user_documents()
        {
            await search.AddDocumentsAsync<User, int>(new List<User>
            {
                new User{_id=10, email = "test10@localhost"},
                new User{_id=11, email = "test11@localhost"}
            });

            var idHit = await search.SearchAsync(new SearchInput { DocType = "User", Search = "_id:10" });
            Assert.Single(idHit);
            Assert.Equal("10", idHit.Single().DocId);
            Assert.Equal("user", idHit.Single().DocType);

            var emailHit = await search.SearchAsync(new SearchInput { DocType = "User", Search = "email:\"test11@local\"" });
            Assert.Single(emailHit);
            Assert.Equal("11", emailHit.Single().DocId);
            Assert.Equal("user", emailHit.Single().DocType);
        }

        [Fact]
        public async Task can_add_and_search_org_documents()
        {
            await search.AddDocumentsAsync<Organization, int>(new List<Organization>
            {
                new Organization{_id=10 },
                new Organization{_id=11 }
            });

            var idHit = await search.SearchAsync(new SearchInput { DocType = "Organization", Search = "_id:10" });
            Assert.Single(idHit);
            Assert.Equal("10", idHit.Single().DocId);
            Assert.Equal("organization", idHit.Single().DocType);

            var wrongType = await search.SearchAsync(new SearchInput { DocType = "User", Search = "_id:10" });
            Assert.Empty(wrongType);
        }

        [Fact]
        public async Task can_add_and_search_ticket_documents()
        {
            var myId = Guid.NewGuid();
            await search.AddDocumentsAsync<Ticket, Guid>(new List<Ticket>
            {
                new Ticket{_id=Guid.NewGuid() },
                new Ticket{_id=myId, external_id = Guid.Empty }
            });

            var externalHit = await search.SearchAsync(new SearchInput { DocType = "Ticket", Search = $"external_id:{Guid.Empty.ToString()}" });
            Assert.Single(externalHit);
            Assert.Equal(myId.ToString(), externalHit.Single().DocId);
            Assert.Equal("ticket", externalHit.Single().DocType);
        }

        [Fact]
        public async Task throws_adding_unknown_doc_type()
        {
            await Assert.ThrowsAsync<TicketAppException>(async () => await search.AddDocumentsAsync<TestDoc, string>(new[] { new TestDoc() }));
        }

        private class TestDoc : ModelBase<string>
        {

        }
    }
}
