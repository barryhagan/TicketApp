using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketApi.GraphQL.Schemas
{
    public class TicketSchema : Schema
    {
        public const string GraphQLAuthPolicyName = "GraphQLAuthorized";

        public TicketSchema(
            TicketQuery query,
            TicketMutation mutation,
            IServiceProvider provider)
            : base(provider)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}
