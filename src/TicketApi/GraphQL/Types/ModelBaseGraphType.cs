using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading.Tasks;
using TicketBusinessLogic;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    internal class ModelBaseGraphType<T, TKey> : ObjectGraphType<T> where T: ModelBase<TKey>
    {
        protected readonly IDataLoaderContextAccessor dataLoader;
        protected readonly BusinessLogic logic;

        public ModelBaseGraphType(IDataLoaderContextAccessor dataLoader, BusinessLogic logic)
        {
            this.dataLoader = dataLoader;
            this.logic = logic;

            if (typeof(TKey) == typeof(int))
            {
                // leading underscores are not allowed in gql
                Field<IntGraphType>().Name("id").Resolve(ctx => ctx.Source._id);
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                // leading underscores are not allowed in gql
                Field<GuidGraphType>().Name("id").Resolve(ctx => ctx.Source._id);
            }

            Field(x => x.created_at, type: typeof(DateTimeGraphType));
            Field(x => x.external_id, type: typeof(GuidGraphType));
            Field(x => x.tags, type: typeof(ListGraphType<StringGraphType>));
            Field(x => x.url, nullable: true);
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
