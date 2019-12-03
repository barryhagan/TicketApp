using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;
using Microsoft.Extensions.Logging;

namespace TicketApi.GraphQL.Schemas
{
    public class TicketMutation : ObjectGraphType
    {       
        public TicketMutation(ILogger<TicketMutation> logger)
        {
            Name = "Mutation";

            this.AuthorizeWith(TicketSchema.GraphQLAuthPolicyName);

            //TODO : Add actual mutation code when required
            Field<BooleanGraphType>()
                .Name("ticketUpdate")
                .Resolve(ctx => { return true; });
        }
    }
}
