using System;
using System.Collections.Generic;
using System.Text;

namespace TicketCore.Model
{
    public class GlobalSearchResult
    {
        public List<User> users { get; set; } = new List<User>();
        public List<Ticket> tickets { get; set; } = new List<Ticket>();
        public List<Organization> organizations { get; set; } = new List<Organization>();
    }
}
