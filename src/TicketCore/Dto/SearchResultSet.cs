using System;
using System.Collections.Generic;

namespace TicketCore.Dto
{
    public class SearchResultSet
    {
        public Dictionary<int, float> Users { get; set; } = new Dictionary<int, float>();
        public Dictionary<Guid, float> Tickets { get; set; } = new Dictionary<Guid, float>();
        public Dictionary<int, float> Organizations { get; set; } = new Dictionary<int, float>();
    }
}
