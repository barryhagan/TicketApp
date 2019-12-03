using GraphQL.Types;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    public class GlobalSearchResultGraphType : ObjectGraphType<GlobalSearchResult>
    {
        public GlobalSearchResultGraphType()
        {
            Name = "SearchResult";

            Field(x => x.organizations, type: typeof(ListGraphType<OrganizationGraphType>));
            Field(x => x.tickets, type: typeof(ListGraphType<TicketGraphType>));
            Field(x => x.users, type: typeof(ListGraphType<UserGraphType>));
        }
    }
}
