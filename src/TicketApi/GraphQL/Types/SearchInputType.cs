using GraphQL.Types;
using TicketCore.Dto;

namespace TicketApi.GraphQL.Types
{
    internal class SearchInputType : InputObjectGraphType<SearchInput>
    {
        public SearchInputType()
        {
            Name = "SearchInput";

            Field(x => x.Search);
            Field(x => x.DocType, nullable: true);
        }
    }
}

