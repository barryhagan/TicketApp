﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Exceptions;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketStorage.InMemory
{
    /// <summary>
    /// A simple in-memory data store using concurrent dictionaries keyed by document ids
    /// </summary>
    public class InMemoryTicketStore : ITicketStore
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<dynamic, dynamic>> dataStore = new ConcurrentDictionary<string, ConcurrentDictionary<dynamic, dynamic>>();

        public Task<T> LoadAsync<T, TKey>(TKey id) where T : ModelBase<TKey>
        {
            var key = typeof(T).Name;
            var typeStore = LoadTypeStore(key);

            if (typeStore.TryGetValue(id, out var obj))
            {
                return Task.FromResult(obj as T);
            }

            throw new ObjectNotFoundException<T, TKey>(id, $"The {key} does not exist.");
        }

        public Task<IList<T>> LoadManyAsync<T, TKey>(IEnumerable<TKey> ids) where T : ModelBase<TKey>
        {
            var key = typeof(T).Name;
            var typeStore = LoadTypeStore(key);

            IList<T> objList = new List<T>();

            foreach (var id in ids)
            {
                if (typeStore.TryGetValue(id, out var obj))
                {
                    objList.Add(obj as T);
                }
            }

            return Task.FromResult(objList);
        }

        public Task DeleteAsync<T, TKey>(IEnumerable<TKey> ids) where T: ModelBase<TKey>
        {
            var key = typeof(T).Name;
            var typeStore = LoadTypeStore(key);
            foreach (var id in ids)
            {
                typeStore.TryRemove(id, out var delete);
            }
            return Task.CompletedTask;
        }

        public Task StoreAsync<T, TKey>(IEnumerable<T> objects) where T : ModelBase<TKey>
        {
            var key = typeof(T).Name;
            var typeStore = LoadTypeStore(key);
            foreach (var obj in objects)
            {
                typeStore.AddOrUpdate(obj._id, obj, (i, o) => obj);
            }
            return Task.CompletedTask;
        }

        public IQueryable<T> Query<T, TKey>() where T : ModelBase<TKey>
        {
            var key = typeof(T).Name;
            var typeStore = LoadTypeStore(key);
            return typeStore.Values.Select(v => v as T).AsQueryable();
        }

        private ConcurrentDictionary<dynamic, dynamic> LoadTypeStore(string key)
        {
            if (!dataStore.TryGetValue(key, out var typeStore))
            {
                typeStore = dataStore.AddOrUpdate(key, new ConcurrentDictionary<dynamic, dynamic>(), (k, d) => d);
            }
            return typeStore;
        }

    }
}
