using GraphQL.Types;
using System;

namespace TicketApi.GraphQL.Schemas
{
    internal class TicketGraphSchema : Schema
    {
        public const string GraphQLAuthPolicyName = "GraphQLAuthorized";

        public TicketGraphSchema(
            TicketGraphQuery query,
            TicketGraphMutation mutation,
            IServiceProvider provider)
            : base(provider)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}
