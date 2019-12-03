using GraphQL.Types;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    public class SearchInputType : InputObjectGraphType<SearchInput>
    {
        public SearchInputType()
        {
            Name = "SearchInput";
            Field(x => x.search);
            Field(x => x.docType, nullable: true);
        }
    }
}

