using GraphQL.DataLoader;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketApi.GraphQL.Types;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketApi.GraphQL.Schemas
{
    public class TicketQuery : ObjectGraphType
    {
        private readonly IDataLoaderContextAccessor dataLoader;
        private readonly TicketBusinessLogic logic;
        private readonly ILogger<TicketQuery> logger;

        public TicketQuery(IDataLoaderContextAccessor dataLoader, TicketBusinessLogic logic, ILogger<TicketQuery> logger)
        {
            Name = "Query";

            this.dataLoader = dataLoader;
            this.logic = logic;
            this.logger = logger;

            this.AuthorizeWith(TicketSchema.GraphQLAuthPolicyName);

            Field<SearchSchemaGraphType>()
                .Name("searchSchema")
                .ResolveAsync(async ctx =>
                {
                    var schema = new Dictionary<string, List<string>>();
                    schema[typeof(Organization).Name] = await logic.GetSearchFields<Organization>();
                    schema[typeof(Ticket).Name] = await logic.GetSearchFields<Ticket>();
                    schema[typeof(User).Name] = await logic.GetSearchFields<User>();
                    return schema;
                });

            Field<GlobalSearchResultGraphType>()
                .Name("globalSearch")
                .Argument<NonNullGraphType<SearchInputType>>("input", "The entity to delete notifications for.")
                .ResolveAsync(async ctx =>
                {
                    var input = ctx.GetArgument<SearchInput>("input");
                    return await logic.SearchAsync(input);
                });

            FieldAsync<OrganizationGraphType>(
               "organization",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>>
                   {
                       Name = "id",
                       Description = "The id of the organization."
                   }),
               resolve: async context => await DataLoadById<Organization, int>(context.GetArgument<int>("id")));

            FieldAsync<UserGraphType>(
               "user",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>>
                   {
                       Name = "id",
                       Description = "The id of the user."
                   }),
               resolve: async context => await DataLoadById<User, int>(context.GetArgument<int>("id")));


            FieldAsync<TicketGraphType>(
               "ticket",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<GuidGraphType>>
                   {
                       Name = "id",
                       Description = "The id of the ticket."
                   }),
               resolve: async context => await DataLoadById<Ticket, Guid>(context.GetArgument<Guid>("id")));
        }


        protected async Task<TEntity> DataLoadById<TEntity, TKey>(TKey id) where TEntity : ModelBase<TKey>
        {
            var loadUserInfo = dataLoader.Context.GetOrAddBatchLoader<TKey, TEntity>(
                $"GetEntities<{typeof(TEntity).Name}>",
                async (ids) => await logic.LoadManyAsync<TEntity, TKey>(ids.ToList()),
                u => u._id
                );

            return await loadUserInfo.LoadAsync(id);
        }

    }
}
