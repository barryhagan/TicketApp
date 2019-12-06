using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Dto;
using TicketCore.Interfaces;
using TicketCore.Model;
using Xunit;

namespace TicketBusinessLogic.UnitTests
{
    public class BusinessLogicTests
    {
        private Mock<IDataLoader> mockLoader = new Mock<IDataLoader>();
        private Mock<ITicketSearch> mockSearch = new Mock<ITicketSearch>();
        private Mock<ITicketStore> mockStore = new Mock<ITicketStore>();
        private BusinessLogic logic;

        public BusinessLogicTests()
        {
            SetupMocks();
            logic = new BusinessLogic(mockStore.Object, mockSearch.Object, mockLoader.Object, new Mock<ILogger<BusinessLogic>>().Object);
        }

        private List<User> mockStorage = new List<User>();

        private void SetupMocks()
        {
            var userData = new List<User>
            {
                new User
                {
                    _id= 1,
                    email = "user1@localhost"
                },
                new User
                {
                    _id = 2,
                    email = "user2@localhost"
                }
            };

            var ticketData = new List<Ticket>
            {
                new Ticket
                {
                    _id = Guid.NewGuid(),
                },
                new Ticket
                {
                    _id = Guid.NewGuid()
                }
            };

            var orgData = new List<Organization>
            {
                new Organization
                {
                    _id = 100
                },
                new Organization
                {
                    _id = 101
                }
            };

            mockLoader.Setup(loader => loader.LoadObjects<User>()).Returns(userData.AsEnumerable());
            mockLoader.Setup(loader => loader.LoadObjects<Ticket>()).Returns(ticketData.AsEnumerable());
            mockLoader.Setup(loader => loader.LoadObjects<Organization>()).Returns(orgData.AsEnumerable());

            mockStore.Setup(store => store.StoreAsync<User, int>(It.IsAny<IEnumerable<User>>()))
                .Callback<IEnumerable<User>>(e => mockStorage.AddRange(e));
            mockStore.Setup(store => store.LoadManyAsync<User, int>(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync((IEnumerable<int> ids) => { return mockStorage.Where(u => ids.Contains(u._id)).ToList(); });


            mockSearch.Setup(search => search.SearchAsync(It.IsAny<SearchInput>())).ReturnsAsync(new List<SearchHit>
            {
                new SearchHit
                {
                    DocId = "10",
                    DocType = "user",
                    Score = .77f
                },
                new SearchHit
                {
                    DocId = Guid.NewGuid().ToString(),
                    DocType= "ticket",
                    Score = .65f

                }
            });
        }

        [Fact]
        public async Task can_init_store()
        {
            await logic.InitializeDataAsync();
            var users = await logic.LoadManyAsync<User, int>(new[] { 1, 2 });
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task can_retrieve_search_results()
        {
            var results = await logic.SearchAsync(new SearchInput { Search = "_id:10 OR _id:101" });

            Assert.Empty(results.Organizations);
            Assert.Single(results.Users);
            Assert.Equal(10, results.Users.Single().Key);
            Assert.Single(results.Tickets);
            Assert.NotEqual(Guid.Empty, results.Tickets.Single().Key);
        }
    }
}
