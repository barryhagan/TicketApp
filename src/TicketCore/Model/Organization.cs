using System.Collections.Generic;

namespace TicketCore.Model
{
    public class Organization : ModelBase<int>
    {
        public string name { get; set; }
        public List<string> domain_names { get; set; }
        public string details { get; set; }
        public bool shared_tickets { get; set; }
    }
}
