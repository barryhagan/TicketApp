using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;

namespace TicketApi.GraphQL.Schemas
{
    internal class TicketGraphMutation : ObjectGraphType
    {       
        public TicketGraphMutation()
        {
            Name = "Mutation";

            this.AuthorizeWith(TicketGraphSchema.GraphQLAuthPolicyName);

            //stub the mutation graph for now
            Field<BooleanGraphType>()
                .Name("ticketUpdate")
                .Resolve(ctx => { return true; });
        }
    }
}
