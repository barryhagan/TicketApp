using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    public class ModelBaseGraphType<T, TKey> : ObjectGraphType<T> where T: ModelBase<TKey>
    {
        protected readonly IDataLoaderContextAccessor dataLoader;
        protected readonly TicketBusinessLogic logic;

        public ModelBaseGraphType(IDataLoaderContextAccessor dataLoader, TicketBusinessLogic logic)
        {
            this.dataLoader = dataLoader;
            this.logic = logic;

            if (typeof(TKey) == typeof(int))
            {
                Field<IntGraphType>().Name("id").Resolve(ctx => ctx.Source._id);
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                Field<GuidGraphType>().Name("id").Resolve(ctx => ctx.Source._id);
            }

            Field(x => x.external_id, type: typeof(GuidGraphType));
            Field(x => x.created_at, type: typeof(DateTimeGraphType));
            Field(x => x.url);
            Field(x => x.tags, type: typeof(ListGraphType<StringGraphType>));
        }

        protected async Task<TEntity> DataLoadById<TEntity, TEntityKey>(TEntityKey id) where TEntity : ModelBase<TEntityKey>
        {
            var loadUserInfo = dataLoader.Context.GetOrAddBatchLoader<TEntityKey, TEntity>(
                $"GetEntities<{typeof(TEntity).Name}>",
                async (ids) => await logic.LoadManyAsync<TEntity, TEntityKey>(ids.ToList()),
                u => u._id
                );

            return await loadUserInfo.LoadAsync(id);
        }
    }
}
