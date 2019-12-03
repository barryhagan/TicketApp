using GraphQL.Types;
using System.Collections.Generic;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    public class SearchSchemaGraphType : ObjectGraphType<Dictionary<string, List<string>>>
    {
        public SearchSchemaGraphType()
        {
            Name = "SearchSchema";

            Field<ListGraphType<StringGraphType>>()
                .Name("organization")
                .Resolve(ctx => ctx.Source[typeof(Organization).Name]);

            Field<ListGraphType<StringGraphType>>()
                .Name("ticket")
                .Resolve(ctx => ctx.Source[typeof(Ticket).Name]);

            Field<ListGraphType<StringGraphType>>()
                .Name("user")
                .Resolve(ctx => ctx.Source[typeof(User).Name]);
        }
    }
}
