using System;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Dto;
using TicketCore.Exceptions;
using TicketCore.Model;
using Xunit;

namespace TicketBusinessLogic.UnitTests
{
    public class EmbeddedJsonDataLoaderTests
    {
        [Fact]
        public void can_load_user_source_data()
        {
            var loader = new EmbeddedJsonDataLoader();
            Assert.Equal(75, loader.LoadObjects<User>().Count());
        }

        [Fact]
        public void can_load_ticket_source_data()
        {
            var loader = new EmbeddedJsonDataLoader();
            Assert.Equal(200, loader.LoadObjects<Ticket>().Count());
        }

        [Fact]
        public void can_load_organization_source_data()
        {
            var loader = new EmbeddedJsonDataLoader();
            Assert.Equal(26, loader.LoadObjects<Organization>().Count());
        }

        [Fact]
        public void throws_for_invalid_object_type()
        {
            var loader = new EmbeddedJsonDataLoader();
            Assert.Throws<TicketAppException>(loader.LoadObjects<SearchHit>);
        }
    }
}
