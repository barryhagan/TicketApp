using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Exceptions;
using TicketCore.Model;
using Xunit;

namespace TicketStorage.InMemory.UnitTests
{
    public class InMemoryTicketStoreTests
    {
        [Fact]
        public async Task throws_when_object_not_found()
        {
            var store = new InMemoryTicketStore();
            await Assert.ThrowsAsync<ObjectNotFoundException<TypeOne, int>>(async () => await store.LoadAsync<TypeOne, int>(10));
        }

        [Fact]
        public async Task can_store_and_retrieve_object()
        {
            var store = new InMemoryTicketStore();
            await store.StoreAsync<TypeOne, int>(new List<TypeOne> { new TypeOne { _id = 10 } });
            var loaded = await store.LoadAsync<TypeOne, int>(10);

            Assert.NotNull(loaded);
            Assert.Equal(10, loaded._id);
        }

        [Fact]
        public async Task can_store_and_delete_object()
        {
            var store = new InMemoryTicketStore();
            await store.StoreAsync<TypeOne, int>(new List<TypeOne> { new TypeOne { _id = 10 } });
            var loaded = await store.LoadAsync<TypeOne, int>(10);
            Assert.NotNull(loaded);
            Assert.Equal(10, loaded._id);
            await store.DeleteAsync<TypeOne, int>(new[] { 10 });
            await Assert.ThrowsAsync<ObjectNotFoundException<TypeOne, int>>(async () => await store.LoadAsync<TypeOne, int>(10));
        }

        [Fact]
        public async Task load_many_can_return_partial_list()
        {
            var store = new InMemoryTicketStore();

            await store.StoreAsync<TypeOne, int>(new List<TypeOne> {
                new TypeOne { _id = 10 },
                new TypeOne { _id = 11 },
                new TypeOne { _id = 12 },
            });

            var loaded = await store.LoadManyAsync<TypeOne, int>(new[] { 10, 11, 12, 13, 14 });
            Assert.NotNull(loaded);
            Assert.Equal(3, loaded.Count());
        }

        [Fact]
        public async Task can_query_data()
        {
            var store = new InMemoryTicketStore();

            await store.StoreAsync<TypeOne, int>(new List<TypeOne> {
                new TypeOne { _id = 10 },
                new TypeOne { _id = 11 },
                new TypeOne { _id = 12 },
            });

            await store.StoreAsync<TypeTwo, string>(new List<TypeTwo> {
                new TypeTwo { _id = "10" },
                new TypeTwo { _id = "11" },
                new TypeTwo { _id = "12" },
            });

            var matches = store.Query<TypeOne, int>().Where(doc => doc._id == 10 || doc._id == 11).ToList();
            Assert.Equal(2, matches.Count);

            var matchesTwo = store.Query<TypeTwo, string>().Where(doc => doc._id == "10" || doc._id == "111").ToList();
            Assert.Single(matchesTwo);
        }


        private class TypeOne : ModelBase<int>
        {
        }

        private class TypeTwo : ModelBase<string>
        {
        }
    }
}
