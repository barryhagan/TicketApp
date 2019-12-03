using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    public class SearchInputType : InputObjectGraphType<SearchInput>
    {
        public SearchInputType()
        {
            Name = "SearchInput";
            Field(x => x.search);
        }
    }
}

